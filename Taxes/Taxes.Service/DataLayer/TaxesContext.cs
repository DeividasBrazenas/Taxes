using Microsoft.EntityFrameworkCore;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Service.DataLayer
{
    public class TaxesContext : DbContext
    {
        public TaxesContext () : base()
        {
        }

        public TaxesContext (DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Municipality>().ToTable("Municipality");
            modelBuilder.Entity<Tax>().ToTable("Tax");
        }

        public virtual DbSet<Municipality> Municipalities { get; set; }
        public virtual DbSet<Tax> Taxes { get; set; }
    }
}
