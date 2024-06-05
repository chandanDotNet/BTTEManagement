using BTTEM.Data.Resources;
using BTTEM.MediatR.State.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using POS.Data.Resources;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.State
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : BaseController
    {
        readonly IMediator _mediator;
        public StateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetStates([FromQuery] StateResource stateResource)
        {
            var getAllstateQuery = new GetAllStateQueryCommand()
            {
                StateResource = stateResource
            };
            var result = await _mediator.Send(getAllstateQuery);
            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }
    }
}
