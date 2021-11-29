using HB.Case.Api.Models.Dtos;
using HB.Case.Models.Entities;
using HB.Case.Services;
using Microsoft.AspNetCore.Mvc;

namespace HB.Case.Controllers
{
    [Route("api/products")]
    public class ProductsController : BaseController<ProductDto, Product>
    {
        public ProductsController(IProductService service) : base(service) { }
    }
}
