using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileControl
{
    class Program
    {
        protected static string flodPath = System.Configuration.ConfigurationManager.AppSettings["flodPath"].ToString();
        protected static string directFlod = System.Configuration.ConfigurationManager.AppSettings["directFlod"].ToString();
        static void Main(string[] args)
        {           
            FileMove(new DirectoryInfo(flodPath));
        }

        /// <summary>
        /// 获取所有的文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="isOPic"></param>
        /// <param name="picType"></param>
        static List<string> GetAllList(DirectoryInfo dir)//搜索文件夹中的文件
        {

            List<string> fileNamelist = new List<string>();
            FileInfo[] allFile = dir.GetFiles();

            foreach (FileInfo fi in allFile)
            {
                fileNamelist.Add(fi.FullName);
            }
            return fileNamelist;
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="isOPic"></param>
        /// <param name="picType"></param>
        static void FileMove(DirectoryInfo dir)//搜索文件夹中的文件
        {
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                if (fi.Name.ToLower().Contains("tile"))
                {
                    File.Copy(fi.FullName, fi.FullName.Replace(flodPath, directFlod), true);
                    Console.WriteLine("{0}-File Copy-{1}", flodPath, directFlod);
                }
            }
        }
    }
}
