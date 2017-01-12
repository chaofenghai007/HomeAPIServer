using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace APIControlServices
{
    public class HttpClientOperation
    {

        //public DataResult HttpGet(string url)
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["SC_Url_Host"].ToString());
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));
        //     HttpResponseMessage response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["SC_Url_Get"].ToString()).Result;
        //     DataResult result = new DataResult();
        //     if (response.IsSuccessStatusCode)
        //     {
        //         //System.Net.Http
        //         //HttpContentExtensions

        //         var msg = response.Content.ReadAsAsync<object>().Result;
        //         if (msg != null)
        //         {
        //             result.success = true;
        //             result.data = msg;
        //         }
        //     }
        //     else
        //     {
        //         result.success = false;
        //     }
        //     return result;
        //}

        public DataResult HttpGet(string url)
        {
            DataResult result = new DataResult();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Get";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        StringBuilder output = new StringBuilder();
                        output.Append(reader.ReadToEnd());
                        response.Close();
                        result.success = true;
                        result.data = output.ToString();                        
                    }
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = ex.Message;               
            }
            return result;
        }
    }

    public class DataResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}