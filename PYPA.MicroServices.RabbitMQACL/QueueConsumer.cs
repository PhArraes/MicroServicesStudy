using Microsoft.Extensions.Options;
using PYPA.MicroServices.RabbitMQACL.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PYPA.MicroServices.RabbitMQACL
{
    public class QueueConsumer : IQueueConsumer
    {
        IConnection Connection;
        private RabbitMQQueueConfiguration QueueConfig { get; }
        private Task ConsumingTask { get; set; }
        public EventHandler<BasicDeliverEventArgs> ConsumeCallback { get; set; }
        public bool Stop { get; set; }
        public QueueConsumer(IConnectionProvider connectionProvider, IOptions<RabbitMQQueueConfiguration> options)
        {
            Connection = connectionProvider.GetConnection();
            this.QueueConfig = options.Value;
            this.Stop = true;
        }

        public void Start()
        {
            if (!Stop) return;
            if (ConsumeCallback == null) throw new Exception("Define a consume call back for rabbit events.");
            if (ConsumingTask == null) ConsumingTask = CreateTask();
            Stop = false;
            ConsumingTask.Start();
        }

        private Task CreateTask()
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
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += ConsumeCallback;
                    channel.BasicConsume(queue: QueueConfig.queue, autoAck: true, consumer: consumer);
                    while (!Stop) Thread.Sleep(100);
                }
            });
        }
    }
}
