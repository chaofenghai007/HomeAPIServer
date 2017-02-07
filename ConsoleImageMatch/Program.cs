using ConsoleImageMatch.MatchService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleImageMatch
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Person p1 = new Person(1, "Scott",new List<Car>(){ new Car("宝马"),new Car("宝马2")});
            Console.WriteLine("原始值：P1:id={0}----------->name={1}------>car={2}", p1.id, p1.name, p1.cars[0].name, p1.cars[1].name);
            Person p2 = Copy<Person>(p1); //克隆一个对象  
          
            Console.WriteLine("改变P1的值");
            p1.id = 2;
            p1.name = "Lacy";
            p1.cars[0].name = "红旗";
            p1.cars[1].name = "红旗1";
            Console.WriteLine("P1:id={0}----------->name={1}------>car1={2}--car2={3}", p1.id, p1.name, p1.cars[0].name,p1.cars[1].name);
            Console.WriteLine("深度复制：P2:id={0}----------->name={1}------>car1={2}--car2={3}", p2.id, p2.name, p2.cars[0].name, p2.cars[1].name);            
            Console.ReadKey();  
            */
            Match33();
         //   Match118();
             int i=9;
        }

        static void Match118()
        {
            ImageAnalysisServiceClient client = new ImageAnalysisServiceClient();
            MatchedImageResult result = client.MatchImage(@"D:\Site\PictureRecAPI\PictureRecAPI\Picture\223.jpg", 0.5, "皂石", 0, 20);
        }


        static void Match33()
        {
            MatchServiceLocal.ImageAnalysisServiceClient client = new MatchServiceLocal.ImageAnalysisServiceClient();
            MatchServiceLocal.MatchedImageResult result = client.MatchImage(@"D:\matchImage\2.jpg", 0.5, "砂岩", 0, 20);
        }



        //要复制的实例必须可序列化，包括实例引用的其它实例都必须在类定义时加[Serializable]特性。  
        public static T Copy<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制     
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        } 
    }

    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
        //<SPAN style="COLOR: #000000">当然前题是List中的对象要实现ICloneable接口</SPAN>  
    }

    [Serializable]
    public class Car
    {
        public string name;
        public Car(string name)
        {
            this.name = name;
        }
    }  

    [Serializable]
    public class Person 
    {
        public int id;
        public string name;
        public List<Car> cars;
        public Person()
        {
            cars = new List<Car>();
        }
        public Person(int id, string name, List<Car> cars)
        {
            this.id = id;
            this.name = name;
            this.cars = cars;
        }

    }
}
