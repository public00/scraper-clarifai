using Microsoft.EntityFrameworkCore;
using Scraper.Entities;

namespace Scraper.DB
{
    public class ScraperContext : DbContext
    {
        public DbSet<Property> Properties { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-KT3QFA5\SQLEXPRESS01;Database=PropertyDb;Trusted_Connection=True;");
        } 
    }
}
