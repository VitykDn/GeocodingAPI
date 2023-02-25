﻿using GeocodingAPI.Data.Implementation;
using GeocodingAPI.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace GeocodingAPI.Data.Repository
{
    public class GeocodingRepository : IGeocoding, IGeoCacheAdd
    {
        private readonly HttpClient _httpClient;
        private WebApiContext _context;

        public GeocodingRepository(HttpClient httpClient, WebApiContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<CoordinateResult> GeocodeCoordinateAsync(AddresRequest addressRequest)
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WebAPI/1.0");

            var requestUrl = $"https://nominatim.openstreetmap.org/search?format=json&street={addressRequest.Address}" +
                $"&city={addressRequest.City}&state={addressRequest.State}&postalcode={addressRequest.PostalCode}" +
                $"&country={addressRequest.Country}";

            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

            if (result.Count == 0)
            {
                throw new Exception("No results found");
            }

            var coordinatesResult = new CoordinateResult
            {
                Latitude = (double)result.lat,
                Longitude = (double)result.lon
            };

            return coordinatesResult;
        }

        public async Task<AddressResult> GeocodeAddressAsync(CoordinateRequest coordinateRequest)
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

            var addressResult = new AddressResult
            {
                Address = result.address.road,
                City = result.address.city,
                State = result.address.state,
                PostalCode = result.address.postcode,
                Country = result.address.country
            };
            return addressResult;
        }

        public async Task AddAdressRequestAsync(AddresRequest addresRequest)
        {
            _context.AddresRequests.Add(addresRequest);
            await _context.SaveChangesAsync();
        }

        public async Task AddAdressResultAsync(AddressResult addressResult)
        {
            _context.AddressResults.Add(addressResult);
            await _context.SaveChangesAsync();
        }

        public async Task AddCoordinateRequestAsync(CoordinateRequest coordinateRequest)
        {
            _context.CoordinateRequests.Add(coordinateRequest);
            await _context.SaveChangesAsync();
        }

        public async Task AddCoordinateResultAsync(CoordinateResult coordinateResult)
        {
            _context.CoordinateResults.Add(coordinateResult);
            await _context.SaveChangesAsync();
        }


    }
}
