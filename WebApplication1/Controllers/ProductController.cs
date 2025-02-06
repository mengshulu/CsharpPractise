using System.Collections.Generic;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Repositories;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApplication1.Controllers
{
    // [RoutePrefix("api/product")]
    // group tag
    /// <summary>
    /// 這是產品 API，提供查詢、建立與刪除產品的功能
    /// </summary>
    [RoutePrefix("api/product")]
    public class ProductController : ApiController
    {
        // GET api/product
        
        /// <summary>
        /// 取得全部產品
        /// </summary>
        /// <returns>產品集合</returns>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ProductRepository.GetAll();
        }

        // GET api/product/1
        
        /// <summary>
        /// 查找產品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var product = ProductRepository.GetById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // POST api/product
        
        /// <summary>
        /// 新增產品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Invalid data.");

            ProductRepository.Add(product);
            return Ok(product);
        }
    
        // PUT api/product/1
        
        /// <summary>
        /// 更新產品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody] Product product)
        {
            if (product == null || id != product.Id)
                return BadRequest("Invalid data.");

            ProductRepository.Update(product);
            return Ok(product);
        }

        // DELETE api/product/1
        
        /// <summary>
        /// 刪除產品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            ProductRepository.Delete(id);
            return Ok();
        }

        /// <summary>
        /// 匯出 excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        public HttpResponseMessage Export()
        {
            return GetFile();
        }
        
        // 假設這是你用來生成 Excel 檔案的程式碼
        private HttpResponseMessage GetFile()
        {
            // 假設你已經生成了 Excel 檔案的內容（以 byte[] 形式）
            byte[] fileContents = GenerateExcelFile();  // 這個方法是你生成 Excel 的邏輯

            // 創建 HttpResponseMessage 並設置檔案內容
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileContents)
            };

            // 設定檔案的 Content-Type，告訴瀏覽器這是 Excel 檔案
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            // 設定檔案的下載名稱
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Products.xlsx"
            };

            return response;
        }

        // 這裡是你生成 Excel 檔案的邏輯
        private byte[] GenerateExcelFile()
        {
            // 使用 EPPlus 或其他工具生成 Excel 檔案，並返回檔案內容（byte[]）
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Price";
                worksheet.Cells[2, 1].Value = 1;
                worksheet.Cells[2, 2].Value = "Apple";
                worksheet.Cells[2, 3].Value = 1.2;

                return package.GetAsByteArray();  // 返回 Excel 檔案的 byte[] 內容
            }
        }

    }
}
