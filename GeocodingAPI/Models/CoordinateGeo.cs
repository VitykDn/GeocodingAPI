using NSwag.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GeocodingAPI.Models
{
    public class CoordinateGeo
    {
        [Key]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int AddressId { get; set; }
        [JsonIgnore]
        [SwaggerIgnore]
        public AddressGeo Address { get; set; }
    }
}
