using Microsoft.Extensions.Options;
using PYPA.MicroServices.RabbitMQACL.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PYPA.MicroServices.RabbitMQACL
{
    public class Publisher : IPublisher
    {
        private ConcurrentQueue<String> MessageQueue { get; }
        private PublisherTaskFactory TaskFactory { get; set; }
        private List<Task> Tasks { get; set; }
        public Publisher(IConnectionProvider connectionProvider, IOptions<RabbitMQQueueConfiguration> options)
        {
            MessageQueue = new ConcurrentQueue<string>();
            Tasks = new List<Task>();
            TaskFactory = new PublisherTaskFactory(connectionProvider, options);
            AddTask();
        }

        public void Publish(String message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageQueue.Enqueue(message);
            }
        }

        public void AddTask()
        {
            var task = TaskFactory.CreateTask("monitor", MessageQueue);
            task.Start();
            Tasks.Add(task);
        }
    }
}
