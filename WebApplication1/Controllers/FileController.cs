using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// 檔案上傳
    /// </summary>
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        
        /// <summary>
        /// 上傳單一檔案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadFile")] // 確保 API 路徑為 api/file/uploadFile
        public IHttpActionResult UploadFile()
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                var file = HttpContext.Current.Request.Files[0]; // 取得上傳的檔案
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string uploadPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads"), fileName);
                    // 如果資料夾不存在，則建立
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    file.SaveAs(uploadPath); // 儲存檔案

                    return Ok(new { message = "檔案上傳成功", fileName });
                }
            }
            return BadRequest("沒有上傳任何檔案");
        }
        
        /// <summary>
        /// 上傳多個檔案
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadMultipleFiles")]
        public IHttpActionResult UploadMultipleFiles()
        {
            var files = HttpContext.Current.Request.Files;
            List<string> uploadedFiles = new List<string>();
        
            if (files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string uploadPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads"), fileName);
                        // 如果資料夾不存在，則建立
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }
                        file.SaveAs(uploadPath); // 儲存檔案
                        uploadedFiles.Add(fileName);
                    }
                }
                return Ok(new { message = "檔案上傳成功", files = uploadedFiles });
            }
            return BadRequest("沒有上傳任何檔案");
        }
    }
}