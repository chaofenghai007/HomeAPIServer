using SCServicesMonitoring.PicMatch;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCServicesMonitoring
{
    /// <summary>
    /// 图片识别服务器
    /// </summary>
   public class PicMatchServiceMonitoring
    {
        log4net.ILog log;
        string apiHost;
        public PicMatchServiceMonitoring()
        {
            log = log4net.LogManager.GetLogger(typeof(PicMatchServiceMonitoring));            
        }

        public void TestMarch()
        {
            Match118();
        }

        private void Match118()
        {
            try
            {
                ImageAnalysisServiceClient client = new ImageAnalysisServiceClient();
                MatchedImageResult result = client.MatchImage(@"D:\Site\ImageSvc_R0119\TestMatchPic\1.jpg", 0.7, "板岩", 0, 20);
                log.Info(string.Format("{0}-检查了一次图片服务器",DateTime.Now.ToString("yyyyMMdd hh:mm:ss_fff")));
                if (result.TotalCountk__BackingField <= 0)
                {
                    log.Error("图片识别服务器出错");
                }
            }
            catch (Exception ex)
            {
                log.Error("图片识别服务器出错！", ex);
            }
        }
    }
}
