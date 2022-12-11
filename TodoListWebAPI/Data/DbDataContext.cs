using Microsoft.EntityFrameworkCore;
using TodoListWebAPI.Models;
namespace TodoListWebAPI.Data
{
    public class DbDataContext : DbContext
    {
        public DbDataContext(DbContextOptions<DbDataContext> options) : base(options)
        { }

        public DbSet<TodoItem> Items { get; set; }
    }



}
