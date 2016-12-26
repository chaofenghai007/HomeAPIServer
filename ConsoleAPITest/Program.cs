using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPITest
{
    class Program
    {


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
    }
}
