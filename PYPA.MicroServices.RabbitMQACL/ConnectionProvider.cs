using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PYPA.MicroServices.RabbitMQACL
{
    public class ConnectionProvider : IConnectionProvider
    {

        private RabbitMQConfiguration Config;
        public IConnection Connection { get; set; }

        private static object monitor = new object();

        public ConnectionProvider(IOptions<RabbitMQConfiguration> config)
        {
            this.Config = config.Value;
            if (this.Config == null || this.Config.Invalid) throw new Exception("Invalid RabbitMQ Configuration.");
            this.CreateConnection();
        }

        public IConnection GetConnection()
        {
            return Connection;
        }

        private void CreateConnection()
        {
            if (Monitor.TryEnter(monitor))
            {
                try
                {
                    if (Connection != null) return;
                    var factory = new ConnectionFactory() { HostName = Config.hostName };
                    Connection = factory.CreateConnection();
                }
                finally
                {
                    Monitor.Exit(monitor);
                }
            }
        }

    }
}
