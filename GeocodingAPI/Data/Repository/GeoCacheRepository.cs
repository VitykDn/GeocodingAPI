using GeocodingAPI.Data.Implementation;
using GeocodingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeocodingAPI.Data.Repository
{
    public class GeoCacheRepository : IGeoCacheAdd
    {
        private WebApiContext _context;
        public GeoCacheRepository(WebApiContext context)
        {
            _context = context;
        }

        public async Task AddAdressRequestAsync(AddresRequest addresRequest)
        {
            _context.AddresRequests.Add(addresRequest);
            await _context.SaveChangesAsync();
        }

        public async Task AddAdressResultAsync(AddressResult addressResult)
        {
            _context.AddressResults.Add(addressResult);
            await _context.SaveChangesAsync();
        }

        public async Task AddCoordinateRequestAsync(CoordinateRequest coordinateRequest)
        {
            _context.CoordinateRequests.Add(coordinateRequest);
            await _context.SaveChangesAsync();
        }

        public async Task AddCoordinateResultAsync(CoordinateResult coordinateResult)
        {
            _context.CoordinateResults.Add(coordinateResult);
            await _context.SaveChangesAsync();
        }
    }
}
