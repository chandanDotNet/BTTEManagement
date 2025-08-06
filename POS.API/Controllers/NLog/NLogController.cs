using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using POS.API.Helpers;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'NLogController'
    public class NLogController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'NLogController'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'NLogController._mediator'
        public IMediator _mediator { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'NLogController._mediator'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'NLogController.NLogController(IMediator)'
        public NLogController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'NLogController.NLogController(IMediator)'
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get System Logs
        /// </summary>
        /// <param name="nLogResource"></param>
        /// <returns></returns>
        [HttpGet]
        [ClaimCheck("LOGS_VIEW_ERROR_LOGS")]
        [Produces("application/json", "application/xml", Type = typeof(NLogList))]
        public async Task<IActionResult> GetNLogs([FromQuery] NLogResource nLogResource)
        {
            var getAllLoginAuditQuery = new GetNLogsQuery
            {
                NLogResource = nLogResource
            };
            var result = await _mediator.Send(getAllLoginAuditQuery);

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
        /// Get Log By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ClaimCheck("LOGS_VIEW_ERROR_LOGS")]
        [Produces("application/json", "application/xml", Type = typeof(NLogDto))]
        public async Task<IActionResult> GetNLog(Guid id)
        {
            var getLogQuery = new GetLogQuery { Id = id };
            var result = await _mediator.Send(getLogQuery);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Create Log.
        /// </summary>
        /// <param name="addLogCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(NLogDto))]
        public async Task<IActionResult> CreatNLog(AddLogCommand addLogCommand)
        {
            var result = await _mediator.Send(addLogCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
