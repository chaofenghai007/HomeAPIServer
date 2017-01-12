using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIControlServices.Models
{
    public class DataResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}