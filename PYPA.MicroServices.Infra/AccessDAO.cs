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

        public List<AccessModel> List(string ip, string path, int take, int skip)
        {
            List<KeyValuePair<String, object>> @params = new List<KeyValuePair<string, object>>();
            @params.Add(new KeyValuePair<string, object>("take", take));
            @params.Add(new KeyValuePair<string, object>("skip", skip));
            var where = "where true ";

            if (!string.IsNullOrEmpty(ip))
            {
                where += " AND ip like $ip ";
                @params.Add(new KeyValuePair<string, object>("ip", ip + '%'));
            }
            if (!string.IsNullOrEmpty(path))
            {
                where += "AND url like $path ";
                @params.Add(new KeyValuePair<string, object>("path", $"{path}%"));
            }

            string query = $@"SELECT _id, ip, url, params, browser, date
                             FROM monitor_test
                             {where}
                             ORDER BY date DESC
                             LIMIT $take
                             OFFSET $skip";

            return ExecuteNamed(query, @params);
        }
    }
}
