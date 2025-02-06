using System;
using System.Threading.Tasks;
using MySqlConnector;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using WebApplication1.Controllers;
using WebApplication1.Models; // 確保命名空間與你的專案匹配

namespace WebApplication1.Repositories
{
    public static class ProductRepository
    {
        
        public static List<Product> GetAll()
        {
            
            
            MyDataBase db = new MyDataBase();
            List<Dictionary<string, object>> dataList = db.GetProducts();
            // 得到資料以後，重新整理資料
            // 不知道為什麼要多這段，感覺有點多餘，但又覺得好像可以讓人更清楚拿到了什麼資料
            return dataList.Select(row => new Product
            {
                Id = Convert.ToInt32(row["id"]), // 確保欄位名稱對應
                Title = row["title"].ToString()   // 轉為字串
            }).ToList();
        }
        // public static List<Product> GetAll() => products;
        public static Product GetById(int id)
        {
            MyDataBase db = new MyDataBase();
            Product data = db.GetDataById(id);
            return data;
        }

        public static void Add(Product data)
        {
            MyDataBase db = new MyDataBase();
            db.AddProduct(data);
        }
        public static void Update(Product product)
        {
            MyDataBase db = new MyDataBase();
            db.UpdateProduct(product);
        }

        public static void Delete(int id)
        {
            MyDataBase db = new MyDataBase();
            db.DeleteProduct(id);
        }
    }
}
