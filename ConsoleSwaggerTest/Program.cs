using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            

            Person p = new Person() { Age = 12, Name = "张三" };
            Person p1 = new Person() { Age = 12, Name = "张三" };

            Console.WriteLine(Equip.Equals<Person,Person>(p,p1));

            Console.ReadKey();

        }

        static void EachProperties()
        {
            Person contract = new Person { Age = 12, Name = "张三" };
            Type type = contract.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                if (i.PropertyType == typeof(string))//属性的类型判断  
                {
                    object obj = i.GetValue(contract, null);
                    string name = i.Name;
                }
            }
        } 
    }

    public class Equip
    {
        /// <summary>
        /// 比较2个对象的值是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="t1"></param>
        /// <param name="k1"></param>
        /// <returns></returns>
        public static bool Equals<T, K>(T t1, K k1)
        {
            if (t1 == null && k1 != null)
                return false;
            if (k1 == null && t1 != null)
                return false;

            //比较两个对象是否是同一类型 
            if (t1.GetType() != k1.GetType())
            {
                return false;
            }

            //反射获取值类型的所有字段 
            FieldInfo[] fields = t1.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++) //遍历字段，对各个字段进行比较 
            {
                object obj3 = fields[i].GetValue(t1);
                object obj4 = fields[i].GetValue(k1);
                if (obj3 == null)
                {
                    if (obj4 != null)
                    {
                        return false;
                    }
                }
                else if (!obj3.Equals(obj4))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }
}
