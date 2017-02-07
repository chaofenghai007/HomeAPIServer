using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyMoneyAndCpuTrace
{
    public class GlobalClass
    {
        private static ConcurrentQueue<MyComputerInfo> _userRequest = new ConcurrentQueue<MyComputerInfo>();

        public static void AddUserInfo(MyComputerInfo uri)
        {
            GlobalClass._userRequest.Enqueue(uri);
        }

        public static int GetCount()
        {
            return GlobalClass._userRequest.Count;
        }

        public static List<MyComputerInfo> QueueToList(int iCount)
        {
            return GlobalClass._userRequest.Take(iCount).ToList<MyComputerInfo>();
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
                    MyComputerInfo product;
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

        public static bool IsContains(MyComputerInfo p)
        {
            return GlobalClass._userRequest.Contains(p);
        }
    }
}
