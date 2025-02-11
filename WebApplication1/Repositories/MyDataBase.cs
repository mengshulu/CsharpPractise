using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using WebApplication1.Configs;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class MyDataBase
    {
        
        public List<Dictionary<string, object>> GetProducts()
        {
            // 連接共用 config
            string connectionString = DatabaseConfig.GetConnectionString();
            // 讀取資料
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
            try
            {
                // Create a connection object
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();
                    Console.WriteLine("Connection to MySQL database established.");

                    // SQL query to execute
                    string query = "SELECT * FROM TestTable";
                    // Create a command object
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Execute the query and get the result set
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            // 以下方法得到 [{key: value}]
                            while (reader.Read())
                            {
                                Dictionary<string, object> row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader[i]; // 使用欄位名稱作為 Key
                                }
                                dataList.Add(row);
                            }

                            // 轉換為 JSON 格式
                            string json = JsonConvert.SerializeObject(dataList, Formatting.Indented);
                            Console.WriteLine(json);
                        }
                    }
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
            return dataList;
        }

        public void UpdateProduct(Product data)
        {
            // 連接共用 config
            string connectionString = DatabaseConfig.GetConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // 一定要有這一段來打開與資料庫的連線
                    connection.Open();
                    string query = "UPDATE TestTable SET Title = @Title WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // command.Parameters.AddWithValue 可以防止注入攻擊 SQL injection
                        command.Parameters.AddWithValue("@Title", data.Title);
                        command.Parameters.AddWithValue("@ID", data.Id);
                        // 執行
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
                throw;
            }

        }

        public void AddProduct(Product data)
        {
            // 連接共用 config
            string connectionString = DatabaseConfig.GetConnectionString();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // 兩種寫法都可以，指定欄位或是不指定欄位，但按照欄位順序新增值
                    string query = "INSERT INTO TestTable Values(@id, @title)";
                    // string query = "INSERT INTO TestTable (id, title) VALUES (@id, @title)";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", data.Title);
                        command.Parameters.AddWithValue("@ID", data.Id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public void DeleteProduct(int id)
        {
            // 連接共用 config
            string connectionString = DatabaseConfig.GetConnectionString();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM TestTable WHERE ID = @ID";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public Product GetDataById(int id)
        {
            Product data = null;
            // 連接共用 config
            string connectionString = DatabaseConfig.GetConnectionString();

            try
            {
                // using 會自動關閉資料庫連線，因此不用再 connection.Close();
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
                                data = new Product
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}