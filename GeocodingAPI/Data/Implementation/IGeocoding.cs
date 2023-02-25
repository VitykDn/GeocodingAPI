using GeocodingAPI.Models;

namespace GeocodingAPI.Data.Implementation
{
    public interface IGeocoding
    {
        public Task<AddressResult> GeocodeAddressAsync(CoordinateRequest coordinateRequest);
        public Task<AddressResult> GeocodeCordinateAsync(AddresRequest addresRequest);

    }
}
