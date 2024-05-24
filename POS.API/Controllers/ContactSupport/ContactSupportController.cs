using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
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

        /// <summary>
        /// Get All Contact Support
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetContactSupport()
        {
            return Ok();
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
