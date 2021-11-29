using AutoMapper;
using HB.Case.Api.DbRepositories;
using HB.Case.Api.Models.Dtos;
using HB.Case.Models.Entities;

namespace HB.Case.Services
{
    public interface ICategoryService : IServiceBase<CategoryDto, Category>
    {
    }
    public class CategoryService : ServiceBase<CategoryDto, Category>, ICategoryService
    {
        public CategoryService(IMongoDbRepository<Category> db, IMapper mapper, ICacheRepository cacheProvider) : base(db, mapper, cacheProvider) { }
    }
}
