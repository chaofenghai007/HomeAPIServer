using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace LocalAPI.Controllers
{
    public class ValuesController : ApiController
    {       
        // 根据传入参数获取数据库的返回值
        public async Task<string> Get(string id)
        {
           // var test = await GetReturnValue(id);
            return await GetReturnValue(id);
        }

        private Task<string> GetReturnValue(string id)
        {
            return Task.Factory.StartNew(() =>
            {
                string sql = string.Empty;
                sql = string.Format(" select id from test where first='{0}'; ", id);
                return GetData(sql);
            },
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        private string GetData(string sql)
        {
            string result = string.Empty;
            while (string.IsNullOrEmpty(result))
            {
                result = SqlConnectTest.ExecuteScalar(sql).ToString();
            }
            return result;
        }


    }
}