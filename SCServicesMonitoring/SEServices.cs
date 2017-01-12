using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SCServicesMonitoring
{
    public partial class SEServices : ServiceBase
    {
        System.Timers.Timer timer1; 
        public SEServices()
        {
            InitializeComponent();            
            
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new System.Timers.Timer();
            var Interval = System.Configuration.ConfigurationManager.AppSettings["Timer_SC_Interval"];
            // timer1.Interval = 60*60* 1000;  //设置计时器事件间隔执行时间,一小时监控一次
            timer1.Interval = (Interval == null ? (60 * 60) : int.Parse(Interval.ToString())) * 1000;  //设置计时器事件间隔执行时间,一小时监控一次
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Operation);
            timer1.Enabled = true;

          //  App.Logging.Info("监控服务启动(MonitoringServies start)");
        }

        protected override void OnStop()
        {
        }

        private void Timer1_Operation(object sender, System.Timers.ElapsedEventArgs e)
        {
            ServiceOperation SO = new ServiceOperation();
            SO.CheckServices();
            WebServiceMonitoring mainWeb = new WebServiceMonitoring();
            mainWeb.CheckMainWebServices();
        }
    }
}
