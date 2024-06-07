using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System.Threading.Tasks;
using POS.API.Helpers;
using System;

namespace BTTEM.API.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private IMediator _mediator;
        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        ///// <summary>
        ///// Get All Notification
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> GetNotification()
        //{
        //    return Ok();
        //}

        /// <summary>
        /// Gets Notification
        /// </summary>
        /// <param name="userId">The identifier.</param>
        /// <returns></returns>
        [HttpGet("GetNotification/{userId}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetNotification(Guid? userId)
        {
            var query = new GetNotificationQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Add Notification.
        /// </summary>
        /// <param name="addNotificationCommand">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(NotificationDto))]
        public async Task<IActionResult> Notification([FromBody] AddNotificationCommand addNotificationCommand )
        {
            var result = await _mediator.Send(addNotificationCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
