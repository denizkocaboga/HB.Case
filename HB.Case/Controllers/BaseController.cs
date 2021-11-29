using HB.Case.Models.Entities;
using HB.Case.Models.PostProduct;
using HB.Case.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HB.Case.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TDto, TEntity> : ControllerBase where TEntity : class, IEntity where TDto : class, IDto
    {
        private readonly ILogger<BaseController<TDto, TEntity>> _logger;
        private readonly IServiceBase<TDto, TEntity> _service;

        public BaseController(IServiceBase<TDto, TEntity> service)
        {
            _service = service;
        }

        //Response'de sadece get endpoint url'i dönüyor. Client bu url ile yaratılan objeye erişebilir.
        //Create edilen objenin son hali de yollanabilir. İhtiyaca göre değişir.         
        protected CreatedAtActionResult Created<T>(T id, string action = null, string controller = null) where T : IConvertible//Primitive Type
        {
            CreatedAtActionResult result = CreatedAtAction(action ?? "get", controller, new { id }, null /*dto*/);
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Get()//ToDo: Pagination with BaseRequest and BaseResponse
        {
            IEnumerable<TDto> result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            TDto result = await _service.Find(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TDto dto)
        {
            string id = await _service.Create(dto);

            return Created(id);
            //return Accepted();//Note : EventBus'a vaktim olursa response bu olacak. TraceId ile client'ın işlem durumunu takip etmesi sağlanır.

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] TDto value)
        {
            await _service.Update(id, value);

            return NoContent();
            //return Accepted();//Note : EventBus'a vaktim olursa response bu olacak. TraceId ile client'ın işlem durumunu takip etmesi sağlanır.
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);
            return NoContent();
            //return Accepted();//Note : EventBus'a vaktim olursa response bu olacak. TraceId ile client'ın işlem durumunu takip etmesi sağlanır.
        }
    }
}
