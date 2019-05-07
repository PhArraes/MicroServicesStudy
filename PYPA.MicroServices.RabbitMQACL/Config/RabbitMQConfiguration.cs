using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.MicroServices.RabbitMQACL
{
    public class RabbitMQConfiguration
    {
        public string hostName { get; set; }
        public string user { get; set; }
        public string password { get; set; }

        public bool Invalid
        {
            get
            {
                return string.IsNullOrEmpty(hostName) ||
                        string.IsNullOrEmpty(user) ||
                        string.IsNullOrEmpty(password);
            }
        }
    }
}
