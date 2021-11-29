using HB.Case.Api.Attributes;
using HB.Case.Models.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace HB.Case.Models.Entities
{
    [BsonCollection("products")]
    public class Product : Entity<Product>
    {
        public Product(string id) : base(id) { }

        [BsonRequired]
        public string Name { get; set; }
        public string Description { get; set; }

        [BsonRequired]
        public decimal Price { get; set; }

        [BsonRequired]
        public Currency Currency { get; set; }

        public string CategoryId { get; set; }
    }

}


