using Infrastructure.Persistence.UnitOfWork;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace Server.Controllers
{
    [Route("api/move")]
    [ApiController]
    public class MoveController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public MoveController(IUnitOfWork uow) { _uow = uow; }

        [HttpGet]
        public async Task<ActionResult<MoveDto>> GetMove([FromQuery]CharacterBattleStateDto dto)
        {
            var result = await new GetRecommendedMoveQuery(_uow).Execute(dto);
            return Ok(result);
        }

    }
}
