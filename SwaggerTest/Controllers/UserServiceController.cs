using SwaggerTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SwaggerTest.Controllers
{
    public class UserServiceController : ApiController
    {
        // GET api/userservice/5
        public User Get(int id)
        {
            return new User() { Id = id, LoginId = "test", UserName = "lake" };
        }

        // POST api/userservice
        public bool Post([FromBody]User user)
        {
            if (user.LoginId == "test")
                return true;
            else
                return false;
        }
    }
}
