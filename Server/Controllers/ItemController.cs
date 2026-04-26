using Infrastructure.Persistence.UnitOfWork;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace Server.Controllers
{
    [Route("api/shop")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ItemController(IUnitOfWork uow) 
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<ItemDto>> Get()
        {
            var result = await new GetRandomItemsQuery(_uow).Execute();
            return Ok(result);
        }
    }
}
