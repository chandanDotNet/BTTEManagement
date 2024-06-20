using BTTEM.Data.Dto;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using System.Threading.Tasks;
using POS.API.Helpers;
using System;
using System.Linq;
using BTTEM.MediatR.Notification.Command;

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
        public async Task<IActionResult> Notification([FromBody] AddNotificationCommand addNotificationCommand)
        {
            var result = await _mediator.Send(addNotificationCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Read Notification.
        /// </summary>
        /// <param name="readNotificationCommand">The identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [Produces("application/json", "application/xml", Type = typeof(NotificationDto))]
        public async Task<IActionResult> ReadNotification([FromBody] ReadNotificationCommand readNotificationCommand)
        {
            var result = await _mediator.Send(readNotificationCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Gets Notification Count
        /// </summary>
        /// <param name="userId">The identifier.</param>
        /// <returns></returns>
        [HttpGet("GetNotificationCount/{userId}")]
        //[ClaimCheck("EXP_VIEW_EXPENSES")]
        public async Task<IActionResult> GetNotificationCount(Guid? userId)
        {
            var query = new GetNotificationQuery { UserId = userId };
            var result = await _mediator.Send(query);
            NotificationResponse response = new NotificationResponse()
            {
                TotalCount = result.Count(),
                ReadCount = result.Where(x => x.Read != 0).Count(),
                UnreadCount = result.Where(x => x.Read == 0).Count(),
            };
            return Ok(response);
        }
        public class NotificationResponse
        {
            public int TotalCount { get; set; }
            public int ReadCount { get; set; }
            public int UnreadCount { get; set; }
        }
    }
}
