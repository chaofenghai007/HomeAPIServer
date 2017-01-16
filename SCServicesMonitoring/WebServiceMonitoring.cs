using Garlic.Common.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SCServicesMonitoring
{
    /// <summary>
    /// 99服务器监控
    /// </summary>
    public class WebServiceMonitoring
    {
        log4net.ILog log;
        string apiHost;
        public WebServiceMonitoring()
        {
            log = log4net.LogManager.GetLogger(typeof(WebServiceMonitoring));
            apiHost = GetApiUrl();
        }

        /// <summary>
        /// 检测99的cpu和内存
        /// </summary>
        public void CheckMainWebServices()
        {
            CheckCpu();
            CheckMemory();
        }

        private string GetApiUrl()
        {
            string key = "stonecontact";
            string iv = "NI!fb@95GUY86GfghUb#er57HB";
            IEncryptionProvider encryptionProvider = new Garlic.Common.Security.DESEncryptionProvider(key, iv);
            return ConfigurationManager.AppSettings["SC_Server_Host"].ToString() 
                + "(S(" + encryptionProvider.Encrypt(ConfigurationManager.AppSettings["SC_ReAPI_User"].ToString(), true) + "))/";
        }

        private string GetUUID()
        {
            return ConfigurationManager.AppSettings["ClientId"].ToString() +"_"+ DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff");
        }

        /// <summary>
        /// 检查cpu
        /// </summary>
        private void CheckCpu()
        {
            string uuid = GetUUID();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiHost);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            float webCpuUseing = 0;
            //获得登录用户数据
             try
             {
                 HttpResponseMessage response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["SC_CPU_Get"].ToString() + uuid).Result;
                 if (response.IsSuccessStatusCode)
                 {
                    // webCpuUseing = int.Parse(response.Content.ReadAsAsync<object>().Result.ToString());
                     string result = response.Content.ReadAsAsync<object>().Result.ToString().Trim('"');
                     float.TryParse(result,out webCpuUseing);                     
                     if (webCpuUseing > float.Parse(ConfigurationManager.AppSettings["WarnCpu"].ToString()))
                     {
                         log.Error(string.Format("{0}-主站的cpu使用率达到了{1}", uuid, webCpuUseing));
                     }
                     else
                         log.Info(string.Format("{0}-检查了一次cpu,主站的cpu使用率达为{1}", uuid, webCpuUseing));
                 }
             }
             catch (Exception ex)
             {
                 log.Error(string.Format("{0}-查询站点服务器出错", uuid), ex);
             }

            //try
            //{
            //    var client = new WebClient();               
            //    string Url = apiHost + System.Configuration.ConfigurationManager.AppSettings["SC_CPU_Get"].ToString() + uuid;
            //    log.Info(string.Format("请求的url_{0}", Url));
            //    int webCpuUseing =int.Parse(client.DownloadString(Url));
            //    if (webCpuUseing > int.Parse(ConfigurationManager.AppSettings["WarnCpu"].ToString()))
            //    {
            //        log.Error(string.Format("{0}-主站的cpu使用率达到了{1}",uuid, webCpuUseing));
            //    }
            //    else
            //        log.Info(string.Format("{0}-检查了一次cpu",uuid));
            //}
            //catch (Exception ex)
            //{
            //    log.Error(string.Format("{0}-查询站点服务器出错",uuid),ex);
            //}
        }

        /// <summary>
        /// 检查内存
        /// </summary>
        private void CheckMemory()
        {

            string uuid = GetUUID();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiHost);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            int webCpuUseing = 0;
            //获得登录用户数据
            try
            {
                HttpResponseMessage response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["SC_Memory_Get"].ToString() + uuid).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsAsync<object>().Result.ToString().Trim('"');
                    webCpuUseing = int.Parse(result);
                    if (webCpuUseing > int.Parse(ConfigurationManager.AppSettings["WarnCpu"].ToString()))
                    {
                        log.Error(string.Format("{0}-主站的内存不足，只剩下了{1}%", uuid, webCpuUseing));
                    }
                    else
                        log.Info(string.Format("{0}-检查了一次内存,剩余内存为{1}%", uuid, webCpuUseing));
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}-查询站点服务器内存出错", uuid), ex);
            }

            //string uuid = GetUUID();
            //try
            //{
            //    var client = new WebClient();
            //    string Url = apiHost + System.Configuration.ConfigurationManager.AppSettings["SC_Memory_Get"].ToString() + uuid;
            //    log.Info(string.Format("请求的url_{0}", Url));
            //    int webCpuUseing = int.Parse(client.DownloadString(Url));
            //    if (webCpuUseing < int.Parse(ConfigurationManager.AppSettings["WarnMenory"].ToString()))
            //    {
            //        log.Error(string.Format("{0}-主站的内存不足，只剩下了{1}%", uuid,webCpuUseing));
            //    }
            //    else
            //        log.Info(string.Format("{0}-检查了一次内存",uuid));
            //}
            //catch (Exception ex)
            //{
            //    log.Error(string.Format("{0}-查询站点服务器内存出错",uuid), ex);
            //}
        }
    }
}
