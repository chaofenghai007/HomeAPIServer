using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garlic.Common.Security;

namespace APIControlServices
{
    public class OAuthorizeAttribute : AuthorizeAttribute  
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