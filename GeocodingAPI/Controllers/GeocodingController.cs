using GeocodingAPI.Data.Implementation;
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
        private readonly IGeocoding _geocoding;

        public GeocodingController(IWebHostEnvironment webHostEnvironment, IGeoCacheAdd geoCacheAdd,
            IGeocoding geocoding)
        {
            _webHostEnvironment = webHostEnvironment;
            _geocoding = geocoding;
        }
        [HttpGet("GeocodeAddress")]
        public async Task<ActionResult<AddressResult>> GeocodeAddress([FromQuery] CoordinateRequest coordinateRequest)
        {
            try
            {
                var addressResult = await _geocoding.GeocodeAddressAsync(coordinateRequest);

                return Ok(addressResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
