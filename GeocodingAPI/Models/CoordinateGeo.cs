using NSwag.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GeocodingAPI.Models
{
    public class CoordinateGeo
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [JsonIgnore]
        public int? AddressId { get; set; }
        [JsonIgnore]
        public AddressGeo? Address { get; set; }
    }
}
