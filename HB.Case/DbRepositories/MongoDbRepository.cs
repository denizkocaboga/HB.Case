using HB.Case.Api.Attributes;
using HB.Case.Models.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Case.Api.DbRepositories
{
    public interface IRepository<T> where T : class, IEntity
    {
    }
    public interface IMongoDbRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<bool> Exists(string id);
        Task Add(TEntity entity);
        Task<bool> Delete(string id);
        Task<TEntity> Find(string id);
        Task<IList<TEntity>> GetAll();
        Task<bool> Update(string id, TEntity entity);
    }

    public class MongoDbRepository<TEntity> : IMongoDbRepository<TEntity> where TEntity : class, IEntity
    {
        private IMongoCollection<TEntity> Collection { get; }

        public MongoDbRepository(IMongoClient client)
        {
            IMongoDatabase db = client.GetDatabase("hbcase");

            Collection = db.GetCollection<TEntity>(GetCollectionName());
        }

        public async Task<bool> Exists(string id)
        {
            bool result = await Collection.AsQueryable().AnyAsync(p => p.Id == id);
            return result;
        }

        public string GetCollectionName()
        {
            return ((BsonCollectionAttribute)typeof(TEntity).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault())?.CollectionName;
        }

        public async Task<IList<TEntity>> GetAll()
        {
            List<TEntity> result = await (await Collection.FindAsync(p => true)).ToListAsync<TEntity>();
            return result;
        }

        public async Task<TEntity> Find(string id)
        {
            TEntity result = await (await Collection.FindAsync(p => p.Id == id)).FirstOrDefaultAsync();
            return result;
        }

        public async Task Add(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public async Task<bool> Update(string id, TEntity entity)
        {
            ReplaceOneResult result = await Collection.ReplaceOneAsync(p => p.Id == id, entity);
            return result.IsAcknowledged;
        }

        public async Task<bool> Delete(string id)
        {
            DeleteResult result = await Collection.DeleteOneAsync(p => p.Id == id);

            return result.IsAcknowledged;
        }
    }
}
