using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HB.Case.Models.PostProduct
{
    public interface IDto
    {
        [JsonIgnore]
        public string RedisKey { get; }

        [Required]
        string Id { get; set; }
    }
    public abstract class Dto : IDto
    {
        [JsonIgnore]
        public abstract string RedisKey { get; }
        public string Id { get; set; }


    }
}
