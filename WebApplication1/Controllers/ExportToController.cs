using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    // 此方法會讓 swagger 出現 parameters null 錯誤
    public class ExportController : Controller
    {
        // 假設你有一個產品列表
        public class ProductA
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        // GET: Export/ToExcel
        public ActionResult ToExcel()
        {
            // 假設從資料庫或其他來源取得的資料
            List<ProductA> products = new List<ProductA>
            {
                new ProductA { Id = 1, Name = "Apple", Price = 1.2M },
                new ProductA { Id = 2, Name = "Banana", Price = 0.8M },
                new ProductA { Id = 3, Name = "Orange", Price = 1.5M }
            };

            // 使用 EPPlus 創建 Excel 檔案
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");

                // 添加表頭
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Price";

                // 填充資料
                for (int i = 0; i < products.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = products[i].Id;
                    worksheet.Cells[i + 2, 2].Value = products[i].Name;
                    worksheet.Cells[i + 2, 3].Value = products[i].Price;
                }

                // 設置 HTTP 回應的內容類型和檔案名稱
                byte[] fileContents = package.GetAsByteArray();
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
            }
        }
    }

}