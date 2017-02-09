using ConsoleImgMatchThread.ServiceReference118;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleImgMatchThread
{
    class Program
    {
        static void Main(string[] args)
        {            
            InitMatchData();
            TellTraceInfo();
            Console.WriteLine("***********        退出：exit              **************");
            Console.WriteLine(string.Format("*********  请输入这次运行的线程数量(1-{0}):  ************", lists.Count())); 
            while (true)
            {
                string strInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(strInput) && strInput.ToLower() == "exit")
                {
                    break;
                }
                int iCount = 0;
                if (int.TryParse(strInput, out iCount))
                {
                    int num = 0;
                    while (!Program.IsStart())
                    {
                        Console.WriteLine("{0}:还没有开始运行，等待0.5秒再测试！--{1}", num, DateTime.Now.ToString("HH:mm:ss:fff"));
                        Thread.Sleep(500);
                        num++;
                    }
                    Console.WriteLine("开始测试！--{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
                    Program.StartThread(iCount);
                    //Console.WriteLine("测试结束！--{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
                }
                //TraceAll(strInput);
            }

            Console.WriteLine("程序运行结束！");
        }

        #region 监控程序控制
        static void TraceAll(string strControl)
        {
            if (string.IsNullOrEmpty(strControl))
            {
                Console.WriteLine("空指令，请重新输入");
                TellTraceInfo();
                return;
            }
            switch (strControl)
            {
                case "star":
                    TraceStart();
                    break;
                case "end":
                    TraceEnd();
                    break;
                default:
                    TellTraceInfo();
                    break;
            }
        }

        static void TellTraceInfo()
        {
            Console.WriteLine("        ***        监控指令                    ***");
            Console.WriteLine("       ****        开始：star                  ****");
            Console.WriteLine("      *****        结束：end                   *****");
            Console.WriteLine("       ****        退出：exit                  ****");
            Console.WriteLine("        ***        请输入指令：                ***");
        }

        static void TraceStart()
        {
            Console.WriteLine(string.Format("***********        请输入这次运行的线程数量(1-{0}):            ****************",lists.Count()));            
            string strWrite = Console.ReadLine();
            if (strWrite.ToLower() == "exit")
            {
                Console.WriteLine("***********        先停止这次测试            ********************");
                return;
            }
            int iCount = 0;
            if (int.TryParse(strWrite, out iCount))
            {
                Console.WriteLine("***********        监控开始            ********************");
                Console.WriteLine("***********        停止：end           ********************");
                Console.WriteLine("***********        退出：exit          ********************");
                if (iCount > lists.Count())
                    iCount = lists.Count();
                WcfControlAll(iCount);
            }
            else
            {
                TraceStart();
            }            
           
        }


        static void TraceEnd()
        {
           // TimerEnd();
            Console.WriteLine("***********        监控结束                    ********************");
        }

        #endregion


        #region 数据初始化

        private static ManualResetEventSlim mrs = new ManualResetEventSlim(false, 2047);
        private static List<MatchPicData> lists = new List<MatchPicData>();
        /// <summary>
        /// 测试启动的线程数
        /// </summary>
        private static int StartId = int.Parse(ConfigurationManager.AppSettings["startId"].ToString());

        static void InitMatchData()
        {
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\1.jpg", Catalog = "板岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });               
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\4.jpg", Catalog = "大理石", PageIndex = 0, PageSize = 20,  Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\2.jpg", Catalog = "板岩", PageIndex = 0, PageSize = 20,    Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\8.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20,  Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\3.jpg", Catalog = "板岩", PageIndex = 0, PageSize = 20,    Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\5.jpg", Catalog = "大理石", PageIndex = 0, PageSize = 20,  Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\9.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20,  Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\6.jpg", Catalog = "大理石", PageIndex = 0, PageSize = 20,  Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\10.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\7.jpg", Catalog = "大理石", PageIndex = 0, PageSize = 20,  Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\11.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\12.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\13.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\14.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\15.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\16.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\17.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\18.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\19.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\20.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\21.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\22.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\23.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\24.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\25.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\26.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\27.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\28.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\29.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\30.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\31.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\32.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\33.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\34.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\35.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\36.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\37.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\38.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\39.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\40.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\41.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\42.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\43.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\44.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\45.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\46.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\47.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\48.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\49.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\50.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\51.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\52.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\53.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\54.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\55.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\56.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\57.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\58.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\59.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\60.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\61.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\62.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\63.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\64.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\65.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\66.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\67.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\68.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\69.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\70.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\71.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\72.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\73.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\74.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\75.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\76.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\77.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\78.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\79.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\80.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\81.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\82.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\83.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\84.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\85.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\86.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\87.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\88.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\89.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\90.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\91.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\92.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\93.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\94.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\95.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\96.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\97.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\98.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\99.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 }); 
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\100.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\101.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\102.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\103.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\104.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\105.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\106.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\107.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\108.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });
            lists.Add(new MatchPicData() { PicPath = @"D:\Site\ImageSvc_R0119\TestMatchPic\109.jpg", Catalog = "花岗岩", PageIndex = 0, PageSize = 20, Ratio = 0.5 });

        }
        #endregion



        #region  wcf测试调用


        private static void WcfControlAll(int iCount)
        {
            int num = 0;            
            while (!Program.IsStart())
            {
                Console.WriteLine("{0}:还没有开始运行，等待0.5秒再测试！--{1}", num, DateTime.Now.ToString("HH:mm:ss:fff"));
                Thread.Sleep(500);
                num++;
            }
            Console.WriteLine("开始测试！--{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
            Program.StartThread(iCount);
            Console.WriteLine("测试结束！--{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
        }

        /// <summary>
        /// 检测wcf是否可用
        /// </summary>
        /// <returns></returns>
        private static bool IsStart()
        {
            string requestUriString = "http://192.168.1.33:8891/api/Values/GetStatus";
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
               

        /// <summary>
        /// 测试主线程
        /// </summary>
        private static void StartThread(int iCount)
        {
            int startId = iCount == 0 ? Program.StartId : iCount;
            for (int i = 0; i < startId; i++)
            {
                Task.Factory.StartNew(delegate(object obj)
                {
                    string result = Program.Run(obj);
                    Console.WriteLine("执行目标{0}-{1}，返回{2}", startId, obj, result);
                    //string strSql=""
                }, i);
            }            
            Console.WriteLine("当前时间:{0}我是主线程{1}，你们这些任务都等 2s 执行吧：\n", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2000);
            Program.mrs.Set();
        }

        /// <summary>
        /// 测试线程
        /// </summary>
        /// <param name="obj"></param>
        private static string Run(object obj )
        {
            int i = 0;
            int.TryParse(obj.ToString(), out i);

            return Match118(obj);
        }

        static string Match118(object obj)
        {
            int iNO = 0;
            //测试用
            /*
            iNO = int.Parse(obj.ToString());
            return string.Format("这次测试的数据信息:{0},{1},{2},{3},{4}", lists[iNO].PicPath, lists[iNO].Ratio, lists[iNO].Catalog, lists[iNO].PageIndex, lists[iNO].PageSize);
             * */
            //
            string trsult = "该方法出错";
            
            if (int.TryParse(obj.ToString(), out iNO))
            {
                try
                {
                    ImageAnalysisServiceClient client = new ImageAnalysisServiceClient();
                    MatchedImageResult result = client.MatchImage(lists[iNO].PicPath, lists[iNO].Ratio, lists[iNO].Catalog, lists[iNO].PageIndex, lists[iNO].PageSize);
                    trsult = string.Format("Match Pic number is {0}", result.TotalCountk__BackingField);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    trsult = "比较出错";
                }

            }
            return trsult;
        }
        #endregion
    }

    //string source, double ratio, string category, int pageIndex, int pageSize
    public class MatchPicData
    {
        public string PicPath { get; set; }
        public double Ratio{get;set;}
        public string Catalog { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
