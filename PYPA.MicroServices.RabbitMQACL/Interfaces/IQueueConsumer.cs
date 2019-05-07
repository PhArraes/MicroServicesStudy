using System;
using RabbitMQ.Client.Events;

namespace PYPA.MicroServices.RabbitMQACL
{
    public interface IQueueConsumer
    {
        EventHandler<BasicDeliverEventArgs> ConsumeCallback { get; set; }
        bool Stop { get; set; }

        void Start();
    }
}