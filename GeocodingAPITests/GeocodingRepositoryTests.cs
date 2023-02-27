using GeocodingAPI.Data;
using GeocodingAPI.Data.Implementation;
using GeocodingAPI.Data.Repository;
using GeocodingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace GeocodingAPITests
{
    public class GeocodingRepositoryTests
    {
        private readonly GeocodingRepository _geocodingRepository;
        private readonly Mock<WebApiContext> _mockDbContext;



        public GeocodingRepositoryTests()
        {

            _mockDbContext = new Mock<WebApiContext>();
            _geocodingRepository = new GeocodingRepository(null, _mockDbContext.Object, null);
        }
        [Fact]
        public async Task GeocodeCoordinateAsync_Returns_Coordinates()
        {
            // Arrange
            var addressRequest = new AddressRequest
            {
                Address = "Lviv",
                City = "Lviv",
                State = "",
                PostalCode = "",
                Country = "Ukraine"
            };
            var expectedCoordinates = new CoordinateGeo { Latitude = 49.841, Longitude = 24.031 };
            var responseContent = JsonConvert.SerializeObject(new[] { new { lat = expectedCoordinates.Latitude, lon = expectedCoordinates.Longitude } });
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(responseContent) };

            // Act
            var coordinates = await _geocodingRepository.GeocodeCoordinateAsync(addressRequest);

            // Assert
            Assert.Equal(expectedCoordinates.Latitude, coordinates.Latitude);
            Assert.Equal(expectedCoordinates.Longitude, coordinates.Longitude);
        }

        [Fact]
        public async Task SaveFromAddressResultAsync_AddsEntitiesToContextAndSavesChanges()
        {
            // Arrange
            var coordinateRequest = new CoordinateRequest
            {
                Latitude = 49.841,
                Longitude = 24.031
            };
            var addressResult = new AddressGeo
            {
                Id = 1,
                Address = "Lviv",
                City = "Lviv",
                State = "",
                PostalCode = "",
                Country = "Ukraine"
            };
            var expectedCoordinateResult = new CoordinateGeo
            {
                Latitude = coordinateRequest.Latitude,
                Longitude = coordinateRequest.Longitude,
                Address = addressResult,
                AddressId = addressResult.Id
            };
            var mockAddressGeos = new Mock<DbSet<AddressGeo>>();
            var mockCoordinateGeos = new Mock<DbSet<CoordinateGeo>>();

            _mockDbContext.Setup(x => x.AddressGeos).Returns(mockAddressGeos.Object);
            _mockDbContext.Setup(x => x.CoordinateGeos).Returns(mockCoordinateGeos.Object);

            // Act
            await _geocodingRepository.SaveFromAddressResultAsync(coordinateRequest, addressResult);

            // Assert
            mockAddressGeos.Verify(x => x.AddAsync(addressResult, default(CancellationToken)), Times.Once);
            mockCoordinateGeos.Verify(x => x.AddAsync(expectedCoordinateResult, default(CancellationToken)), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(default(CancellationToken)), Times.Once);
        }
    }
}
