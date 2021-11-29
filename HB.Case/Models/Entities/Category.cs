using HB.Case.Api.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HB.Case.Models.Entities
{
    [BsonCollection("categories")]
    public class Category : Entity<Category>
    {
        public Category(string id) : base(id) { }

        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Description { get; set; }
    }
}


