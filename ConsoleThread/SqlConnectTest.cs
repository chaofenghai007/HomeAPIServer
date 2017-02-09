using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleThread
{
    public static class SqlConnectTest
    {
        private static SqlConnection con;

        private static SqlCommand com;

        static SqlConnectTest()
        {
            SqlConnectTest.con = new SqlConnection("Data Source=127.0.0.1;Database=HBOOK;User Id=sa;Password=sa;Integrated Security=false; Application name=lakeMember;Pooling=true;Max Pool Size=40000;Min Pool Size=0;");
            //SqlConnectTest.con = new SqlConnection("Data Source=192.168.1.66;Database=test;User Id=lake;Password=lake;Integrated Security=false; Application name=lakeMember;Pooling=true;Max Pool Size=40000;Min Pool Size=0;");
            SqlConnectTest.com = new SqlCommand();
            SqlConnectTest.com.Connection = SqlConnectTest.con;
        }

        public static bool ExecuteNonQuery(string Sql)
        {
            SqlConnectTest.com.CommandText = Sql;
            bool result;
            try
            {
                if (SqlConnectTest.con.State != ConnectionState.Open)
                {
                    if (SqlConnectTest.con.State != ConnectionState.Closed)
                    {
                        SqlConnectTest.con.Close();
                    }
                    SqlConnectTest.con.Open();
                }
                SqlConnectTest.com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                File.AppendAllText("e:\\errorLog\\log.txt", string.Format("时间：{0},出错{1} \r\n", DateTime.Now.ToString("HH:mm:ss:fff"), ex.Message), Encoding.UTF8);
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        public static object ExecuteScalar(string Sql)
        {
            SqlConnectTest.com.CommandText = Sql;
            object result;
            try
            {
                if (SqlConnectTest.con.State != ConnectionState.Open)
                {
                    if (SqlConnectTest.con.State != ConnectionState.Closed)
                    {
                        SqlConnectTest.con.Close();
                    }
                    SqlConnectTest.con.Open();
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("e:\\errorLog\\log.txt", string.Format("时间：{0},出错{1} \r\n", DateTime.Now.ToString("HH:mm:ss:fff"), ex.Message), Encoding.UTF8);
                result = false;
                return result;
            }
            result = SqlConnectTest.com.ExecuteScalar();
            return result;
        }
    }
}
