using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIControlServices.Controllers
{
    public class RequestController : ApiController
    {
        // GET api/request
        public DataResult  Get(string url)
        {
            HttpClientOperation co = new HttpClientOperation();
            return co.HttpGet(url);            
        }

    }
}
