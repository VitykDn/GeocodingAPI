using GeocodingAPI.Models;

namespace GeocodingAPI.Data.Implementation
{
    public interface IGeocoding
    {
        public Task<AddressGeo> GeocodeAddressAsync(CoordinateGeo coordinateRequest);
        public Task<CoordinateGeo> GeocodeCoordinateAsync(AddressGeo addresRequest);

    }
}
