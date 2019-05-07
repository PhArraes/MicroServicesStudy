using Microsoft.Extensions.Options;
using PYPA.MicroServices.RabbitMQACL.Config;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PYPA.MicroServices.RabbitMQACL
{
    class PublisherTaskFactory
    {
        private IConnection Connection { get; set; }
        private RabbitMQQueueConfiguration QueueConfig { get; }
        public bool Running { get; set; }
        public PublisherTaskFactory(IConnectionProvider connectionProvider, IOptions<RabbitMQQueueConfiguration> options)
        {
            this.QueueConfig = options.Value;
            this.Connection = connectionProvider.GetConnection();
            Running = true;
        }

        public Task CreateTask(string queue, ConcurrentQueue<string> messageQueue)
        {
            return new Task(() =>
            {
                using (var channel = Connection.CreateModel())
                {
                    channel.QueueDeclare(queue: QueueConfig.queue,
                                         durable: QueueConfig.durable,
                                         exclusive: QueueConfig.exclusive,
                                         autoDelete: QueueConfig.autoDelete,
                                         arguments: null);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    string message = null;
                    while (Running)
                    {
                        if(messageQueue.TryDequeue(out message))
                        {
                            var body = Encoding.UTF8.GetBytes(message);
                            channel.BasicPublish(exchange: "",
                                                 routingKey: queue,
                                                 basicProperties: properties,
                                                 body: body);
                            Console.WriteLine(" [x] Sent {0}", message);
                        }
                        Thread.Sleep(200);
                    }
                }
            });
        }
    }
}
