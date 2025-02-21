using System;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Borders;
using iText.Layout.Properties;
using Table = iText.Layout.Element.Table;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/pdf")]
    public class PdfController : ApiController
    {
        [HttpGet]
        [Route("export")]
        public IHttpActionResult ExportPdf()
        {
            // Create a memory stream to hold the PDF data
            var ms = new MemoryStream();
            using (ms)
            {
                // Create a PdfWriter instance to write to the memory stream
                using (var writer = new PdfWriter(ms))
                {
                    // Create a PdfDocument with the writer
                    using (var pdf = new PdfDocument(writer))
                    {
                        // Create a Document object to add content
                        var document = new Document(pdf);
                        
                        
                        // ✅ 指定支援中文的字型 (SimSun 或 Noto Sans CJK)
                        string fontPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fonts", "NotoSansTC-VariableFont_wght.ttf");
                        // string fontPath = "/usr/share/fonts/opentype/noto/NotoSansCJK-Regular.otf"; // Linux
                        PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, true);
                        // 全域設定中文
                        document
                            .SetFont(font)
                            .SetBold()
                            .SetFontColor(new DeviceRgb(28, 28, 28));
                        
                        
                        
                        
                        // === 定義 Lambda 方法 ===
                        Func<string, Cell> CreateCell = text => new Cell()
                            .Add(new Paragraph(text).SetFixedLeading(13))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                            .SetPadding(5f);
                        Func<string, Cell> CreateBoldCell = text => new Cell()
                            .Add(new Paragraph(text)
                                .SetFixedLeading(13)
                                .SetBold())
                            .SetPadding(5f)
                            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                            .SetTextAlignment(TextAlignment.CENTER);
                        Func<string, Cell> CreateHighlightedCell = text =>
                            new Cell()
                                .Add(new Paragraph(text)
                                    .SetBold().SetFixedLeading(13))
                                // .SetBackgroundColor(ColorConstants.ORANGE)
                                .SetPadding(5f)
                                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                .SetBackgroundColor(new DeviceRgb(239, 126, 50))
                                .SetTextAlignment(TextAlignment.CENTER);
                        
                        // === 建立橫向表格 ===
                        // ✅ 建立 Table，設定每個欄位寬度
                        float[] columnWidths = { 45f, 65f, 55f, 55f, 55f };  // 自訂單元格寬度
                        Table table = new Table(UnitValue.CreatePointArray(columnWidths));
                        // Table table = new Table(5); // 5 欄的表格
                        // ✅ 讓 Table 離左邊 100px
                        table.SetMarginLeft(50);

                        // 加入標題欄位
                        table.AddCell(CreateHighlightedCell("電子支付")); // 橘色
                        table.AddCell(CreateBoldCell("30分鐘\n以內免費"));
                        table.AddCell(CreateCell("充電\n停車位"));
                        table.AddCell(CreateCell("鐵馬\n休息區"));
                        table.AddCell(CreateHighlightedCell("停車場\n登記證")); // 橘色

                        // 加入表格到 PDF
                        document.Add(table);
        
                        // Add some content to the PDF
                        document.Add(new Paragraph("Hello, World! 嗨嗨"));
                        document.Add(new Paragraph("This is a simple PDF generated using iTextSharp."));
        
                        // Add additional content as needed, such as tables, images, etc.
                        // Example: Add another paragraph with some styling
                        var styledParagraph = new Paragraph("Styled Text")
                            .SetBold()
                            .SetFontSize(16);
                        document.Add(styledParagraph);
                        
                        
                        // ✅ 建立外部 Table (只放 1 列)
                        float[] columnWidthsForTwo = { 300f };  // 設定寬度
                        // UnitValue.CreatePercentValue(100)
                        // Table tableForTwo = new Table(UnitValue.CreatePointArray(columnWidthsForTwo));
                        Table tableForTwo = new Table(1);
                        // 百分比寬度
                        // Table tableForTwo = new Table(UnitValue.CreatePointArray(new float[]{5, 5}));

                        tableForTwo.SetWidth(UnitValue.CreatePercentValue(100));
                        
                        // ✅ 內部 Table (兩欄)
                        // cannot convert from double to float
                        // float f = (float) d;
                        // Or if you just need to express 1.2 as a float to start with, then use 1.2f.
                        float[] innerColWidths = { 5.5f, (float)4.5 };  // 設定兩欄的寬度
                        Table innerTable1 = new Table(UnitValue.CreatePercentArray(innerColWidths));
                        innerTable1.SetWidth(UnitValue.CreatePercentValue(100));
                        innerTable1.AddCell(new Cell().Add(new Paragraph("姓名：蘋果")).SetBorder(Border.NO_BORDER));
                        innerTable1.AddCell(new Cell().Add(new Paragraph("狀態：很好")).SetBorder(Border.NO_BORDER));
                        innerTable1.AddCell(new Cell().Add(new Paragraph("姓名：香蕉")).SetBorder(Border.NO_BORDER));
                        innerTable1.AddCell(new Cell().Add(new Paragraph("行政區：天神島")).SetBorder(Border.NO_BORDER));
                        
                        innerTable1.AddCell(new Cell().Add(new Paragraph("超好吃三明治：索爾的襪子")).SetBorder(Border.NO_BORDER));
                        // 加入有虛線的表
                        Table dashedTable = new Table(1);
                        dashedTable
                            .SetWidth(UnitValue.CreatePercentValue(100))
                            .SetBorder(new DashedBorder(DeviceGray.GRAY, 1))
                            .SetPadding(5f);
                        dashedTable.AddCell(new Cell().Add(new Paragraph("宇宙明星：23位")).SetBorder(Border.NO_BORDER));
                        Cell child = new Cell()
                                .Add(new Paragraph("第一名: 金希澈").SetFontColor(new DeviceRgb(154, 154, 154))).SetPaddingLeft(55f);
                        dashedTable.AddCell(child.SetBorder(Border.NO_BORDER));
                        innerTable1.AddCell(new Cell().Add(dashedTable).SetBorder(Border.NO_BORDER));
                        

                        Table innerTable2 = new Table(UnitValue.CreatePointArray(innerColWidths));
                        innerTable2.SetWidth(UnitValue.CreatePercentValue(100));
                        innerTable2.AddCell(new Cell().Add(new Paragraph("電話號碼：000")).SetBorder(Border.NO_BORDER));
                        innerTable2.AddCell(new Cell().Add(new Paragraph("行政區：大狗區")).SetBorder(Border.NO_BORDER));

                        // ✅ 把內部 Table 加到外部 Table 的 Cell
                        tableForTwo.AddCell(new Cell().Add(innerTable1).SetBorder(Border.NO_BORDER));
                        tableForTwo.AddCell(new Cell().Add(innerTable2).SetBorder(Border.NO_BORDER));
                        document.Add(tableForTwo);
                        
                        
        
                        // 创建填充形状对象（矩形）
                        // Rectangle rectangle = new Rectangle(10, 10, 50, 50);
                        // Create a Canvas object to draw on the page
                        // PdfCanvas canvas = new PdfCanvas(pdf.GetFirstPage());
                        // canvas.SetFillColor(ColorConstants.YELLOW);
                        // canvas.Rectangle(rectangle);
                        // canvas.Fill();
        
        
                        // Add the cell to the document
                        document.Close();
                    }
                }
            }
        
            // Convert the memory stream to byte array to return as a file
            var pdfBytes = ms.ToArray();
        
            // Return the PDF as a file response
            var fileResult = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(pdfBytes)
            };
            fileResult.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            fileResult.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "GeneratedPDF.pdf"
            };
        
            return ResponseMessage(fileResult);
        }
    }
    

}
