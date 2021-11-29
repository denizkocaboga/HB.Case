using AutoMapper;
using HB.Case.Api.DbRepositories;
using HB.Case.Api.Exceptions;
using HB.Case.Models.Entities;
using HB.Case.Models.PostProduct;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HB.Case.Services
{

    public interface IServiceBase<TDto, TEntity> where TEntity : class, IEntity where TDto : class, IDto
    {
        IMongoDbRepository<TEntity> Db { get; }
        IMapper Mapper { get; }

        Task<string> Create(TDto dto);
        Task Delete(string id);
        Task<TDto> Find(string id);
        Task<TDto> Get(string id);
        Task<IEnumerable<TDto>> GetAll();
        Task Update(string id, TDto dto);
    }
    public class ServiceBase<TDto, TEntity> : IServiceBase<TDto, TEntity> where TEntity : class, IEntity where TDto : class, IDto
    {
        protected readonly ICacheRepository Cache;

        public IMongoDbRepository<TEntity> Db { get; }

        public IMapper Mapper { get; }

        public ServiceBase(IMongoDbRepository<TEntity> db, IMapper mapper, ICacheRepository cacheProvider)
        {
            Db = db;
            Mapper = mapper;
            Cache = cacheProvider;
        }

        public virtual async Task<string> Create(TDto dto)
        {
            await ValidateConflict(dto.Id);

            TEntity entity = Map(dto);
            await Db.Add(entity);

            return dto.Id;
        }

        public virtual async Task Delete(string id)
        {
            await ValidateNotFound(id);

            await Db.Delete(id);
        }

        public virtual async Task<TDto> Find(string id)
        {
            TDto result = await Get(id);

            if (result == default)
                throw new NotFoundException();

            return result;
        }

        public virtual async Task<TDto> Get(string id)
        {
            TEntity entity = await Db.Find(id);
            TDto result = Map(entity);

            return result;
        }

        public virtual async Task<IEnumerable<TDto>> GetAll()
        {
            IList<TEntity> entities = await Db.GetAll();
            IEnumerable<TDto> result = Map(entities);
            return result;
        }

        public virtual async Task Update(string id, TDto dto)
        {
            await ValidateNotFound(id);

            TEntity entity = Map(dto);
            await Db.Update(id, entity);

            await Cache.Remove(dto.RedisKey);
        }

        #region Validates
        protected async Task ValidateCreate(string id)
        {
            await ValidateConflict(id);
        }

        protected async Task ValidateConflict(string id)
        {
            bool isExists = await Db.Exists(id);
            if (isExists)
                throw new ConflictException();
        }

        protected async Task ValidateDelete(string id) => await ValidateNotFound(id);

        protected async Task ValidateNotFound(string id)
        {
            bool isExists = await Db.Exists(id);
            if (!isExists)
                throw new NotFoundException();
        }
        #endregion

        #region Protected
        protected TDto Map(TEntity entity)
        {
            return Mapper.Map<TDto>(entity);
        }
        protected TEntity Map(TDto entity)
        {
            return Mapper.Map<TEntity>(entity);
        }
        protected IEnumerable<TDto> Map(IEnumerable<TEntity> entity)
        {
            return Mapper.Map<IEnumerable<TDto>>(entity);
        }
        protected IEnumerable<TEntity> Map(IEnumerable<TDto> entity)
        {
            return Mapper.Map<IEnumerable<TEntity>>(entity);
        }
        #endregion

    }


}
