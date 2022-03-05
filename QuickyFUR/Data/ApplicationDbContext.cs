using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickyFUR.Data.Models;

namespace QuickyFUR.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(@"Server=.;Database=QuickyFUR;Trusted_Connection=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Designer> Designers { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ConfiguratedProduct> ConfiguratedProducts { get; set;}
        public DbSet<Cart> Carts { get; set; }
    }
}