using HB.Case.Api.Models.Dtos;
using HB.Case.Models.Entities;
using HB.Case.Services;
using Microsoft.AspNetCore.Mvc;

namespace HB.Case.Controllers
{
    [Route("api/categories")]
    public class CategoriesController : BaseController<CategoryDto, Category>
    {
        public CategoriesController(IServiceBase<CategoryDto, Category> service) : base(service) { }
    }
}
