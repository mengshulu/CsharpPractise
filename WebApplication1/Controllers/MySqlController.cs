using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class GetMySqlController
    {
        public List<Dictionary<string, object>> Main()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Port = 3306,
                Database = "TestDatabase",
                UserID = "root",
                Password = "my-secret-pw",
                AllowUserVariables = true,
            };
            // 儲存資料的陣列或集合
            // List<string[]> dataList = new List<string[]>();
            
            // 讀取資料
            List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();
            try
            {
                // Create a connection object
                using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
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
                            // 以下方法得到 [[value, value]]
                            // while (reader.Read())
                            // {
                            //     // Access columns by name or index
                            //     Console.WriteLine($"ID: {reader["id"]}, Name: {reader["title"]}");
                            //     // 將每列的資料存入陣列
                            //     string[] row = new string[reader.FieldCount];
                            //     for (int i = 0; i < reader.FieldCount; i++)
                            //     {
                            //         row[i] = reader[i].ToString(); // 將每個欄位轉為字串
                            //     }
                            //
                            //     // 將陣列加入集合
                            //     dataList.Add(row);
                            // }
                            
                            
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
    }
}