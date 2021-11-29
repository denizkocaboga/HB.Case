using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HB.Case.Models.Entities
{
    public interface IEntity
    {
        public string EntityName { get; }

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }//Note: Direk Mongo Id'si mi isteniyor?
    }

    public abstract class Entity<T> : IEntity where T : IEntity
    {
        public virtual string EntityName => nameof(T).ToLower();

        public Entity(string id) => Id = id;

        public string Id { get; set; }

    }
}
