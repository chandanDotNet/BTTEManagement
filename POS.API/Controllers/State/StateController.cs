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
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StateController'
    public class StateController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StateController'
    {
        readonly IMediator _mediator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StateController.StateController(IMediator)'
        public StateController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StateController.StateController(IMediator)'
        {
            _mediator = mediator;
        }

        [HttpGet]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'StateController.GetStates(StateResource)'
        public async Task<IActionResult> GetStates([FromQuery] StateResource stateResource)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'StateController.GetStates(StateResource)'
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
