using MVCTestImageMatch.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVCTestImageMatch.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {          
            HttpFileCollectionBase files = Request.Files;
            var file = files[0];
            string returnfilePath = string.Empty;
            var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));            
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                file.SaveAs(fileName);
                returnfilePath = "/Upload/" + file.FileName;
                TempData["filePath"] = returnfilePath;
                TempData["catagory"] = form["seleCatagory"];
                return RedirectToAction("Show");
               // return View("Show", returnfilePath);
            }
            catch (Exception ex)
            {   
                ViewBag.Message = "上传失败！" + ex.Message;
                return View();
            }
        }

        public ActionResult Show()
        {
            ViewBag.FilePath = TempData["filePath"];

            string sourcePath = Server.MapPath(TempData["filePath"].ToString());            
            
            AnalysisNew.ImageAnalysisServiceClient client = new AnalysisNew.ImageAnalysisServiceClient();
            var result = client.MatchImage(sourcePath, 0.2, TempData["catagory"].ToString(), 0, 20);
            List<string> picPath = new List<string>();
            List<Tuple<string, string>> filePath = new List<Tuple<string, string>>();
            //result.MatchedImagesk__BackingField[0].File
            foreach (var temp in result.MatchedImagesk__BackingField)
            {
                filePath.Add(new Tuple<string, string>(RepalceFile(temp.File), temp.Result.ToString()));               
            }
            ViewBag.Result = filePath;
            //旧版本        
            List<Tuple<string, string>> filePathOld = new List<Tuple<string, string>>();
            var oldMatch= GetOldImageMatch(sourcePath, TempData["catagory"].ToString());
            foreach (var temp in oldMatch.data)
            {
                filePathOld.Add(new Tuple<string, string>(temp.FilePath.Replace("s.jpg","b.jpg"), temp.Similarity.ToString()));
            }
            ViewBag.ResultOld = filePathOld;
            return View();
        }

        private string RepalceFile(string sourceFile)
        {
            string oldChar = System.Configuration.ConfigurationManager.AppSettings["oldChar"].ToString();
            string newChar = System.Configuration.ConfigurationManager.AppSettings["newChar"].ToString();
            return sourceFile.Replace(oldChar, newChar).Replace("\\", "/");
        }

        

        [HttpPost]
        public string PostTest()
        {
            string name=Request["Name"];
            var files =Request.Files;
            string returnfilePath = string.Empty;
            string fileName = string.Empty;
            for (int i = 0; i < files.Count; i++)
            {
                fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(files[i].FileName));
                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                    files[i].SaveAs(fileName);
                    returnfilePath = returnfilePath + "," + fileName;                   
                }
                catch (Exception ex)
                {
                    returnfilePath = returnfilePath + ","+ files[i].FileName+"上传失败:"+ex.Message;        
                }
            }
            return name+"|"+returnfilePath;
        }


        protected MatchResult GetOldImageMatch(string filePath, string catagory)
        {
            string serfilePath = GetUploadFilePath(filePath);
            if (string.IsNullOrEmpty(serfilePath))
                return null;
            else
                return MatchPicOld(serfilePath, catagory);
        }

        /// <summary>
        /// 上传图片到到服务器返回服务器图片路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected string GetUploadFilePath(string filePath)
        {
            HttpRequestClient c1 = new HttpRequestClient();
            string md5 = GetMD5HashFromFile(filePath);
            string parms = string.Format("hash={0}&Content-Range=bytes0-{1}/{1}", md5, new FileInfo(filePath).Length);
            string returnFilePath = c1.HttpPostFile("http://stoneapi.bstone.com/api/picture/postuploadpic", parms, new List<string>() { filePath });
            var result = JsonConvert.DeserializeObject<DataResult>(returnFilePath);
            if (result != null && result.success)
                return result.data.Url;
            else
                return null;
        }

        protected MatchResult MatchPicOld(string filePath,string catagory)
        {
            HttpRequestClient c1 = new HttpRequestClient();
            string parms = string.Format("source={0}&ratio=0.5&category={1}&pageIndex=0&pageSize=20", filePath, catagory);
            var obj = c1.HttpPostData("http://stoneapi.bstone.com/api/matchimg/postimgmatch", parms);
            return JsonConvert.DeserializeObject<MatchResult>(obj);
        }

        protected string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        
    }
}
