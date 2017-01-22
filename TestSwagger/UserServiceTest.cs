using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UserServiceClient;
using UserServiceClient.Models;

namespace TestSwagger
{
    [TestFixture]
    public class UserServiceTest
    {
        private SwaggerTest test;
        private User user;
        [SetUp]
        public void InitializeOperands()
        {
            test = new SwaggerTest();
            user = new User() { Id = 1, LoginId = "test", UserName = "lake" };
        }

        /// <summary>
        /// 测试Get方法
        /// </summary>
        [Test]
        public void TestGet()
        {
            var tUser = test.UserService.Get(1);
            Assert.IsTrue(Equip.Equals<User,User>(user,tUser));
        }

        /// <summary>
        /// 测试post方法
        /// </summary>
        [Test]
        public void TestPostSuc()
        {
            Assert.IsTrue(test.UserService.Post(user).Value);            
        }

        /// <summary>
        /// 测试post方法
        /// </summary>
        [Test]
        public void TestPostFail()
        {            
            var tUser = test.UserService.Get(1);
            tUser.LoginId = "lake";
            Assert.AreEqual(test.UserService.Post(tUser).Value, false);
        }

        [TearDown]
        public void DisportOperands()
        {
            user = null;
            test = null;
            
        } 
    }
}
