using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCTraceAPI.Controllers
{
    /// <summary>
    /// 内存API
    /// </summary>
    public class MemoryTraceController : ApiController
    {

        log4net.ILog log;
        public MemoryTraceController()
        {
            log = log4net.LogManager.GetLogger(typeof(MemoryTraceController));
        }

        /// <summary>
        /// 获取内存的使用API
        /// </summary>
        /// <param name="id">用户业务唯一id</param>
        /// <returns></returns>
        [APIOAuthorize]
        public string Get(string id)
        {
            log.Info(string.Format("{0}-{1}:request memory", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), id));            
            return GetMemoryUseing();
        }

        private string GetMemoryUseing()
        {
            ComputerInfo ci = new ComputerInfo();
            float freeRadio = 0f;
            freeRadio=ci.AvailablePhysicalMemory * 100 / ci.TotalPhysicalMemory;
            int WarMemory = int.Parse(System.Configuration.ConfigurationManager.AppSettings["WarnMenory"]);
            if (freeRadio < WarMemory)
                log.Error(string.Format("service's memory useing {0}", 100 - freeRadio));
            return freeRadio.ToString();
        }
    }
}
