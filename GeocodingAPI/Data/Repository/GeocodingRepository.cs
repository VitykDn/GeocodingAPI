using GeocodingAPI.Models;
using System.Net.Http;
using System.Text.Json;

namespace GeocodingAPI.Data.Repository
{
    public class GeocodingRepository
    {
        private readonly HttpClient _httpClient;

        public GeocodingRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<AddressResult> GeocodeAddressAsync(CoordinateRequest coordinateRequest)
        {
            var requestUrl = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={coordinateRequest.Latitude}&lon={coordinateRequest.Longitude}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<AddressResult>(responseContent, options);

            return new AddressResult
            {
                Address = result.Address,
                City = result.City,
                State = result.State,
                PostalCode = result.PostalCode,
                Country = result.Country
            };
        }
    }
}
