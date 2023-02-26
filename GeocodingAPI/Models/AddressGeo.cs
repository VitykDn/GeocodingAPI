using NSwag.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GeocodingAPI.Models
{
    public class AddressGeo
    {
        [Key]
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

        public int CoordinateId { get; set; }
        [JsonIgnore]
        [SwaggerIgnore]
        public CoordinateGeo? Coordinate { get; set; }
    }
}
