using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class WordController : ApiController
    {
        [HttpGet]
        [Route("api/word/download")]
        public HttpResponseMessage DownloadWord()
        {
            try
            {
                // **1. 產生 Word 文件**
                byte[] fileBytes = GenerateWordDocument();
    
                // **2. 回傳 HTTP 響應**
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileBytes)
                };
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "ApiDocumentation.docx"
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
    
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    
        // **產生 Word 文件 (Byte 陣列)**
        private byte[] GenerateWordDocument()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document, true))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
    
                    // **插入標題**
                    body.AppendChild(new Paragraph(new Run(new Text("API 文件"))));
    
                    // **產生表格**
                    Table table = new Table();
    
                    // **設定表格邊框**
                    TableProperties tblProps = new TableProperties(
                        new TableBorders(
                            new TopBorder { Val = BorderValues.Single, Size = 12 },
                            new BottomBorder { Val = BorderValues.Single, Size = 12 },
                            new LeftBorder { Val = BorderValues.Single, Size = 12 },
                            new RightBorder { Val = BorderValues.Single, Size = 12 },
                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 6 },
                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 6 }
                        )
                    );
                    table.AppendChild(tblProps);
    
                    // **加入表頭**
                    TableRow headerRow = new TableRow();
                    headerRow.Append(
                        CreateTableCell("API 名稱"),
                        CreateTableCell("請求方式"),
                        CreateTableCell("描述")
                    );
                    table.Append(headerRow);
    
                    // **加入 API 資料**
                    List<Tuple<string, string, string>> apiData = new List<Tuple<string, string, string>>
                    {
                        Tuple.Create("GetUser", "GET", "取得使用者資料"),
                        Tuple.Create("CreateUser", "POST", "新增使用者"),
                        Tuple.Create("UpdateUser", "PUT", "更新使用者"),
                        Tuple.Create("DeleteUser", "DELETE", "刪除使用者")
                    };
    
                    foreach (var api in apiData)
                    {
                        TableRow row = new TableRow();
                        row.Append(
                            CreateTableCell(api.Item1),
                            CreateTableCell(api.Item2),
                            CreateTableCell(api.Item3)
                        );
                        table.Append(row);
                    }
    
                    // **把表格加入 Word**
                    body.Append(table);
                }
    
                return memoryStream.ToArray();
            }
        }
    
        // **建立表格儲存格**
        private static TableCell CreateTableCell(string text)
        {
            return new TableCell(new Paragraph(new Run(new Text(text))))
            {
                TableCellProperties = new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Auto }
                )
            };
        }
    }
}


