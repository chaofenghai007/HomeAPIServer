using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleThread
{
    class Program
    {
        private static ManualResetEventSlim mrs = new ManualResetEventSlim(false, 2047);
        private static int StartId = int.Parse(ConfigurationManager.AppSettings["startId"].ToString());
        private static int AddCount = int.Parse(ConfigurationManager.AppSettings["addCount"].ToString());
        private static string DeviceId = ConfigurationManager.AppSettings["DeviceId"].ToString();
        static void Main(string[] args)
        {
            int num = 0;
            while (!Program.IsStart())
            {
                Console.WriteLine("{0}:还没有开始运行，等待0.5秒再测试！--{1}", num, DateTime.Now.ToString("HH:mm:ss:fff"));
                Thread.Sleep(500);
                num++;
            }
            Console.WriteLine("开始测试！--{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
            Program.StartThread();
            Console.Read();
        }
        private static void StartThread()
        {
            int startId = Program.StartId;
            for (int i = Program.StartId; i < startId + Program.AddCount; i++)
            {
                Task.Factory.StartNew(delegate(object obj)
                {
                    string result = Program.Run(obj);
                    Console.WriteLine("执行任务成功，返回{0}", result);
                    //string strSql=""
                },String.Format("{0}_{1}_{2}", i, DeviceId ,Guid.NewGuid()));
            }
            Console.WriteLine("当前时间:{0}我是主线程{1}，你们这些任务都等 2s 执行吧：\n", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2000);
            Program.mrs.Set();
        }

        private static string Run(object obj)
        {
            Program.mrs.Wait();
            Console.WriteLine("当前时间:{0}任务 {1}已经进入。", DateTime.Now.ToString("HH:mm:ss:fff"), obj);
            return Program.HttpPost(obj.ToString(), DateTime.Now.ToString("HH:mm:ss:fff"));
        }

        private static void FetchData(int clientNumber)
        {
            var client = new WebClient();
            string data = client.DownloadString("http://localhost:18579/api/MyName");
            Console.WriteLine("Client {0} got data: {1}", clientNumber, data);
        }


        private static string HttpPost(string first, string second)
        {
            string text = DateTime.Now.ToString("HH:mm:ss:fff");
            string text2 = string.Format("first={0}&second={1}&three={2}&DeviceId={3}", new object[]
		{
			first,
			second,
			text,
			Program.DeviceId
		});
            Console.WriteLine("执行任务完成:{0}-- {1}已经进入。", text2, text);
            string requestUriString = "http://192.168.1.33:8090/api/MyName/Post";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)Encoding.UTF8.GetByteCount(text2.ToString());
            Stream requestStream = httpWebRequest.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.GetEncoding("gb2312"));
            streamWriter.Write(text2);
            streamWriter.Close();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string text3 = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            InsertResultToDB(first, text3);
            return text3;
        }

        private static bool IsStart()
        {
            string requestUriString = "http://192.168.1.33:8090/api/Values/GetStatus";
            string text = "";
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Method = "Get";
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(streamReader.ReadToEnd());
                    httpWebResponse.Close();
                    text = stringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return !string.IsNullOrEmpty(text) && !(text.Replace("\"", "") == "0");
        }

        private static string GetRemotedata(string uri)
        {
            uri = uri + "&v=" + DateTime.Now.Ticks;
            string result;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Method = "Get";
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(streamReader.ReadToEnd());
                    httpWebResponse.Close();
                    result = stringBuilder.ToString();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            result = string.Empty;
            return result;
        }

        private static void InsertResultToDB(string first, string result)
        {
            string Sql = string.Format("insert into test_result values('{0}',{1})", first, int.Parse(result));
            Console.WriteLine("更新数据SQL：{0}", Sql);
            try
            {
                SqlConnectTest.ExecuteNonQuery(Sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
