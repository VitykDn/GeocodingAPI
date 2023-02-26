using GeocodingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeocodingAPI.Data
{
    public class WebApiContext : DbContext
    {
        public WebApiContext( DbContextOptions<WebApiContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressGeo>()
                .HasOne(a => a.Coordinate)
                .WithOne(c => c.Address)
                .HasForeignKey<CoordinateGeo>(c => c.AddressId);

            modelBuilder.Entity<CoordinateGeo>()
                .HasOne(a => a.Address)
                .WithOne(c => c.Coordinate)
                .HasForeignKey<AddressGeo>(c => c.CoordinateId);
        }

        public DbSet<AddressGeo> AddressGeos { get; set; }
        public DbSet<CoordinateGeo> CoordinateGeos { get; set; }
    }

}
