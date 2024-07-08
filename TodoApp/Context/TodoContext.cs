using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoAPP.Context
{
    public class TodoContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-G4KNQNU\\SQLEXPRESS;database=Todo;Trusted_connection=true;TrustServerCertificate=True;");
        }
    }
}
