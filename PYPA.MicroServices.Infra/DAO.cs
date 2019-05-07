using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using Microsoft.Extensions.Options;
using PYPA.MicroServices.Infra.Config;
using System.Collections.Generic;

namespace PYPA.MicroServices.Infra
{
    public class DAO<T>
    {
        protected readonly IBucket bucket;
        public DAO(IOptions<CouchbaseConfigModel> options)
        {
            bucket = ClusterHelper.GetBucket(options.Value.Bucket);
        }

        public List<T> Execute(string queryStr, params object[] @params)
        {
            var query = new QueryRequest()
               .Statement(@queryStr)
               .AddPositionalParameter(@params);
            var queryResult = bucket.Query<T>(query);

            return queryResult.Rows;
        }
    }
}
