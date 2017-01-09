using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTestImageMatch.Models
{
    public class MatchResult
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
        public List<MatchStoneBase> data { get; set; }
    }

    public class MatchStoneBase
    {
        /// <summary>
        /// 石种ID
        /// </summary>
        public int StoneBaseID { get; set; }
        /// <summary>
        /// 国家ID
        /// </summary>
        public int CountryID { get; set; }
        /// <summary>
        /// 国家名称英文
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// 国家名称中文
        /// </summary>
        public string CountryName_CN { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 石种名称
        /// </summary>
        public string MaterialName { get; set; }
        /// <summary>
        /// 石种中文名称
        /// </summary>
        public string MaterialName_CN { get; set; }
        /// <summary>
        /// 材质名称
        /// </summary>
        public string CatalogName { get; set; }
        /// <summary>
        /// 材质名称(中文)
        /// </summary>
        public string CatalogName_CN { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string ColorName { get; set; }
        /// <summary>
        /// 颜色中文
        /// </summary>
        public string ColorName_CN { get; set; }
        /// <summary>
        /// 相识度
        /// </summary>
        public double Similarity { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string FilePath { get; set; }
    }
}