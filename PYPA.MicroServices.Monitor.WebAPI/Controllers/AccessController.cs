using System;
using System.Collections.Generic;
using Couchbase;
using Couchbase.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PYPA.MicroServices.Infra;
using PYPA.MicroServices.Infra.Config;
using PYPA.MicroServices.Core;
using PYPA.MicroServices.RabbitMQACL;

namespace PYPA.MicroServices.Monitor.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        public IPublisher Publisher { get; set; }

        public AccessController(IPublisher publisher)
        {
            Publisher = publisher;            
        }

        [HttpGet()]
        public IActionResult Get()
        {
            //var aa = new Class1();
            //aa.a();
            return Ok("FOI");
        }

        [HttpPost]
        public IActionResult Post([FromBody] AccessModel model)
        {
            if (model == null || !model.IsValid()) return Ok();
            Publisher.Publish(model.ToJson());
                        
            return Ok();
        }

    }
}
