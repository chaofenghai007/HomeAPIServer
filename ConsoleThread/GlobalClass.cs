using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsoleThread
{
    public class GlobalClass
    {
        private static ConcurrentQueue<Product> _userRequest = new ConcurrentQueue<Product>();

        public static void AddUserInfo(Product uri)
        {
            GlobalClass._userRequest.Enqueue(uri);
        }

        public static int GetCount()
        {
            return GlobalClass._userRequest.Count;
        }

        public static List<Product> QueueToList(int iCount)
        {
            return GlobalClass._userRequest.Take(iCount).ToList<Product>();
        }

        public static bool ClearQuene(int iCount)
        {
            bool result;
            try
            {
                if (iCount > GlobalClass._userRequest.Count)
                {
                    iCount = GlobalClass._userRequest.Count;
                }
                for (int i = 0; i < iCount; i++)
                {
                    Product product;
                    GlobalClass._userRequest.TryDequeue(out product);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static bool IsContains(Product p)
        {
            return GlobalClass._userRequest.Contains(p);
        }
    }
}