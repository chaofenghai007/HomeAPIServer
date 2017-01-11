using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garlic.Common.Security;

namespace SCTraceAPI
{

    public class APIOAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {            
            if (System.Configuration.ConfigurationManager.AppSettings["IsDebug"] != null && System.Configuration.ConfigurationManager.AppSettings["IsDebug"].ToString().ToLower() == "true"
                && actionContext.Request.Headers.Referrer!=null && actionContext.Request.Headers.Referrer.AbsoluteUri.ToString().ToLower().Contains("swagger/ui/index"))
                return true;
            string sessionid = System.Web.HttpContext.Current.Request.Headers["AspFilterSessionId"];
            if (string.IsNullOrEmpty(sessionid))
            {
                return false;
            }
            string key = "stonecontact";
            string iv = "NI!fb@95GUY86GfghUb#er57HB";
            Garlic.Common.Security.IEncryptionProvider encryptionProvider = new Garlic.Common.Security.DESEncryptionProvider(key, iv);
            sessionid = sessionid.Substring(2, sessionid.Length - 3);
            string plainText = encryptionProvider.Decrypt(sessionid, true);
            string Authorization = System.Configuration.ConfigurationManager.AppSettings["SC_ReAPI_User"].ToString();
            if (Authorization.Contains(plainText))
                return true;
            else
                return false;
        }
    }
    
    /// <summary>
    /// 用户合法性验证
    /// </summary>
    public class OAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        // 只需重载此方法，模拟自定义的角色授权机制  
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string sessionid = System.Web.HttpContext.Current.Request.Headers["AspFilterSessionId"];
            if (string.IsNullOrEmpty(sessionid))
            {
                return false;
            }
            string key = "stonecontact";
            string iv = "NI!fb@95GUY86GfghUb#er57HB";
            Garlic.Common.Security.IEncryptionProvider encryptionProvider = new Garlic.Common.Security.DESEncryptionProvider(key, iv);
            sessionid = sessionid.Substring(2, sessionid.Length - 3);
            string plainText = encryptionProvider.Decrypt(sessionid, true);
            string Authorization = System.Configuration.ConfigurationManager.AppSettings["SC_ReAPI_User"].ToString();
            if (Authorization.Contains(plainText))
                return true;
            else
                return false;
        }
    }
}