using Microsoft.Extensions.Options;
using PYPA.MicroServices.Infra.Config;
using PYPA.MicroServices.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.MicroServices.Infra
{
    public class AccessDAO : DAO<AccessModel>, IAccessDAO
    {
        public AccessDAO(IOptions<CouchbaseConfigModel> options) : base(options)
        {
        }

        public List<AccessModel> List(int take, int skip)
        {
            string query = @"SELECT _id, ip, url, params, browser, date
                             FROM monitor_test
                             ORDER BY date
                             LIMIT $1
                             OFFSET $2";

            return Execute(query, take, skip);

        }
    }
}
