using GeocodingAPI.Models;

namespace GeocodingAPI.Data.Implementation
{
    public interface IGeoCacheAdd
    {
        Task AddAdressResultAsync(AddressResult addressResult);
        Task AddAdressRequestAsync(AddresRequest addresRequest);
        Task AddCoordinateRequestAsync(CoordinateRequest coordinateRequest);
        Task AddCoordinateResultAsync(CoordinateResult coordinateResult);
    }
}
