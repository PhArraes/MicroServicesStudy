using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.MicroServices.RabbitMQACL.Config
{
    public class RabbitMQQueueConfiguration
    {
        public string queue { get; set; }
        public bool durable { get; set; }
        public bool exclusive { get; set; }
        public bool autoDelete { get; set; }
    }
}
