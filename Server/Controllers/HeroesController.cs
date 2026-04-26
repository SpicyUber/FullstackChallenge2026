using Infrastructure.Persistence.UnitOfWork;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace Server.Controllers
{
    [Route("api/heroes")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public HeroesController(IUnitOfWork uow) { _uow = uow; }

        [HttpGet]
        public async Task<ActionResult<CharacterDto>> GetAll()
        {
            var result = await new GetAllHeroesQuery(_uow).Execute();
            return Ok(result);
        }


    }
}
