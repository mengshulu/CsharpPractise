using System;
using System.Collections.Generic;
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
        class DataItem
        {
            public string Name { get; set; }
            public string Text { get; set; }
        }
        
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
                        var document = new Document(pdf, PageSize.A4);
                        
                        # region ===設定成中文===
                        // ✅ 指定支援中文的字型 (SimSun 或 Noto Sans CJK)，否則會無法顯示中文
                        // 使用專案目錄下的字型包,目前用 Google-思源黑體
                        string fontPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fonts", "NotoSansTC-VariableFont_wght.ttf");
                        // string fontPath = "/usr/share/fonts/opentype/noto/NotoSansCJK-Regular.otf"; // Linux
                        PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, true);
                        // 全域設定中文,粗體,顏色
                        document
                            .SetFont(font)
                            .SetBold()
                            .SetFontColor(new DeviceRgb(28, 28, 28))
                            .SetProperty(Property.LEADING, new Leading(Leading.MULTIPLIED, 1.5f));
                        # endregion
                        
                        # region ===狀態表格===
                        Func<string, bool, bool, Cell> createCell = (text, isBold, isHighlighted) =>
                        {
                            var paragraph = new Paragraph(text).SetFixedLeading(13);
                            if (isBold) paragraph.SetBold();

                            var cell = new Cell()
                                .Add(paragraph)
                                .SetPadding(5f)
                                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                .SetTextAlignment(TextAlignment.CENTER);

                            if (isHighlighted) cell.SetBackgroundColor(new DeviceRgb(239, 126, 50));

                            return cell;
                        };
                        // === 建立橫向表格 ===
                        // ✅ 建立 Table，設定每個欄位寬度
                        float[] columnWidths = { 45f, 65f, 55f, 55f, 55f };  // 自訂單元格寬度, f = float
                        Table statusTable = new Table(UnitValue.CreatePointArray(columnWidths));
                        // Table table = new Table(5); // 5 欄的表格
                        // ✅ 讓 Table 離左邊 50px
                        statusTable.SetMarginLeft(50).SetMarginBottom(30);
                        // 加入標題欄位
                        statusTable.AddCell(createCell("電子支付", false, true));
                        statusTable.AddCell(createCell("30分鐘\n以內免費", true, false));
                        statusTable.AddCell(createCell("充電\n停車位", false, false));
                        statusTable.AddCell(createCell("鐵馬\n休息區", false, false));
                        statusTable.AddCell(createCell("停車場\n登記證", false, true));
                        // 加入表格到 PDF
                        document.Add(statusTable);
                        # endregion

                        float[] innerColWidths = { 5, 5 };  // 設定兩欄的寬度
                        Table ad = new Table(UnitValue.CreatePercentArray(innerColWidths));
                        ad.SetWidth(UnitValue.CreatePercentValue(100));
                        
                        // 左邊文字
                        Div leftDiv = new Div();
                        leftDiv.SetWidth(UnitValue.CreatePercentValue(100));
                        // leftDiv.SetTextAlignment(TextAlignment.LEFT);
                        
                        Func<string, Cell> createCellParagraph = (text) =>
                        {
                            var paragraph = new Paragraph(text);
                            var cell = new Cell()
                                .Add(paragraph)
                                .SetBorder(Border.NO_BORDER);
                            return cell;
                        };
                        
                        # region ===左邊內容===
                        // 使用明確類型的 List
                        List<DataItem> list = new List<DataItem>
                        {
                            new DataItem
                            {
                                Name = "姓名：",
                                Text = "蘋果"
                            },
                            new DataItem
                            {
                                Name = "狀態：",
                                Text = "很好"
                            },
                            new DataItem
                            {
                                Name = "品種：",
                                Text = "香蕉"
                            },
                            new DataItem
                            {
                                Name = "行政區：",
                                Text = "天神島"
                            },
                            new DataItem
                            {
                                Name = "無敵辣漢堡(嗝)：",
                                Text = "摩斯辣味漢堡"
                            },
                            new DataItem
                            {
                                Name = "好喝飲料：",
                                Text = "天仁茗茶"
                            },
                            new DataItem
                            {
                                Name = "無敵辣漢堡(嗝)：",
                                Text = "摩斯辣味漢堡"
                            },
                            new DataItem
                            {
                                Name = "好喝飲料：",
                                Text = "天仁茗茶"
                            },
                            new DataItem
                            {
                                Name = "無敵辣漢堡(嗝)：",
                                Text = "摩斯辣味漢堡"
                            },
                            new DataItem
                            {
                                Name = "好喝飲料：",
                                Text = "天仁茗茶"
                            },
                            new DataItem
                            {
                                Name = "超好吃三明治：",
                                Text = "索爾的襪子"
                            },
                            new DataItem
                            {
                                Name = "我要豆花：",
                                Text = "板橋區民族段569 、570\n570-2 、570-3\n570 -4地號"
                            },
                            new DataItem
                            {
                                Name = "練舞室：",
                                Text = "小型車月租:每月3500元(全日)、每月 2000元(夜間晚上5點至隔日8點 國定例 假日全日)"
                            },
                        };

                        Table innerParagraph1 = new Table(new float[] { 100, 1 })
                            .UseAllAvailableWidth()
                            .SetWidth(UnitValue.CreatePercentValue(100));
                        foreach (var item in list)
                        {
                            innerParagraph1.AddCell(createCellParagraph(item.Name).SetTextAlignment(TextAlignment.RIGHT));
                            innerParagraph1.AddCell(createCellParagraph(item.Text));
                        }
                        leftDiv.Add(innerParagraph1);
                        # endregion

                        // 右邊文字 (自適應)
                        Div rightDiv = new Div();
                        rightDiv.SetWidth(UnitValue.CreatePercentValue(100));
                        rightDiv.SetTextAlignment(TextAlignment.LEFT);
                        // rightDiv.Add(new Paragraph("起司"));
                        
                        # region ===有虛線的表===
                        // 加入有虛線的表
                        Table dashedTable = new Table(1);
                        dashedTable
                            .SetWidth(UnitValue.CreatePercentValue(100))
                            .SetBorder(new DashedBorder(DeviceGray.GRAY, 1))
                            .SetPadding(5f)
                            .SetMarginLeft(20);
                        dashedTable.AddCell(createCellParagraph("宇宙明星：23位"));
                        string[] childText =
                        {
                            "第一名: 金希澈",
                            "第二名: 劉在石",
                            "第三名: 張員瑛",
                            "第四名: 安俞貞"
                        };
                        foreach (var ctx in childText)
                        {
                            Cell cell = new Cell()
                                .Add(new Paragraph(ctx).SetFontColor(new DeviceRgb(154, 154, 154)))
                                .SetPaddingLeft(55f);
                            dashedTable.AddCell(cell.SetBorder(Border.NO_BORDER));
                        }
                        // innerTable1.AddCell(new Cell(5, 1).Add(dashedTable).SetBorder(Border.NO_BORDER));
                        rightDiv.Add(dashedTable);
                        # endregion

                        # region ===右邊內容===
                        
                        List<DataItem> list2 = new List<DataItem>
                        {
                            new DataItem
                            {
                                Name = "無敵辣漢堡(嗝)：",
                                Text = "摩斯辣味漢堡"
                            },
                            new DataItem
                            {
                                Name = "好喝飲料：",
                                Text = "天仁茗茶"
                            },
                            new DataItem
                            {
                                Name = "索爾的襪子：",
                                Text = "超好吃三明治"
                            },
                            new DataItem
                            {
                                Name = "收費拉各位爹：",
                                Text = "收費月費100億"
                            },
                            new DataItem
                            {
                                Name = "練舞室：",
                                Text = "小型車月租:每月3500元(全日)、每月 2000元(夜間晚上5點至隔日8點 國定例 假日全日)"
                            },
                            new DataItem
                            {
                                Name = "西川教授：",
                                Text = "快幫我解決"
                            }
                        };
                        foreach (var item in list2)
                        {
                            Table innerParagraph = new Table(new float[] { 100, 1 })
                                .UseAllAvailableWidth()
                                .SetWidth(UnitValue.CreatePercentValue(100));
                            innerParagraph.AddCell(createCellParagraph(item.Name).SetTextAlignment(TextAlignment.RIGHT));
                            innerParagraph.AddCell(createCellParagraph(item.Text));
                            rightDiv.Add(innerParagraph);
                        }
                        # endregion

                        // 把左右兩欄加到 Paragraph，確保它們是同一行
                        ad.AddCell(new Cell().Add(leftDiv).SetBorder(Border.NO_BORDER));
                        ad.AddCell(new Cell().Add(rightDiv).SetBorder(Border.NO_BORDER));

                        document.Add(ad);
                        
                        
                        # region ===表格寫法===
                        // ✅ 建立外部 Table (只放 1 列)
                        // Table tableForTwo = new Table(1);
                        // 百分比寬度
                        // Table tableForTwo = new Table(UnitValue.CreatePointArray(new float[]{5, 5}));
                        // tableForTwo.SetWidth(UnitValue.CreatePercentValue(100));

                        // ✅ 內部 Table (兩欄)
                        // Or if you just need to express 1.2 as a float to start with, then use 1.2f.
                        // float[] innerColWidths = { 5, 5 };  // 設定兩欄的寬度
                        // Table innerTable1 = new Table(UnitValue.CreatePercentArray(innerColWidths));
                        // innerTable1.SetWidth(UnitValue.CreatePercentValue(100));
                        // ✅ 把內部 Table 加到外部 Table 的 Cell
                        // tableForTwo.AddCell(new Cell().Add(innerTable1).SetBorder(Border.NO_BORDER));
                        // tableForTwo.AddCell(new Cell().Add(innerTable2).SetBorder(Border.NO_BORDER));
                        // document.Add(tableForTwo);
                        # endregion
                        
                        # region ===矩形===
                        // 创建填充形状对象（矩形）
                        // Rectangle rectangle = new Rectangle(10, 10, 50, 50);
                        // Create a Canvas object to draw on the page
                        // PdfCanvas canvas = new PdfCanvas(pdf.GetFirstPage());
                        // canvas.SetFillColor(ColorConstants.YELLOW);
                        // canvas.Rectangle(rectangle);
                        // canvas.Fill();
                        # endregion
        
        
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
