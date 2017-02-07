using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CompanyMoneyAndCpuTrace
{
    class Program
    {
        private static System.Timers.Timer getInfoTimer;
        static void Main(string[] args)
        {
            
            TellTraceInfo();
            InitTimer();
            Thread thread = new Thread(new ThreadStart(WriteConcurrent));
            thread.Start();
            while (1 > 0)
            {
                string strInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(strInput) && strInput.ToLower() == "exit")
                {
                    break;
                }
                TraceAll(strInput);
            }
            Console.WriteLine("程序运行结束！");
            TimerEnd();
            thread.Abort();
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
            Console.WriteLine("***********        监控开始            ********************");
            Console.WriteLine("***********        停止：end           ********************");
            Console.WriteLine("***********        退出：exit          ********************");
            TimerStart();
        }


        static void TraceEnd()
        {
            TimerEnd();
            Console.WriteLine("***********        监控结束                    ********************");
        }

        #endregion

        #region 定时器控制

        static void InitTimer()
        {
            int interval = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["traceTime"]) ? 500 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["traceTime"]);
            getInfoTimer = new System.Timers.Timer();
            getInfoTimer.Interval = interval;
            getInfoTimer.Elapsed += TimerOperation;
        }

        static void TimerOperation(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(string.Format("******{0}:定时器运行******",DateTime.Now));
            MyComputerInfo info = new MyComputerInfo() { AddTime = DateTime.Now.ToString("yyyyMMdd hh:mm:ss:fff"), CpuUse = GetCpuUseing(), MemoryUse = GetMemoryUseing() };
            GlobalClass.AddUserInfo(info);
            Console.WriteLine(string.Format("当前电脑的闲置内存:{0}%,CPU使用:{1}%",GetMemoryUseing(),GetCpuUseing()));
        }

        static void TimerStart()
        {
            getInfoTimer.Start();
            WriteCsvStart();
        }

        static void TimerEnd()
        {
            getInfoTimer.Stop();
        }

        #endregion


        #region 队列数据写入

        static void NewThreadRun()
        {           
               Thread thread = new Thread(new ThreadStart(WriteConcurrent));
               thread.Start();
        }

        static void WriteConcurrent()
        {
            while (1 > 0)
            {
                int iCount = GlobalClass.GetCount();
                if (iCount > 0)
                {
                    List<MyComputerInfo> lists = GlobalClass.QueueToList(iCount);
                    List<string> contexts = new List<string>();
                    foreach (var temp in lists)
                    {
                        contexts.Add(string.Format("{0},{1},{2}", temp.AddTime, temp.MemoryUse, temp.CpuUse));
                    }
                    WriteCsv(contexts);
                    GlobalClass.ClearQuene(iCount);
                }
                Thread.Sleep(100);               
            }
        }


        static void WriteCsvStart()
        {
            string fileName = "1.csv";
            using (
            StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8))
            {
                sw.WriteLine("时间,空闲内存,CPU使用");
                sw.Close();
            }
        }

        static void WriteCsv(List<string> contexts)
        {
            string fileName = "1.csv";
            using (
            StreamWriter sw = new StreamWriter(fileName, true, Encoding.UTF8))
            {
                foreach (var temp in contexts)
                {
                    sw.WriteLine(temp);
                }
                sw.Close();
            }
        }

        #endregion

        static string GetMemoryUseing()
        {
            ComputerInfo ci = new ComputerInfo();
            float freeRadio = 0f;
            freeRadio = ci.AvailablePhysicalMemory * 100 / ci.TotalPhysicalMemory;
            return freeRadio.ToString();
        }

        static string GetCpuUseing()
        {
            float cpuUsing = 0f;
            using (
            PerformanceCounter pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                pcCpuLoad.MachineName = ".";
                pcCpuLoad.NextValue();
                System.Threading.Thread.Sleep(100);
                cpuUsing = pcCpuLoad.NextValue();
            }
            return cpuUsing.ToString();
        }
    }


}
