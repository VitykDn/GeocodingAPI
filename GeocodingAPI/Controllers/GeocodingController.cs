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
        private readonly GeocodingRepository _geocoding;
        private readonly ILogger<GeocodingController> _logger;

        public GeocodingController(GeocodingRepository geocoding, ILogger<GeocodingController> logger)
        {
            _geocoding = geocoding;
            _logger = logger;
        }


        /// <summary>
        /// Handles the HTTP GET request to geocode an address.
        /// </summary>
        /// <param name="coordinateRequest"></param>
        /// <returns></returns>
        [HttpGet("GeocodeAddress")]
        public async Task<ActionResult<AddressGeo>> GetGeocodeAddress([FromQuery] CoordinateRequest coordinateRequest)
        {
            try
            {
                var addressResult = await _geocoding.GeocodeAddressAsync(coordinateRequest);

                await _geocoding.SaveFromAddressResultAsync(coordinateRequest, addressResult);
                _logger.LogInformation($"GeocodeAddress success for {coordinateRequest.Latitude},{coordinateRequest.Longitude}");

                return Ok(addressResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GeocodeAddress error for {coordinateRequest.Latitude},{coordinateRequest.Longitude}");

                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Handles the HTTP GET request to geocode a coordinates.
        /// </summary>
        /// <param name="addressRequest"></param>
        /// <returns></returns>
        [HttpGet("GeocodeCoordinate")]
        public async Task<ActionResult<CoordinateGeo>> GetGeocodeCoordinate([FromQuery] AddressRequest addressRequest)
        {
            try
            {
                var coordinateResult = await _geocoding.GeocodeCoordinateAsync(addressRequest);

                await _geocoding.SaveFromCoordinateResultAsync(addressRequest, coordinateResult);

                _logger.LogInformation($"GeocodeCoordinate success for {addressRequest.Address}, {addressRequest.City}, {addressRequest.State}, {addressRequest.Country}");

                return Ok(coordinateResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GeocodeCoordinate error for {addressRequest.Address}, {addressRequest.City}, {addressRequest.State}, {addressRequest.Country}");
                return BadRequest(ex.Message);
            }
        }



        ////For Refactoring
        ////
        ////
        //[HttpGet("GetCoordinate")]
        //public async Task<ActionResult<CoordinateGeo>> GetGeoCoordinateTest(AddressGeo addressRequest)
        //{
        //    try
        //    {
        //        var coordinateResult = await _geocoding.GetCoordinateRequestAsync(addressRequest);
        //        return Ok(coordinateResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}