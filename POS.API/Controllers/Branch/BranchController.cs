using BTTEM.Data.Dto;
using BTTEM.Data.Resources;
using BTTEM.MediatR.Branch.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using POS.Data.Dto;
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

        /// <summary>
        /// Create Branch.
        /// </summary>
        /// <param name="addBranchCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(BranchDto))]
        public async Task<IActionResult> AddBranch([FromBody] AddBranchCommand addBranchCommand)
        {
            var result = await _mediator.Send(addBranchCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Branch.
        /// </summary>
        /// <param name="updateBranchCommand"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces("application/json", "application/xml", Type = typeof(BranchDto))]
        public async Task<IActionResult> UpdateBranch([FromBody] UpdateBranchCommand updateBranchCommand)
        {
            var result = await _mediator.Send(updateBranchCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Branch.
        /// </summary>
        /// <param name="deleteBranchCommand"></param>
        /// <returns></returns>
        [HttpDelete]
        [Produces("application/json", "application/xml", Type = typeof(BranchDto))]
        public async Task<IActionResult> UpdateBranch([FromBody] DeleteBranchCommand deleteBranchCommand)
        {
            var result = await _mediator.Send(deleteBranchCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
