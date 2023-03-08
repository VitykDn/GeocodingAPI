using NSwag.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GeocodingAPI.Models
{
    public class AddressGeo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        [JsonIgnore]
        public int? CoordinateId { get; set; }
        [JsonIgnore]
        [OpenApiIgnore]
        public CoordinateGeo? Coordinate { get; set; }
    }
}
