using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Couchbase;
using Couchbase.Authentication;
using Couchbase.Configuration.Client;

namespace PYPA.MicroServices.Infra.Config
{
    //Font: https://github.com/couchbaselabs/try-cb-dotnet
    public class CouchbaseConfig
    {
        CouchbaseConfigModel config;
        private static readonly List<string> IndexNames = new List<string>
        {
            "def_ip",
            "def_url",
            "def_browser",
            "def_params",
            "def_type"
        };
        public CouchbaseConfig(IOptions<CouchbaseConfigModel> options)
        {
            config = options.Value;
        }

        public void Register()
        {
            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri(config.CouchbaseServer) }
            });
            
            ClusterHelper.Get().Authenticate(new PasswordAuthenticator(config.Username, config.Password));

            EnsureIndexes(config.Bucket);
        }

        private static void EnsureIndexes(string bucketName)
        {
            var bucket = ClusterHelper.GetBucket(bucketName);
            var bucketManager = bucket.CreateManager();

            var indexes = bucketManager.ListN1qlIndexes();
            if (!indexes.Any(index => index.IsPrimary))
            {
                bucketManager.CreateN1qlPrimaryIndex(true);
            }

            var missingIndexes = IndexNames.Except(indexes.Where(x => !x.IsPrimary).Select(x => x.Name)).ToList();
            if (!missingIndexes.Any())
            {
                return;
            }

            foreach (var missingIndex in missingIndexes)
            {
                var propertyName = missingIndex.Replace("def_", string.Empty);
                bucketManager.CreateN1qlIndex(missingIndex, true, propertyName);
            }

            bucketManager.BuildN1qlDeferredIndexes();
            bucketManager.WatchN1qlIndexes(missingIndexes, TimeSpan.FromSeconds(30));
        }
    }
}
