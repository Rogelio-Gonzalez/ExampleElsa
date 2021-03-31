using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleElsa.Models
{
    public class UserContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=E-REGONZALEZ\\SQLEXPRESS;Database=ElsaDatabase;Integrated Security=True;");
            }
        }
        public DbSet<UserModel> Users { get; set; }
        public UserContext()
        {

        }
    }
}
