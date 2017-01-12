using APIControlServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace APIControlServices.Controllers
{
    public class KRProcessController : ApiController
    {
            [HttpPost]
            [OAuthorize]
            public DataResult Post(PostData postData)
            {
                string processName;
                if (postData.PostType == DataType.my_string)
                    processName = postData.Data.ToString();
                else
                    processName = System.Configuration.ConfigurationManager.AppSettings["ServicesName"].ToString();
                DataResult dr = new DataResult();
                ServiceOperation so = new ServiceOperation();
                if (so.ISWindowsServiceInstalled(processName))
                {
                    string msg = string.Empty;
                    dr.success = so.KillAndRestartServices(processName, out msg);
                    dr.message = msg;
                }
                else
                {
                    dr.success = false;
                    dr.message = "服务不存在！";
                }
                return dr;


            }
        }

}
