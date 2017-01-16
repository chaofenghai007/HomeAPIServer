using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;
using System.IO;

namespace ConsoleFileReName
{
    class Program
    {
        protected static string flodPath = System.Configuration.ConfigurationManager.AppSettings["flodPath"].ToString();
        static void Main(string[] args)
        {
            ChangeFileName(new DirectoryInfo(flodPath));
        }

        /// <summary>
        /// 获取所有的文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="isOPic"></param>
        /// <param name="picType"></param>
        static void ChangeFileName(DirectoryInfo dir)//搜索文件夹中的文件
        {

            List<string> fileNamelist = new List<string>();
            FileInfo[] allFile = dir.GetFiles();
            Computer MyComputer = new Computer();
         
            foreach (FileInfo fi in allFile)
            {
                if (!fi.Name.StartsWith("sc-") && fi.Name.ToLower().Contains("tile"))
                    MyComputer.FileSystem.RenameFile(fi.FullName, "sc-" + fi.Name);
            }

            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                ChangeFileName(d);
            }   
        }
    }
}
