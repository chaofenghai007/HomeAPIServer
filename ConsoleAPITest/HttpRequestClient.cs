using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAPITest
{
    public class HttpRequestClient
    {

        /// <summary>
        /// get无参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parmString"></param>
        /// <returns></returns>
        public string HttpGet(string url)
        {           
            WebClient webClient = new WebClient();           
            byte[] responseData =webClient.UploadData(url, "GET",null);//得到返回字符流    
            return Encoding.UTF8.GetString(responseData);//解码   
        }        

        /// <summary>
        /// get取值带参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parmString"></param>
        /// <returns></returns>
        public string HttpGet(string url, string parmString)
        {
            byte[] postData = Encoding.UTF8.GetBytes(parmString);//编码，尤其是汉字，事先要看下抓取网页的编码方式    
            WebClient webClient = new WebClient();            
            byte[] responseData = webClient.UploadData(url, "GET", postData);//得到返回字符流    
            return Encoding.UTF8.GetString(responseData);//解码   
        }

        /// <summary>  
        /// post发送数据(没有文件)  
        /// </summary>  
        ///  <param name="url">接收地址</param>  
        /// <param name="postString">拼接Post字符串"arg1=a&arg2=b"</param>  
        /// <returns></returns>  
        public string HttpPostData(string url, string postString)
        {
            byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式    
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可    
            byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流    
            return Encoding.UTF8.GetString(responseData);//解码   
        }

        /// <summary>  
        /// post发送数据(带文件)  
        /// </summary>  
        ///  <param name="url">接收地址</param>  
        /// <param name="postString">拼接Post字符串"arg1=a&arg2=b"</param>  
        /// <returns></returns>  
        public string HttpPostFile(string url, string postString,List<string> filePaths)
        {
            Init();
            string responseText = string.Empty;
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);
            SetFieldValue(postString);
            SetFileValue(filePaths);
            byte[] responseBytes;
            byte[] bytes = MergeContent();

            try
            {
                responseBytes = webClient.UploadData(url, bytes);               
            }
            catch (WebException ex)
            {
                Stream responseStream = ex.Response.GetResponseStream();
                responseBytes = new byte[ex.Response.ContentLength];
                responseStream.Read(responseBytes, 0, responseBytes.Length);               
            }
            responseText = System.Text.Encoding.UTF8.GetString(responseBytes);
            return responseText;
        }

        #region  带文件post处理

        #region //字段
        private ArrayList bytesArray;
        private Encoding encoding = Encoding.UTF8;
        private string boundary = String.Empty;
        #endregion

        private void Init()
        {
            bytesArray = new ArrayList();
            string flag = DateTime.Now.Ticks.ToString("x");
            boundary = "---------------------------" + flag;
        }

        /// <summary>  
        /// 合并请求数据  
        /// </summary>  
        /// <returns></returns>  
        private byte[] MergeContent()
        {
            int length = 0;
            int readLength = 0;
            string endBoundary = "--" + boundary + "--\r\n";
            byte[] endBoundaryBytes = encoding.GetBytes(endBoundary);

            bytesArray.Add(endBoundaryBytes);

            foreach (byte[] b in bytesArray)
            {
                length += b.Length;
            }

            byte[] bytes = new byte[length];

            foreach (byte[] b in bytesArray)
            {
                b.CopyTo(bytes, readLength);
                readLength += b.Length;
            }

            return bytes;
        }

        /// <summary>
        /// 设置表单数据字段
        /// </summary>
        /// <param name="postString">拼接Post字符串"arg1=a&arg2=b"</param>
        private void SetFieldValue(string postString)
        {
            if (string.IsNullOrWhiteSpace(postString))
                return;
            postString.Split('&').ToList().ForEach(
                o =>
                {
                    string httpRow = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
                    string httpRowData = String.Format(httpRow, o.Split('=')[0], o.Split('=').Count() > 1 ? o.Split('=')[1] : "");
                    bytesArray.Add(encoding.GetBytes(httpRowData));
                }
                );
        }

        private void SetFileValue(List<string> files)
        {
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    SetFileAddBuff(file, "application/octet-stream");                    
                }
            }
        }

        private void SetFileAddBuff(string filePath, String contentType)
        {
            string fileName = Path.GetFileName(filePath);
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            byte[] fileBytes = new byte[fs.Length];
            fs.Read(fileBytes, 0, fileBytes.Length);
            fs.Close();
            fs.Dispose();

            string end = "\r\n";
            string httpRow = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string httpRowData = String.Format(httpRow, "key", fileName, contentType);

            byte[] headerBytes = encoding.GetBytes(httpRowData);
            byte[] endBytes = encoding.GetBytes(end);
            byte[] fileDataBytes = new byte[headerBytes.Length + fileBytes.Length + endBytes.Length];

            headerBytes.CopyTo(fileDataBytes, 0);
            fileBytes.CopyTo(fileDataBytes, headerBytes.Length);
            endBytes.CopyTo(fileDataBytes, headerBytes.Length + fileBytes.Length);
            bytesArray.Add(fileDataBytes);

        }

        /// <summary>  
        /// 设置表单文件数据  
        /// </summary>  
        /// <param name="fieldName">字段名</param>  
        /// <param name="filename">字段值</param>  
        /// <param name="contentType">内容内型</param>  
        /// <param name="fileBytes">文件字节流</param>  
        /// <returns></returns>  
        private void SetFieldValue(String fieldName, String filename, String contentType, Byte[] fileBytes)
        {
            string end = "\r\n";
            string httpRow = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string httpRowData = String.Format(httpRow, fieldName, filename, contentType);

            byte[] headerBytes = encoding.GetBytes(httpRowData);
            byte[] endBytes = encoding.GetBytes(end);
            byte[] fileDataBytes = new byte[headerBytes.Length + fileBytes.Length + endBytes.Length];

            headerBytes.CopyTo(fileDataBytes, 0);
            fileBytes.CopyTo(fileDataBytes, headerBytes.Length);
            endBytes.CopyTo(fileDataBytes, headerBytes.Length + fileBytes.Length);

            bytesArray.Add(fileDataBytes);
        }
        #endregion
    }
}
