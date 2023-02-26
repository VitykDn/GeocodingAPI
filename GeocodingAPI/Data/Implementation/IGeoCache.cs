using GeocodingAPI.Models;

namespace GeocodingAPI.Data.Implementation
{
    public interface IGeoCache
    {
        Task AddAddressRequestAsync(AddressGeo addressGeo);
        Task AddCoordinateRequestAsync(CoordinateGeo coordinateGeo);
        Task<CoordinateGeo> GetCoordinateRequestAsync(AddressGeo addressGeo);
        Task<AddressGeo> GetAddressRequestAsync(CoordinateGeo coordinateGeo);
    }
}
