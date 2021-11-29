using HB.Case.Models.PostProduct;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HB.Case.Api.Models.Dtos
{
    public class CategoryDto : Dto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public override string RedisKey => $"category:{Id}";
    }
}
