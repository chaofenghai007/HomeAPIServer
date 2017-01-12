using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIControlServices.Models
{
    public class PostData
    {
        public DataType PostType { get; set; }
        public object Data { get; set; }
    }
}