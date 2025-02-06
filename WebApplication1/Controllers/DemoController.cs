using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace WebApplication1.Controllers
{
    public class DemoController : Controller
    {
        // GET
        public ActionResult Index()
        {
            var service = new GetMySqlController();
            List<Dictionary<string, object>> dataList = service.Main();
            Console.WriteLine(dataList);
            
            // 印出所有資料
            foreach (var row in dataList)
            {
                Console.WriteLine($"ID: {row["id"]}, Name: {row["title"]}");
            }
            
            
            ViewData["Name"] = "Demo";
            ViewBag.Text = "Milk";
            TempData["Color"] = "Red";
            return View(dataList);
        }

        public ActionResult TableEdit(int id)
        {
            Data data = GetDataById(id);
            return View(data);
        }

        [HttpPost]
        public ActionResult TableEdit(Data data)
        {
            if (ModelState.IsValid)
            {
                UpdateData(data);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        
      
        string connectionString = "Server=localhost;Port=3306;Database=TestDatabase;User Id=root;Password=my-secret-pw;Allow User Variables=true;";
        public class Data
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }

        private Data GetDataById(int id)
        {
            Data data = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Title FROM TestTable WHERE ID = @ID";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data = new Data
                            {
                                Id = reader.GetInt32("ID"),
                                Title = reader["Title"]?.ToString() ?? string.Empty
                            };
                        }
                    }
                }
            }
            return data;
        }
        private void UpdateData(Data data)
        {
            Console.WriteLine(data.Title);
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE TestTable SET Title = @Title WHERE ID = @ID";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", data.Title);
                    command.Parameters.AddWithValue("@ID", data.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        [HttpGet]
        public string ShowHelloWorld()
        {
            return "Hello World";
        }
        
        [HttpGet] //可省略
        public string ShowPrice(string product, int price)
        {
            return $"is:{product},price:{price}";
        }
    }
}