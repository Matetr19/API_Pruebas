using Microsoft.EntityFrameworkCore;
using APIPoliza.Models;

namespace APIPoliza.Data
{
    public class APIPolizaContext : DbContext
    {
        public APIPolizaContext(DbContextOptions<APIPolizaContext> options) : base(options) { }
        public DbSet<Client> vw_endpoint_cliente { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasNoKey().ToView("vw_endpoint_cliente", "mpm");
        }
    }
}