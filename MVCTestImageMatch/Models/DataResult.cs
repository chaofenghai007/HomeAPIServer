using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTestImageMatch.Models
{
    public class DataResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public ImageResult data { get; set; }
    }

    public class ImageResult
    {
        public string Url { get; set; }
        public int NextPosition { get; set; }
    }
}