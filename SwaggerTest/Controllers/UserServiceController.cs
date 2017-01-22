using SwaggerTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace SwaggerTest.Controllers
{
    /// <summary>
    /// swagger类说明
    /// </summary>
    public class UserServiceController : ApiController
    {
        /// <summary>
        /// 根据id取值
        /// </summary>
        /// <param name="id">对应的id</param>
        /// <returns></returns>
        public User Get(int id)
        {
            return new User() { Id = id, LoginId = "test", UserName = "lake" };
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public bool Post([FromBody]User user)
        {
            if (user.LoginId == "test")
                return true;
            else
                return false;
        }
    }
}
