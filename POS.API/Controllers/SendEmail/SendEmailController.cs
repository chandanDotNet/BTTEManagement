using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Controllers
{
    /// <summary>
    /// Supplier Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SendEmailController : BaseController
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SendEmailController._mediator'
        public readonly IMediator _mediator;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SendEmailController._mediator'
        private readonly ILogger<SendEmailController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public SendEmailController(
            IMediator mediator,
#pragma warning disable CS1573 // Parameter 'logger' has no matching param tag in the XML comment for 'SendEmailController.SendEmailController(IMediator, ILogger<SendEmailController>)' (but other parameters do)
             ILogger<SendEmailController> logger
#pragma warning restore CS1573 // Parameter 'logger' has no matching param tag in the XML comment for 'SendEmailController.SendEmailController(IMediator, ILogger<SendEmailController>)' (but other parameters do)

              )
        {
            _mediator = mediator;
            _logger = logger;

        }
        
#pragma warning disable CS1572 // XML comment has a param tag for 'supplierResource', but there is no parameter by that name
/// <summary>
        /// Get All Suppliers
        /// </summary>
        /// <param name="supplierResource"></param>
        /// <returns></returns>

        [HttpPost("suppliers")]
#pragma warning restore CS1572 // XML comment has a param tag for 'supplierResource', but there is no parameter by that name
#pragma warning disable CS1573 // Parameter 'addSendEmailSuppliersCommand' has no matching param tag in the XML comment for 'SendEmailController.SendEmailSuppliers(AddSendEmailSuppliersCommand)' (but other parameters do)
        public async Task<IActionResult> SendEmailSuppliers([FromBody] AddSendEmailSuppliersCommand addSendEmailSuppliersCommand)
#pragma warning restore CS1573 // Parameter 'addSendEmailSuppliersCommand' has no matching param tag in the XML comment for 'SendEmailController.SendEmailSuppliers(AddSendEmailSuppliersCommand)' (but other parameters do)
        {
            var result = await _mediator.Send(addSendEmailSuppliersCommand);
            return Ok(result);
        }
    }
}
