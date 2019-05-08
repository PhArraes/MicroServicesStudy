using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PYPA.MicroServices.Core;
using PYPA.MicroServices.Infra;

namespace PYPA.MicroServices.Report.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccessesController : Controller
    {
        public IAccessDAO accessDAO { get; }

        public AccessesController(IAccessDAO accessDAO)
        {
            this.accessDAO = accessDAO;
        }

        [HttpGet]
        public IEnumerable<AccessModel> List(string ip, string path, int take = 10, int skip = 0)
        {
            return accessDAO.List(ip, path,  take, skip);
        }

        
    }
}
