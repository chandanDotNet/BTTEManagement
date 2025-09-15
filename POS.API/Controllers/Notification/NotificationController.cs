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
using POS.MediatR.CommandAndQuery;
using POS.Data.Dto;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text;
using Azure;
using BTTEM.Data.Entities;

namespace BTTEM.API.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private IMediator _mediator;
        private readonly UserInfoToken _userInfoToken;
        public NotificationController(IMediator mediator, UserInfoToken userInfoToken)
        {
            _mediator = mediator;
            _userInfoToken = userInfoToken;
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

        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        [HttpPut("notification/MarkAllAsRead")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> AllNotificationMarkAsRead()
        {
            var allNotificationsMarkAsReadCommand = new AllNotificationsMarkAsReadCommand()
            {
                UserId = Guid.Parse(_userInfoToken.Id)
            };
            var result = await _mediator.Send(allNotificationsMarkAsReadCommand);
            if (result.Data == true)
            {
                ResponseData response = new ResponseData()
                {
                    status = result.Data,
                    StatusCode = result.StatusCode,
                    message = "All notifications have been read."
                };
                return Ok(response);
            }
            else
            {
                ResponseData response = new ResponseData()
                {
                    status = result.Data,
                    StatusCode = 400,
                    message = "You're all caught up! No unread notifications left."
                };
                return BadRequest(response);
            }           
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
