using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPITest
{
    class Program
    {


        #region 后台post文件测试
        static void Main(string[] args)
        {
           // Console.WriteLine(HttpPost());
           // NameValueCollection nvc = new NameValueCollection();
          //  nvc.Add("Name", "MyLake");
          //  Console.WriteLine(HttpUploadFile("http://localhost:2794/Home/PostTest", @"D:\matchImage\1.jpg", "file", "image/jpeg", nvc));
            //string result=string.Empty;
            //sendFileData(out result);
            //Console.WriteLine(result);
            HttpRequestClient c1 = new HttpRequestClient();
            string filePath = @"D:\matchImage\1.jpg";
            string md5 = GetMD5HashFromFile(filePath);
            string parms = string.Format("hash={0}&Content-Range=bytes0-{1}/{1}", md5, new FileInfo(filePath).Length);
            string returnFilePath = c1.HttpPostFile("http://stoneapi.bstone.com/api/picture/postuploadpic", parms, new List<string>() { filePath });
            Console.WriteLine(returnFilePath);
            var retult= JsonConvert.DeserializeObject<DataResult>(returnFilePath);
            Console.WriteLine(retult.data.Url);
            parms = string.Format("source={0}&ratio=0.5&category=大理石&pageIndex=0&pageSize=20", retult.data.Url);
            var obj= c1.HttpPostData("http://stoneapi.bstone.com/api/matchimg/postimgmatch", parms);
            var matchResult = JsonConvert.DeserializeObject<MatchResult>(obj);
            foreach (var temp in matchResult.data)
            {
                Console.WriteLine(string.Format("相似度：{0},图片路径{1}", temp.Similarity, temp.FilePath));
            }
           // Console.WriteLine(obj);
           // Console.WriteLine(c1.HttpPostData("http://localhost:2794/Home/PostTest", "Name=Lake1"));
          //  Console.WriteLine(c1.HttpPostFile("http://localhost:2794/Home/PostTest", null, new List<string>() { @"D:\matchImage\1.jpg", @"D:\matchImage\2.jpg" }));
            Console.ReadKey(true);
        }
        #endregion


        static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /*异步测试
        static void Main(string[] args)
        {
            var t1 = Task.Run(() => FetchData(1));
            var t2 = Task.Run(() => FetchData(2));
            var t3 = Task.Run(() => FetchData(3));

            var index = Task.WaitAny(t1, t2, t3);
            Console.WriteLine("Task {0} finished first", index + 1);

            Task.WaitAll(t1, t2, t3);
            Console.WriteLine("All tasks have finished");

            Console.WriteLine("Press any key");
            Console.ReadKey(true);
        }
        */
        static void FetchData(int clientNumber)
        {
            var client = new WebClient();
            string data = client.DownloadString("http://localhost:18579/api/MyName");
            Console.WriteLine("Client {0} got data: {1}", clientNumber, data);
        }

        /*
        static void Main(string[] args)
        {

            string url = "http://localhost:18579/api/values";
           // string postDataStr = "status=0&pi=0&ps=10";
            //初始化request参数
            string postData = string.Format(@"{{ ID:'{0}',NAME:'{1}',CREATETIME:'{2}' }}", 1, "Jim", "1988-09-11");// "{ ID: \"1\", NAME: \"Jim\", CREATETIME: \"1988-09-11\" }";
          //  string postData = "{ ID: '1', NAME: 'Jim', CREATETIME: '1988-09-11' }";
            //  var tp1= GetRemotedata(url + "?" + postDataStr);
            HttpPostPut(url, postData, "POST");
            url = "http://localhost:18579/api/values/2";
            HttpPostPut(url, postData, "PUT");

            Console.Read();
        }
        */
        static void HttpPostPut(string url, string postData, string HttpType)
        {          

            //定义request并设置request的路径
            WebRequest request = WebRequest.Create(url);
            request.Method = HttpType;

            //初始化request参数
            //string postData = "{ ID: \"1\", NAME: \"Jim\", CREATETIME: \"1988-09-11\" }";

            //设置参数的编码格式，解决中文乱码
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //设置request的MIME类型及内容长度
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            //打开request字符流
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //定义response为前面的request响应
            WebResponse response = request.GetResponse();

            //获取相应的状态代码
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            //定义response字符流
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();//读取所有
            Console.WriteLine(responseFromServer);
        }

        private static  string HttpPost()
        {
            string text2 = string.Format("Name={0}", "lake");
            string requestUriString = "http://localhost:2794/Home/PostTest";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)Encoding.UTF8.GetByteCount(text2.ToString());
            Stream requestStream = httpWebRequest.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.GetEncoding("gb2312"));
            streamWriter.Write(text2);
            streamWriter.Close();

            using (WebResponse wr = httpWebRequest.GetResponse())
            {
                //在这里进行图片上传
            }

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string text3 = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            return text3;
        }

        public static string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string result = string.Empty;
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);

                result = reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }

            return result;
        }

    }

    public class DataResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public ImageResult data { get; set; }
    }

    public class ImageResult
    {
        public string Url { get; set; }
        public int NextPosition { get; set; }
    }

    public class MatchResult
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public List<MatchStoneBase> data { get; set; }
    }

    public class MatchStoneBase
    {
        /// <summary>
        /// 石种ID
        /// </summary>
        public int StoneBaseID { get; set; }
        /// <summary>
        /// 国家ID
        /// </summary>
        public int CountryID { get; set; }
        /// <summary>
        /// 国家名称英文
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 国家名称中文
        /// </summary>
        public string CountryName_CN { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 石种名称
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 石种中文名称
        /// </summary>
        public string MaterialName_CN { get; set; }
        /// <summary>
        /// 材质名称
        /// </summary>
        public string CatalogName { get; set; }
        /// <summary>
        /// 材质名称(中文)
        /// </summary>
        public string CatalogName_CN { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string ColorName { get; set; }
        /// <summary>
        /// 颜色中文
        /// </summary>
        public string ColorName_CN { get; set; }
        /// <summary>
        /// 相识度
        /// </summary>
        public double Similarity { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string FilePath { get; set; }
    }
}
