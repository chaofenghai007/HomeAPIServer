using APIServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace APIServer.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpPost]
        public string PostData()
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
            GlobalClass.AddUserInfo(product);
            //string sql = string.Empty;
            //sql = string.Format(" INSERT INTO dbo.test   select '{0}','{1}','{2}','{3}','{4}','{5}','{6}' ", new object[]
            //    {
            //        product.First,
            //        product.Second,
            //        product.Three,
            //        product.DeviceId,
            //        product.ServerIP,
            //        product.ServerName,
            //        product.ServerPort
            //    });
            //SqlConnectTest.ExecuteNonQuery(sql);
            //通过API获取返回信息
            return GetReturnData(product.First);
          //  return product.Second;
        }

        [HttpGet]
        public string GetData(string first, string second, string three, string DeviceId)
        {
            string sql = string.Format("select id from test where first='{0}' and second='{1}' and three='{2}' and DeviceId='{3}'", new object[]
			{
				first,
				second,
				three,
				DeviceId
			});
            object obj = SqlConnectTest.ExecuteScalar(sql);
            string result = string.Empty;
            if (obj != null)
            {
                result = obj.ToString();
            }
            return result;
        }

        [HttpGet]
        public string GetStatus()
        {
            string sql = "select CodeValue from Sys_Code where CodeName='IsStart'";
            return SqlConnectTest.ExecuteScalar(sql).ToString();
        }

        public string GetQueueCount()
        {
            return GlobalClass.GetCount().ToString();
        }

        public void PostRunQueue()
        {
            while (true)
            {
                int count = GlobalClass.GetCount();
                if (count == 0)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    this.AddData(count);
                }
            }
        }

        private void AddData(int iCount)
        {
            string text = " INSERT INTO dbo.test  ";
            string text2 = "";
            List<Product> list = GlobalClass.QueueToList(iCount);
            foreach (Product current in list)
            {
                text2 += string.Format(" Union select '{0}','{1}','{2}','{3}','{4}','{5}','{6}' ", new object[]
				{
					current.First,
					current.Second,
					current.Three,
					current.DeviceId,
					current.ServerIP,
					current.ServerName,
					current.ServerPort
				});
            }
            text += text2.Substring(6);
            try
            {
                SqlConnectTest.ExecuteNonQuery(text);
                GlobalClass.ClearQuene(iCount);
            }
            catch (Exception var_4_C9)
            {
                Console.WriteLine("进行队列的异常处理");
                GlobalClass.ClearQuene(iCount);
            }
        }

        private string GetReturnData(string id)
        {
            var client = new WebClient();
            return client.DownloadString(string.Format("http://192.168.1.33:8091/api/Values/{0}",id));
        }
    }
}
