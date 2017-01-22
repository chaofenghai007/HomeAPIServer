using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceClient;

namespace ConsoleSwaggerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SwaggerTest test = new SwaggerTest();
            var temp = test.UserService.Get(1);
            Console.WriteLine("ID:{0},LoginID:{1},Name:{2}",temp.Id,temp.LoginId,temp.UserName);

            Console.WriteLine(test.UserService.Post(temp));
            temp.LoginId = "lake1";
            Console.WriteLine(test.UserService.Post(temp));
        }
    }
}
