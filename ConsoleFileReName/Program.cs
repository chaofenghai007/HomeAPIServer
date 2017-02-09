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
            //string fileName = Path.GetFileName("http://stoneapipic.bstone.com/Pictures/荒料和板材/其他/巴玉 (巴基斯坦)_tile_6_1b.jpg");
            //Console.WriteLine(fileName);
            //var tlist=fileName.Substring(fileName.IndexOf("tile_")).Split('_');
            //if (tlist.Length > 2)
            //{
            //     Console.WriteLine(tlist[1]);                
            //}
        //    ChangeFileName(new DirectoryInfo(flodPath));
            ChangeFileNameNoWhere(new DirectoryInfo(System.Configuration.ConfigurationManager.AppSettings["flodPath"].ToString()));
        }




        /// <summary>
        /// 修改文件名
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="isOPic"></param>
        /// <param name="picType"></param>
        static void ChangeFileNameNoWhere(DirectoryInfo dir)//搜索文件夹中的文件
        {

            List<string> fileNamelist = new List<string>();
            FileInfo[] allFile = dir.GetFiles();
            Computer MyComputer = new Computer();
            int iStart = int.Parse(System.Configuration.ConfigurationManager.AppSettings["iStarName"].ToString());
            foreach (var file in allFile)
            {
                if (Path.GetExtension(file.FullName).ToLower() == ".jpg")
                {
                    MyComputer.FileSystem.RenameFile(file.FullName, (iStart++).ToString()+".jpg");
                }
            }
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
