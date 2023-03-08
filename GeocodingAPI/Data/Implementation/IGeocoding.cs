using GeocodingAPI.Models;

namespace GeocodingAPI.Data.Implementation
{
    public interface IGeocoding
    {
        public Task<AddressGeo> GeocodeAddressAsync(CoordinateRequest coordinateRequest);
        public Task<CoordinateGeo> GeocodeCoordinateAsync(AddressRequest addresRequest);

    }
}
