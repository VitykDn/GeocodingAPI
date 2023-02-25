using GeocodingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeocodingAPI.Data
{
    public class WebApiContext : DbContext
    {
        public WebApiContext( DbContextOptions<WebApiContext> options) : base(options)
        {

        }
        public DbSet<AddressResult> AddressResults { get; set; }
        public DbSet<AddresRequest> AddresRequests { get; set; }
        public DbSet<CoordinateRequest> CoordinateRequests { get; set; }
        public DbSet<CoordinateResult> CoordinateResults { get; set; }
    }

}
