using BTTEM.Data.Resources;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.MediatR.HelpSupport.Command;
using MediatR;
using BTTEM.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.HelpSupport
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpSupportController : BaseController
    {
        readonly IMediator _mediater;
        public HelpSupportController(IMediator mediator)
        {
            _mediater = mediator;
        }

        /// <summary>
        /// Get Help & Support
        /// </summary>
        /// <param name="helpSupportResource"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetHelpSupport([FromQuery] HelpSupportResource helpSupportResource)
        {
            var getAllHelpSupportQuery = new GetAllHelpSupportQuery()
            {
                HelpSupportResource = helpSupportResource
            };
            var result = await _mediater.Send(getAllHelpSupportQuery);

            var paginationMetaData = new
            {
                totalCount = result.Count,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetaData));
            return Ok(result);
        }
    }
}
