using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using POS.MediatR.CommandAndQuery;
using Microsoft.AspNetCore.Authorization;
using POS.API.Helpers;

namespace POS.API.Controllers.Email
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmailController'
    public class EmailController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmailController'
    {
        IMediator _mediator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'EmailController.EmailController(IMediator)'
        public EmailController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'EmailController.EmailController(IMediator)'
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Send mail.
        /// </summary>
        /// <param name="sendEmailCommand"></param>
        /// <returns></returns>
        [HttpPost(Name = "SendEmail")]
       
#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// [ClaimCheck("EMAIL_SEND_EMAIL")]
        [Produces("application/json", "application/xml", Type = typeof(void))]
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
        public async Task<IActionResult> SendEmail(SendEmailCommand sendEmailCommand)
        {
            var result = await _mediator.Send(sendEmailCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
