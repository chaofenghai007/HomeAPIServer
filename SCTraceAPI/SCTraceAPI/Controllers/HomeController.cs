using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCTraceAPI.Controllers
{
    public class HomeController : Controller
    {
        [OAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
