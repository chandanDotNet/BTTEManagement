using BTTEM.Data.Resources;
using BTTEM.MediatR.Branch.Command;
using BTTEM.MediatR.BusinessArea.Command;
using BTTEM.MediatR.CostCenter.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.CostCenter
{
    [Route("api")]
    [ApiController]
    public class CostCenterController : BaseController
    {
        readonly IMediator _mediator;
        public CostCenterController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetCostCenterBranches")]
        public async Task<IActionResult> GetCostCenterBranches([FromQuery] CostCenterResource costCenterResource)
        {
            var getAllCostCenterQueryCommand = new GetAllCostCenterQueryCommand()
            {
                CostCenterResource = costCenterResource
            };
            var result = await _mediator.Send(getAllCostCenterQueryCommand);
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

        [HttpGet("GetBusinessAreas")]
        public async Task<IActionResult> GetBusinessAreas([FromQuery] BusinessAreaResource businessAreaResource)
        {
            var getAllBusinessAreaQueryCommand = new GetAllBusinessAreaQueryCommand()
            {
                BusinessAreaResource = businessAreaResource
            };
            var result = await _mediator.Send(getAllBusinessAreaQueryCommand);
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
