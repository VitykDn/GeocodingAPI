using GeocodingAPI.Data.Implementation;
using GeocodingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace GeocodingAPI.Data.Repository
{
    public class GeocodingRepository : IGeocoding, IGeoCache
    {
        private readonly HttpClient _httpClient;
        private WebApiContext _context;
        private readonly ILogger<GeocodingRepository> _logger;

        public GeocodingRepository(HttpClient httpClient, WebApiContext context, ILogger<GeocodingRepository> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
        }

        public async Task<CoordinateGeo> GeocodeCoordinateAsync(AddressRequest addressRequest)
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WebAPI/1.0");

            var requestUrl = $"https://nominatim.openstreetmap.org/search?format=json&street={addressRequest.Address}" +
                $"&city={addressRequest.City}&state={addressRequest.State}&postalcode={addressRequest.PostalCode}" +
                $"&country={addressRequest.Country}&limit=1";

            _logger.LogInformation($"Geocoding address for coordinates:" +
                $" Addres= {addressRequest.Address}, Country = {addressRequest.Country}" +
                $" State = {addressRequest.State}, PostCode = {addressRequest.PostalCode} ");
            _logger.LogDebug($"Geocoding reverse request URL: {requestUrl}");

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogDebug($"Geocoding response: {responseContent}");

            try
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

                if (result.Count == 0)
                {
                    _logger.LogDebug($"This response: {responseContent} has no results");
                    throw new Exception("No results found");

                }

                var coordinatesResult = new CoordinateGeo
                {
                    Latitude = (double)result[0].lat,
                    Longitude = (double)result[0].lon
                };

                _logger.LogInformation($"Geocoding result: {JsonConvert.SerializeObject(coordinatesResult)}");

                return coordinatesResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during coordinates geocoding");

                throw;
            }
        }

        public async Task<AddressGeo> GeocodeAddressAsync(CoordinateRequest coordinateRequest)
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WebAPI/1.0");
            var longitude = coordinateRequest.Longitude.ToString(CultureInfo.InvariantCulture);
            var latitude = coordinateRequest.Latitude.ToString(CultureInfo.InvariantCulture);

            //var requestUrl = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latitude}&lon={longitude}";
            var requestUrl = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=37.7749&lon=-122.4194";

            _logger.LogInformation($"Geocoding address for coordinates: {latitude}, {longitude}");
            _logger.LogInformation($"Geocoding request URL: {requestUrl}");
            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Geocoding response: {responseContent}");

                var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

                var addressResult = new AddressGeo
                {
                    Address = result.address?.road,
                    City = result.address?.city,
                    State = result.address?.state,
                    PostalCode = result.address?.postcode,
                    Country = result.address?.country,
                };
                if (addressResult.Address == null && addressResult.City == null
                    && addressResult.State == null && addressResult.PostalCode == null && addressResult.Country == null)
                {
                    throw new Exception("Geocoding result returned null values for all address fields.");
                }
                _logger.LogInformation($"Geocoding result: {JsonConvert.SerializeObject(addressResult)}");

                return addressResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during address geocoding");

                throw;
            }
        }

        public async Task SaveFromCoordinateResultAsync(AddressRequest addressRequest, CoordinateGeo coordinateResult)
        {
            var addressResult = new AddressGeo()
            {
                Address = addressRequest.Address,
                City = addressRequest.City,
                Country = addressRequest.Country,
                State = addressRequest.State,
                PostalCode = addressRequest.PostalCode,
                Coordinate = coordinateResult,
                CoordinateId = coordinateResult.Id
            };
            coordinateResult.AddressId = addressResult.Id;

            try
            {
                await _context.AddressGeos.AddAsync(addressResult);
                await _context.CoordinateGeos.AddAsync(coordinateResult);
                await _context.SaveChangesAsync();

                // Log success
                _logger.LogInformation($"Added new AddressGeo with ID {addressResult.Id} and CoordinateGeo with ID {coordinateResult.Id}");
            }
            catch (Exception ex)
            {
                // Log error and rethrow exception
                _logger.LogError(ex, $"Failed to add new AddressGeo and CoordinateGeo. Address: {addressRequest}, Coordinate: {coordinateResult}");
                throw;
            }
        }

        public async Task SaveFromAddressResultAsync(CoordinateRequest coordinateRequest, AddressGeo addressResult)
        {
            var coordinateResult = new CoordinateGeo()
            {
                Latitude = coordinateRequest.Latitude,
                Longitude = coordinateRequest.Longitude,
                Address = addressResult,
                AddressId = addressResult.Id
            };
            try
            {
                await _context.AddressGeos.AddAsync(addressResult);
                await _context.CoordinateGeos.AddAsync(coordinateResult);
                await _context.SaveChangesAsync();

                // Log success
                _logger.LogInformation($"Added new AddressGeo with ID {addressResult.Id} and CoordinateGeo with ID {coordinateResult.Id}");
            }
            catch (Exception ex)
            {
                // Log error and rethrow exception
                _logger.LogError(ex, $"Failed to add new AddressGeo and CoordinateGeo. Address: {addressResult}, Coordinate: {coordinateResult}");
                throw;
            }
        }



        //In Future
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