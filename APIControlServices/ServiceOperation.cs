using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Web;

namespace APIControlServices
{
    public class ServiceOperation
    {
        log4net.ILog log;
        public ServiceOperation()
        {
            log = log4net.LogManager.GetLogger(typeof(ServiceOperation));
        }

        // <summary>  
        /// 判断是否安装了某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  
        /// <returns></returns>  
        public bool ISWindowsServiceInstalled(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();

                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            { return false; }
        }


        public bool RestartServices(string serviceName,out string msg)
        {
            msg = string.Empty;
            using (System.ServiceProcess.ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    try
                    {
                        control.Start();
                        log.Info(string.Format("启动服务成功", serviceName));
                        msg = "启动服务成功！";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("启动服务:{0}出错", serviceName), ex);
                        msg = "启动服务失败！";
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        control.Stop();
                        msg = "关闭服务成功！/r/n";
                        control.WaitForStatus(ServiceControllerStatus.Stopped,new TimeSpan(0,3,0));
                        control.Start();
                        msg = msg + "启动服务成功！";
                        return true;
                    }
                    catch (Exception ex)
                    {                        
                        msg = msg + "重启服务失败！";
                        log.Error(string.Format("{0}:{1}出错",msg, serviceName), ex);
                        return false;
                    }
                }
            }
            return false;
        }

        public bool KillAndRestartServices(string serviceName, out string msg)
        {
            msg = string.Empty;

            Process[] pro = Process.GetProcesses();//获取已开启的所有进程
            //遍历所有查找到的进程
            for (int i = 0; i < pro.Length; i++)
            {
                //判断此进程是否是要查找的进程
                if (pro[i].ProcessName.ToString().ToLower() == serviceName)
                {
                    pro[i].Kill();//结束进程
                    log.Info(string.Format("Kill Server: {0}", serviceName));
                    break;
                }
            }
           bool isOK= RestartServices(serviceName, out msg);
           msg = string.Format("Kill Server: {0} success \r\n", serviceName) + msg;
           return isOK;
        }
        /*
        public bool StartServices(string serviceName)
        {
            using (System.ServiceProcess.ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    try
                    {
                        control.Start();
                        log.Info(string.Format("启动服务成功", serviceName));    
                        return true;
                    }
                    catch (Exception ex)
                    {
                        log.Error(string.Format("启动服务:{0}出错",serviceName), ex);                        
                        return false;
                    }
                }
            }
            return false;
        }

        public bool StopService(string serviceName)
        {
            using (System.ServiceProcess.ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    try
                    {
                        control.Stop();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
*/
        /*

        /// <summary>  
        /// 启动某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  
        public void StartService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        service.Start();                       

                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                    }
                }
            }
            catch { }
        }

        /// <summary>  
        /// 停止某个服务  
        /// </summary>  
        /// <param name="serviceName"></param>  
        public void StopService(string serviceName)
        {
            try
            {
                ServiceController[] services = ServiceController.GetServices();


                foreach (ServiceController service in services)
                {
                    if (service.ServiceName == serviceName)
                    {
                        service.Stop();

                        service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                    }
                }
            }
            catch { }
        }  

        */
    }
}