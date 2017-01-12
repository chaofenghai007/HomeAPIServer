using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace APIControlServices.Models
{
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