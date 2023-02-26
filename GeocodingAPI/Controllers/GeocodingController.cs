using GeocodingAPI.Data.Implementation;
using GeocodingAPI.Data.Repository;
using GeocodingAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeocodingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeocodingController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GeocodingRepository _geocoding;

        public GeocodingController(IWebHostEnvironment webHostEnvironment, IGeoCache geoCacheAdd,
            GeocodingRepository geocoding)
        {
            _webHostEnvironment = webHostEnvironment;
            _geocoding = geocoding;
        }
        [HttpGet("GetCoordinate")]
        public async Task<ActionResult<CoordinateGeo>> GetGeoCoordinateTest( AddressGeo addressRequest)
        {
            try
            {
                var coordinateResult = await _geocoding.GetCoordinateRequestAsync(addressRequest);
                return Ok(coordinateResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GeocodeAddress")]
        public async Task<ActionResult<AddressGeo>> GeocodeAddress([FromQuery]CoordinateRequest coordinateRequest)
        {
            try
            {
                var addressResult = await _geocoding.GeocodeAddressAsync(coordinateRequest);

                var coordinateResult = new CoordinateGeo()
                {
                    Latitude = coordinateRequest.Latitude,
                    Longitude = coordinateRequest.Longitude,
                    Address = addressResult
                };
                await _geocoding.AddCoordinateRequestAsync(coordinateResult);
                await _geocoding.AddAddressRequestAsync(addressResult);
                return Ok(addressResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GeocodeCoordinate")]
        public async Task<ActionResult<CoordinateGeo>>GeocodeCoordinate([FromQuery] AddressRequest addresRequest)
        {
            try
            {
                var coordinateResult = await _geocoding.GeocodeCoordinateAsync(addresRequest);
                var addressResult = new AddressGeo()
                {
                    Address = addresRequest.Address,
                    City = addresRequest.City,
                    Country = addresRequest.Country,
                    State = addresRequest.State,
                    PostalCode = addresRequest.PostalCode,
                    Coordinate = coordinateResult
                };
                await _geocoding.AddCoordinateRequestAsync(coordinateResult);
                await _geocoding.AddAddressRequestAsync(addressResult);
                return Ok(coordinateResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
