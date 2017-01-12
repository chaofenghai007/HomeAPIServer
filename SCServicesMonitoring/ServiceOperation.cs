using System.ServiceProcess;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Garlic.Common.Security;
using System.Configuration;
using Newtonsoft.Json;


namespace SCServicesMonitoring
{
    public class ServiceOperation
    {
        log4net.ILog log;
        public ServiceOperation()
        {
            log = log4net.LogManager.GetLogger(typeof(ServiceOperation));
        }

        /// <summary>
        /// 检查并重启服务
        /// </summary>
        public void CheckServices()
        {
            log.Debug(string.Format("{0},run one times",DateTime.Now));
            //if (!MonitoringSEServerStatues1())
            //{
            //    RestartSEServer();                
            //}
            int iStatus=MonitoringSEServerStatues2();
            if ( iStatus== 1)
            {
                RestartSEServer();
            }
            else if( iStatus==2)
            {
 
            }
        }


        /// <summary>
        /// 检测搜索引擎服务是否正常运行,修改返回值为int类型
        /// </summary>
        /// <returns></returns>
        private int MonitoringSEServerStatues2()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["SC_ReAPI_Host"].ToString());
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            int isOK = 0;
            //获得登录用户数据
            try
            {
                HttpResponseMessage response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["SC_ReAPI_Request"].ToString() + System.Configuration.ConfigurationManager.AppSettings["SC_Url_Get"].ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    //System.Net.Http
                    //HttpContentExtensions

                    var msg = JsonConvert.DeserializeObject<DataResult>(response.Content.ReadAsAsync<object>().Result.ToString());
                    if (msg != null && msg.success == true)
                    {

                        string tmsg = msg.data.ToString().ToLower();
                        log.Debug(tmsg);
                        if (tmsg.IndexOf("green") > 0 || tmsg.ToString().IndexOf("yellow") > 0)
                        {
                            isOK = 0;
                        }
                        else if (tmsg.IndexOf("red") > 0)
                        {
                            log.Error(string.Format("搜索引擎服务在运行，但是状态为红色"));
                            if (CheckProductNO())
                                isOK = 0;
                            else
                                isOK = 1;
                        }
                    }
                    else
                    {
                        log.Error(string.Format("不明错误（状态不是绿色、黄色和红色）"));
                        isOK = 2;
                    }

                }
                else
                {
                    log.Error(string.Format("搜索引擎停止服务"));
                    isOK = 2;
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("搜索引擎停止服务,{0}", ex.Message));
                isOK = 2;
            }
            return isOK;
        }

        public bool CheckTest()
        {
            string key = "stonecontact";
            string iv = "NI!fb@95GUY86GfghUb#er57HB";
            IEncryptionProvider encryptionProvider = new Garlic.Common.Security.DESEncryptionProvider(key, iv);

            HttpClient client = new HttpClient();
            //重启搜索引擎需要进行用户确认,加在head头里
            string urlHead = "/(S(" + encryptionProvider.Encrypt(ConfigurationManager.AppSettings["SC_ReAPI_User"].ToString(), true) + "))/";
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["SC_ReAPI_Host"].ToString() + urlHead);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            PostData postData = new PostData() { PostType = DataType.my_string, Data = ConfigurationManager.AppSettings["SC_ServerName"].ToString() };
            return false;
        }

        /// <summary>
        /// 检测搜索引擎服务是否正常运行
        /// </summary>
        /// <returns></returns>
        private bool MonitoringSEServerStatues1()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["SC_ReAPI_Host"].ToString());
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            bool isOK = false;
            //获得登录用户数据
            try
            {
                HttpResponseMessage response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["SC_ReAPI_Request"].ToString() + System.Configuration.ConfigurationManager.AppSettings["SC_Url_Get"].ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    //System.Net.Http
                    //HttpContentExtensions

                    var msg = JsonConvert.DeserializeObject < DataResult >(response.Content.ReadAsAsync<object>().Result.ToString());
                    if (msg != null && msg.success==true)
                    {
                        
                        string tmsg = msg.data.ToString().ToLower();
                        log.Debug(tmsg);
                        if (tmsg.IndexOf("green") > 0 || tmsg.ToString().IndexOf("yellow") > 0)
                        {
                            isOK = true;
                        }
                        else if (tmsg.IndexOf("red") > 0)
                        {
                            log.Error(string.Format("搜索引擎服务在运行，但是状态为红色"));
                            isOK = CheckProductNO();
                        }
                    }
                    else
                    {
                        log.Error(string.Format("不明错误（状态不是绿色、黄色和红色）"));
                        isOK = false;
                    }

                }
                else
                {
                    log.Error(string.Format("搜索引擎停止服务"));
                    isOK = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("搜索引擎停止服务,{0}", ex.Message));
                isOK = false;
            }
            return isOK;
        }


        /// <summary>
        /// 检测搜索引擎服务是否正常运行
        /// </summary>
        /// <returns></returns>
        private bool MonitoringSEServerStatues()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["SC_Url_Host"].ToString());
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            bool isOK = false;
            //获得登录用户数据
            try
            {
                HttpResponseMessage response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["SC_Url_Get"].ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    //System.Net.Http
                    //HttpContentExtensions
                    
                    var msg = response.Content.ReadAsAsync<object>().Result;
                    if (msg != null)
                    {
                        string tmsg=msg.ToString().ToLower();
                        log.Debug(tmsg);
                        if (tmsg.IndexOf("green") > 0 || tmsg.ToString().IndexOf("yellow") > 0)
                        {
                            isOK = true;
                        }
                        else if (tmsg.IndexOf("red") > 0)
                        {
                            log.Error(string.Format("搜索引擎服务在运行，但是状态为红色"));
                            isOK = CheckProductNO();
                        }
                    }                   
                    else
                    {
                        log.Error(string.Format("不明错误（状态不是绿色、黄色和红色）"));
                        isOK = false;
                    }

                }
                else
                {
                    log.Error(string.Format("搜索引擎停止服务"));
                    isOK = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("搜索引擎停止服务,{0}",ex.Message));
                isOK= false;
            }
            return isOK;
        }

        /// <summary>
        /// 重新启动搜索引擎服务
        /// </summary>
        /// <returns></returns>
        private bool RestartSEServer()
        {
            string key="stonecontact";
            string iv="NI!fb@95GUY86GfghUb#er57HB";
            IEncryptionProvider encryptionProvider = new Garlic.Common.Security.DESEncryptionProvider(key, iv);

            HttpClient client = new HttpClient();
            //重启搜索引擎需要进行用户确认,加在head头里
            string urlHead = "/(S(" + encryptionProvider.Encrypt(ConfigurationManager.AppSettings["SC_ReAPI_User"].ToString(), true) + "))/";
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["SC_ReAPI_Host"].ToString() + urlHead);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            PostData postData = new PostData() { PostType = DataType.my_string, Data = ConfigurationManager.AppSettings["SC_ServerName"].ToString() };
            HttpResponseMessage response = client.PostAsJsonAsync(ConfigurationManager.AppSettings["SC_ReAPI_Post"].ToString(), postData).Result;
            
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<DataResult>().Result;
                if (result.success)
                {
                    log.Warn(string.Format("搜索引擎重启成功,{0}", result.message));
                    return true;
                }
                else
                {
                    log.Error(string.Format("搜索引擎重启失败,{0}", result.message));
                    return true;
                }
            }
            else
            {
                log.Error(string.Format("搜索引擎重启失败,{0}", "搜索引擎服务器的重启API有问题，检查重启API是否启动"));
                return false;
            }
        }

        private bool CheckProductNO()
        {
            string result = "";
            bool isOK = false;
            int iProductNO = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SC_Product_NO"].ToString());
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["SC_Url_Check"].ToString());
                request.Method = "Get";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    StringBuilder output = new StringBuilder();
                    output.Append(reader.ReadToEnd());
                    response.Close();
                    result = output.ToString();
                }
                if (string.IsNullOrEmpty(result))
                {
                    isOK = false;
                }
                else
                {
                    if (int.Parse(result.Substring(14, result.IndexOf("PageIndex") - 16)) > iProductNO)
                    {
                        log.Error(string.Format("搜索引擎为红色，但是数量正确,等待下次查询，查询数量是{0}", result.Substring(14, result.IndexOf("PageIndex") - 16)));
                        isOK = true;
                    }
                    else
                    {
                        log.Error(string.Format("搜索引擎为红色，但是数量比较少，查询数量是{0}，需求重启服务", result.Substring(14, result.IndexOf("PageIndex") - 16)));
                        isOK = false;
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("查询产品数量错误,{0}", ex.Message));
                isOK = false;
                return isOK;
            }            
                return isOK; 
            
            
        }


        /// <summary>
        /// 关闭服务然后启动搜索引擎服务
        /// </summary>
        /// <returns></returns>
        private bool KillAndRestartSEServer()
        {
            string key = "stonecontact";
            string iv = "NI!fb@95GUY86GfghUb#er57HB";
            IEncryptionProvider encryptionProvider = new Garlic.Common.Security.DESEncryptionProvider(key, iv);

            HttpClient client = new HttpClient();
            //重启搜索引擎需要进行用户确认,加在head头里
            string urlHead = "/(S(" + encryptionProvider.Encrypt(ConfigurationManager.AppSettings["SC_ReAPI_User"].ToString(), true) + "))/";
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["SC_ReAPI_Host"].ToString() + urlHead);
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            PostData postData = new PostData() { PostType = DataType.my_string, Data = ConfigurationManager.AppSettings["SC_ServerName"].ToString() };
            HttpResponseMessage response = client.PostAsJsonAsync(ConfigurationManager.AppSettings["SC_KillAPI_Post"].ToString(), postData).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsAsync<DataResult>().Result;
                if (result.success)
                {
                    log.Warn(string.Format("搜索引擎重启成功,{0}", result.message));
                    return true;
                }
                else
                {
                    log.Error(string.Format("搜索引擎重启失败,{0}", result.message));
                    return true;
                }
            }
            else
            {
                log.Error(string.Format("搜索引擎重启失败,{0}", "搜索引擎服务器的重启API有问题，检查重启API是否启动"));
                return false;
            }
        }
    }
}
