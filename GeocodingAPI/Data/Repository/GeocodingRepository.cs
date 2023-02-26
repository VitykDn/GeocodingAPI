using GeocodingAPI.Data.Implementation;
using GeocodingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Nominatim.API.Models;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace GeocodingAPI.Data.Repository
{
    public class GeocodingRepository : IGeocoding, IGeoCache
    {
        private readonly HttpClient _httpClient;
        private WebApiContext _context;

        public GeocodingRepository(HttpClient httpClient, WebApiContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<CoordinateGeo> GeocodeCoordinateAsync(AddressRequest addressRequest)
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WebAPI/1.0");

            var requestUrl = $"https://nominatim.openstreetmap.org/search?format=json&street={addressRequest.Address}" +
                $"&city={addressRequest.City}&state={addressRequest.State}&postalcode={addressRequest.PostalCode}" +
                $"&country={addressRequest.Country}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent,serializerSettings);

            if (result.Count == 0)
            {
                throw new Exception("No results found");
            }

            var coordinatesResult = new CoordinateGeo
            {
                Latitude = (double)result.lat,
                Longitude = (double)result.lon
            };
            //var document = JsonDocument.Parse(responseContent);
            //var root = document.RootElement;

            //if (root.GetArrayLength() == 0)
            //{
            //    throw new Exception("No results found");
            //}

            //var coordinatesResult = new CoordinateGeo
            //{
            //    Latitude = root.GetProperty("lat").GetDouble(),
            //    Longitude = root.GetProperty("lon").GetDouble()
            //};

            return coordinatesResult;
        }

        public async Task<AddressGeo> GeocodeAddressAsync(CoordinateRequest coordinateRequest)
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WebAPI/1.0");
            var longitude = coordinateRequest.Longitude.ToString(CultureInfo.InvariantCulture);
            var latitude = coordinateRequest.Latitude.ToString(CultureInfo.InvariantCulture);

            //var requestUrl = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=37.7749&lon=-122.4194";
            var requestUrl = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latitude}&lon={longitude}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

            var addressResult = new AddressGeo
            {
                Address = result.address.road,
                City = result.address.city,
                State = result.address.state,
                PostalCode = result.address.postcode,
                Country = result.address.country,
            };

            //var result = JsonSerializer.Deserialize<JsonElement>(responseContent);

            //var addressResult = new AddressGeo
            //{
            //    Address = result.GetProperty("address").GetProperty("road").GetString(),
            //    City = result.GetProperty("address").GetProperty("city").GetString(),
            //    State = result.GetProperty("address").GetProperty("state").GetString(),
            //    PostalCode = result.GetProperty("address").GetProperty("postcode").GetString(),
            //    Country = result.GetProperty("address").GetProperty("country").GetString()
            //};
            return addressResult;
        }

        public async Task AddAddressRequestAsync(AddressGeo addresRequest)
        {
            _context.AddressGeos.Add(addresRequest);
            await _context.SaveChangesAsync();
        }

        public async Task AddCoordinateRequestAsync(CoordinateGeo coordinateRequest)
        {
            _context.CoordinateGeos.Add(coordinateRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<CoordinateGeo> GetCoordinateRequestAsync(AddressGeo addressGeo)
        {
            var coordinateGeo = await _context.CoordinateGeos
                .SingleOrDefaultAsync(c => c.AddressId == addressGeo.Id);

            return coordinateGeo;
        }

        public Task<AddressGeo> GetAddressRequestAsync(CoordinateGeo coordinateGeo)
        {
            throw new NotImplementedException();
        }
    }
}
