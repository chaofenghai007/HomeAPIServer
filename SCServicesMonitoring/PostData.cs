using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCServicesMonitoring
{
    public class PostData
    {
        public DataType PostType { get; set; }
        public object Data { get; set; }
    }

    public class DataResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    /// <summary>
    /// 传递的数据类型
    /// </summary>
    public enum DataType
    {
        [Description("string")]
        my_string = 0,
        [Description("my_DataResult")]
        my_DataResult = 1
    }
}
