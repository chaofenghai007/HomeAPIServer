using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleImageServices
{
    class Program
    {
        static void Main(string[] args)
        {
           // ServiceReference1.ImageAnalysisServiceClient client = new ServiceReference1.ImageAnalysisServiceClient();
            string file = "http://www.mystone.cn/Pictures/荒料和板材/其他/艾斯帝灰_tile_2_2b.jpg";
            //file.IndexOf("_tile_")
            string filePath = @"D:\matchImage\1.jpg";
            string md5 = GetMD5HashFromFile(@"D:\matchImage\1.jpg");
            FileInfo fi = new FileInfo(filePath);
            string range = string.Format("bytes0-{0}/{0}",fi.Length);
            string parms = string.Format("hash:{0},Content-Range:bytes0-{1}/{1}",md5,new FileInfo(filePath).Length);
            Console.WriteLine(GetMD5HashFromFile(@"D:\matchImage\1.jpg"));
            Console.WriteLine(GetMD5HashFromFile(@"D:\matchImage\2.jpg"));
            Console.WriteLine(GetMD5HashFromFile(@"D:\matchImage\1.jpg"));
          //  MarchNew();
            Console.WriteLine("123");
        }

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

        static void MarchNew()
        {
            string sourcePath = @"D:\matchImage\1.jpg";
            ServiceReference1.ImageAnalysisServiceClient clientNew = new ServiceReference1.ImageAnalysisServiceClient();
            var resultOld = clientNew.MatchImage(sourcePath, 0.5, "大理石", 0, 20);     
        }

        static void MarchOld()
        {
            string sourcePath = @"D:\matchImage\1.jpg";
            ServiceReference2.ImageAnalysisServiceClient clientOld = new ServiceReference2.ImageAnalysisServiceClient();
            var resultOld = clientOld.MatchImage(sourcePath, 0.5, "大理石", 0, 20);
        }

        static void MarchOld30()
        {
            string sourcePath = @"C:\1\1.jpg";
            ServiceReference3.ImageAnalysisServiceClient clientOld = new ServiceReference3.ImageAnalysisServiceClient();
            var resultOld = clientOld.MatchImage(sourcePath, 0.5, "大理石", 0, 20);
        }
    }
}
