using BTTEM.Data;
using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using POS.Helper;
using System;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.RequestACall
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestACallController : BaseController
    {
        readonly IMediator _mediator;
        public RequestACallController(IMediator mediator)
        {
            _mediator = mediator;
        }
        ///// <summary>
        ///// Get All Reuest Call
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> GetRequestCall()
        //{
        //    return Ok();
        //}


        /// <summary>
        /// Gets Request Call
        /// </summary>
        /// <param name="assignedTo">The identifier.</param>
        /// <returns></returns>
        [HttpGet("GetRequestCall/{userid}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetRequestCall(Guid? assignedTo)
        {
            var query = new GetRequestCallQuery { AssignedTo = assignedTo };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Request A Call.
        /// </summary>
        /// <param name="addRequestCallCommand">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(RequestCallDto))]
        public async Task<IActionResult> RequestCall([FromBody] AddRequestCallCommand addRequestCallCommand)
        {
            var result = await _mediator.Send(addRequestCallCommand);
            //return ReturnFormattedResponse(result);
            return Ok(result.Data.RequestNo);
        }
    }
}
