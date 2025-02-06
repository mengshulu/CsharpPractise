using MySql.Data.MySqlClient;

namespace WebApplication1.Configs
{
    public static class DatabaseConfig
    {
        private static readonly string Server = "localhost";
        private static readonly uint Port = 3306;
        private static readonly string Database = "TestDatabase";
        private static readonly string UserID = "root";
        private static readonly string Password = "my-secret-pw";
        private static readonly bool AllowUserVariables = true;

        public static string GetConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = Server,
                Port = Port,
                Database = Database,
                UserID = UserID,
                Password = Password,
                AllowUserVariables = AllowUserVariables,
            };
            return builder.ConnectionString;
        }
    }
}