using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using APIServer.Models;

namespace APIServer.Controllers
{
    public class MyNameController : ApiController
    {
        private static bool _firstTime = true;
        // GET api/myname
        //public async Task<string> Get()
        //{
        //    Debug.WriteLine("Entry thread id: {0}. Sync: {1}",
        //        Thread.CurrentThread.ManagedThreadId,
        //        SynchronizationContext.Current);

        //   var test= await LongWaitAsync();
        //   return test;
        //}

        private Task<string> LongWaitAsync(Product product)
        {
            return Task.Factory.StartNew(() =>
            {
                string sql = string.Empty;
                sql = string.Format(" INSERT INTO dbo.test   select '{0}','{1}','{2}','{3}','{4}','{5}','{6}';SELECT @@Identity; ", new object[]
				{
					product.First,
					product.Second,
					product.Three,
					product.DeviceId,
					product.ServerIP,
					product.ServerName,
					product.ServerPort
				});
                return SqlConnectTest.ExecuteScalar(sql).ToString();
               // SqlConnectTest.ExecuteNonQuery(sql);
                //return string.Format("Success. thread id: {0}. Sync: {1},Three:{2}",
                //        Thread.CurrentThread.ManagedThreadId,
                //        SynchronizationContext.Current,product.Three);
            },
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        // GET api/myname/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/myname
        public async Task<string> Post()
        {
            var product = new Product
            {
                First = HttpContext.Current.Request["first"],
                Second = HttpContext.Current.Request["second"],
                Three = DateTime.Now.ToString("HH:mm:ss:fff"),
                DeviceId = HttpContext.Current.Request["DeviceId"],
                ServerName = HttpContext.Current.Server.MachineName,
                ServerIP = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"],
                ServerPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"]
            };
            var test = await LongWaitAsync(product);
            return test;
        }

        // PUT api/myname/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/myname/5
        public void Delete(int id)
        {
        }
    }
}
