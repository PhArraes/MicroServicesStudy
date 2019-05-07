using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.MicroServices.Infra.Config
{
    public class CouchbaseConfigModel
    {
        public String CouchbaseServer { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Bucket { get; set; }
    }
}
