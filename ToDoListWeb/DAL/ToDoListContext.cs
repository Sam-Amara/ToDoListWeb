using ToDoListWeb.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ToDoListWeb.DAL
{
    public class ToDoListContext : DbContext
    {

        public ToDoListContext() : base("ToDoListContext")
        {
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}