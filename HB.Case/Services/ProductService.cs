using AutoMapper;
using HB.Case.Api.DbRepositories;
using HB.Case.Api.Models.Dtos;
using HB.Case.Models.Entities;
using System.Threading.Tasks;

namespace HB.Case.Services
{


    public interface IProductService : IServiceBase<ProductDto, Product>
    {
        new Task<ProductDto> Find(string id);
    }

    public class ProductService : ServiceBase<ProductDto, Product>, IProductService
    {
        private readonly IServiceBase<CategoryDto, Category> _categoryService;

        public ProductService(
            IMongoDbRepository<Product> productDb
            , IMapper mapper
            , ICacheRepository cache
            , ICategoryService categoryService
            ) : base(productDb, mapper, cache)
        {
            _categoryService = categoryService;
        }

        public override async Task<ProductDto> Find(string id)
        {
            ProductDto result = await Cache.Get<ProductDto>($"product:{id}");
            if (result != null)
                return result;


            ProductDto product = await base.Find(id);

            if (product.Category == null)
                product.Category = await _categoryService.Get(product.CategoryId);



            await Cache.Set(product.RedisKey, product);

            return product;
        }
    }
}
