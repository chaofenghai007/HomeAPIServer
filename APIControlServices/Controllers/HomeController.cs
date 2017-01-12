using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace APIControlServices.Controllers
{
    public class HomeController : Controller
    {
       [OAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index1()
        {
            return View();
        }
    }
}
