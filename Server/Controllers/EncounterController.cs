using Infrastructure.Persistence.UnitOfWork;
using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace Server.Controllers
{
    [Route("api/encounters")]
    [ApiController]
    public class EncounterController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public EncounterController(IUnitOfWork uow) { _uow = uow; }

        [HttpGet]
        public async Task<ActionResult<EncounterTreeNodeDto>> GetMap()
        {
            var result = await new GetEncounterMapQuery(_uow).Execute();
            return Ok(result);
        }
    }
}
