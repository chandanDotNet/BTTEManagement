using BTTEM.Data.Resources;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using POS.API.Helpers;
using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace POS.API.Controllers.MMT
{
    [Route("api/[controller]")]
    [ApiController]
    public class MMTController : BaseController
    {
        readonly IMediator _mediator;
        public MMTController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get All MMT Cities
        /// </summary>
        /// <param name="mmtCityResource"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetMMTCities")]
        public async Task<IActionResult> GetMMTCities([FromQuery] MMTCityResource mmtCityResource)
        {
            var getAllCityQuery = new GetAllMMTCityQuery
            {
                 MMTCityResource = mmtCityResource
            };
            var result = await _mediator.Send(getAllCityQuery);

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
