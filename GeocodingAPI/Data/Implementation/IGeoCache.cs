using GeocodingAPI.Models;

namespace GeocodingAPI.Data.Implementation
{
    public interface IGeoCache
    {
        Task SaveFromCoordinateResultAsync(AddressRequest addressRequest, CoordinateGeo coordinateResult);
        Task SaveFromAddressResultAsync(CoordinateRequest coordinateRequest, AddressGeo addressResult);
        Task<CoordinateGeo> GetCoordinateRequestAsync(AddressGeo addressGeo);
        Task<AddressGeo> GetAddressRequestAsync(CoordinateGeo coordinateGeo);
    }
}
