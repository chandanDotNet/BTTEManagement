using BTTEM.Data.Resources;
using BTTEM.MediatR.Branch.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.Branch
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : BaseController
    {
        readonly IMediator _mediator;
        public BranchController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetBranches([FromQuery] BranchResource branchResource)
        {
            var getAllBranchQueryCommand = new GetAllBranchQueryCommand()
            {
                BranchResource = branchResource
            };
            var result = await _mediator.Send(getAllBranchQueryCommand);
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
