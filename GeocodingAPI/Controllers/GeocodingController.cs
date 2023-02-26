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
        public async Task<ActionResult<AddressGeo>> GeocodeAddress([FromQuery] CoordinateGeo coordinateRequest)
        {
            try
            {
                var addressResult = await _geocoding.GeocodeAddressAsync(coordinateRequest);
                await _geocoding.AddCoordinateRequestAsync(coordinateRequest);
                await _geocoding.AddAddressRequestAsync(addressResult);
                return Ok(addressResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GeocodeCoordinate")]
        public async Task<ActionResult<CoordinateGeo>>GeocodeCoordinate([FromQuery] AddressGeo addresRequest)
        {
            try
            {
                var coordinateResult = await _geocoding.GeocodeCoordinateAsync(addresRequest);
                await _geocoding.AddCoordinateRequestAsync(coordinateResult);
                await _geocoding.AddAddressRequestAsync(addresRequest);
                return Ok(coordinateResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
