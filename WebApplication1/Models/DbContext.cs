using System;
using System.Configuration;
using System.Data.Entity;

namespace WebApplication1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base(GetConnectionString()) { }

        public DbSet<User> Users { get; set; }

        private static string GetConnectionString()
        {
            Console.WriteLine("Connection");
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
    }
}