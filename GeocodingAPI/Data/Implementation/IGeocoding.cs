using GeocodingAPI.Models;

namespace GeocodingAPI.Data.Implementation
{
    public interface IGeocoding
    {
        public Task<string> GeocodeAddressAsync(CoordinateRequest coordinateRequest);
        public Task<AddressResult> GeocodeCoordinateAsync(AddresRequest addresRequest);

    }
}
