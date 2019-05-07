using PYPA.MicroServices.RabbitMQACL;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PYPA.MicroServices.Infra.Config;
using PYPA.MicroServices.RabbitMQACL.Config;
using System.IO;
using System.Text;
using System.Threading;
using PYPA.MicroServices.Core;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Couchbase;
using Couchbase.Core;
using Microsoft.Extensions.Options;

namespace PYPA.MicroServices.Infra.Application
{
    class Program
    {
        static ILogger Logger { get; set; }

        static void Main(string[] args)
        {
            var serviceProv = SetupServiceProvider();

            Logger = serviceProv.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            var queueCons = serviceProv.GetService<IQueueConsumer>();

            serviceProv.GetService<CouchbaseConfig>().Register();

            var options = serviceProv.GetService<IOptions<CouchbaseConfigModel>>();
            IBucket Bucket = ClusterHelper.GetBucket(options.Value.Bucket);

            queueCons.ConsumeCallback = (model, ea) =>
             {
                 var body = ea.Body;
                 var messageSTR = Encoding.UTF8.GetString(body);
                 var accessModel = JsonConvert.DeserializeObject<AccessModel>(messageSTR);
                 var res = Bucket.Upsert(accessModel.Document());
                 if (res.Success)
                 {
                     var get = Bucket.GetDocument<AccessModel>(accessModel.Id);
                     var document = get.Document;
                     var msg = string.Format("{0} {1}!", document.Id, document.Content.IP);
                     Console.WriteLine(msg);
                 }
             };
            queueCons.Start();
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
            queueCons.Stop = true;
        }


        static ServiceProvider SetupServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<RabbitMQConfiguration>(Configuration.GetSection("RabbitMQ"));
            services.Configure<RabbitMQQueueConfiguration>(Configuration.GetSection("RabbitMQ.Queue"));
            services.Configure<CouchbaseConfigModel>(Configuration.GetSection("Couchbase"));

            services
              .AddLogging()
               .AddSingleton<IConnectionProvider, ConnectionProvider>()
               .AddSingleton<CouchbaseConfig>()
               .AddSingleton<IQueueConsumer, QueueConsumer>();

            return services.BuildServiceProvider();
        }

        private static IConfiguration _configuration;
        static IConfiguration Configuration
        {
            get
            {
                if (_configuration != null) return _configuration;
                Console.OutputEncoding = Encoding.UTF8;

                string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                if (String.IsNullOrWhiteSpace(environment))
                    throw new ArgumentNullException("Environment not found in ASPNETCORE_ENVIRONMENT");

                Console.WriteLine("Environment: {0}", environment);

                var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true);
                if (environment == "Development")
                {
                    builder
                        .AddJsonFile(
                            Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json"),
                            optional: true
                        );
                }
                else
                {
                    builder
                        .AddJsonFile($"appsettings.{environment}.json", optional: false);
                }

                _configuration = builder.Build();
                return _configuration;
            }
        }
    }
}
