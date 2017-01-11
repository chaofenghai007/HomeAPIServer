using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SCTraceAPI.Controllers
{
    /// <summary>
    /// CpuAPI
    /// </summary>
    public class CupTraceController : ApiController
    {
        log4net.ILog log;
        public CupTraceController()
        {
            log = log4net.LogManager.GetLogger(typeof(CupTraceController));
        }
        /// <summary>
        /// 获取服务器剩余的cpu
        /// </summary>
        /// <param name="id">客户端传进的唯一id</param>
        /// <returns></returns>
        [APIOAuthorize]
        public string Get(string id)
        {
            log.Info(string.Format("{0}-{1}:request cpu", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), id));            
            return GetAllCpuUse();
        }

        private string GetAllCpuUse()
        {
            float cpuUsing=0f;           
            using (
            PerformanceCounter pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                pcCpuLoad.MachineName = ".";
                pcCpuLoad.NextValue();
                System.Threading.Thread.Sleep(1000);
                cpuUsing=pcCpuLoad.NextValue();                
            }
            int WarCpu = int.Parse(System.Configuration.ConfigurationManager.AppSettings["WarnCpu"]);
            if (cpuUsing > WarCpu)
                log.Error(string.Format("service's cpu useing {0}",cpuUsing));
            return cpuUsing.ToString();
        }
    }
}
