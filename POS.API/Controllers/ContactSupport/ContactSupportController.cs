using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.ContactSupport
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactSupportController : BaseController
    {
        readonly IMediator _mediator;
        public ContactSupportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        ///// <summary>
        ///// Gets Request Call
        ///// </summary>
        ///// <param name="assignedTo">The identifier.</param>
        ///// <returns></returns>
        //[HttpGet("GetTravelDocument/{userid}")]
        ////[ClaimCheck("EXP_VIEW_EXPENSES")]
        //public async Task<IActionResult> GetRequestCall(Guid? assignedTo)
        //{
        //    var query = new GetRequestCallQuery { AssignedTo = assignedTo };
        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}


        /// <summary>
        /// Gets Contact Support
        /// </summary>
        /// <param name="assignedTo">The identifier.</param>
        /// <returns></returns>
        [HttpGet("GetContactSupport/{userid}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetContactSupport(Guid? assignedTo)
        {
            var query = new GetContactSupportQuery { AssignedTo = assignedTo };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Contact Support.
        /// </summary>
        /// <param name="addContactSupportCommand">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(ContactSupportDto))]
        public async Task<IActionResult> ContactSupport([FromBody] AddContactSupportCommand addContactSupportCommand)
        {
            var result = await _mediator.Send(addContactSupportCommand);
            //return ReturnFormattedResponse(result);
            return Ok(result.Data.RequestNo);
        }
    }
}
