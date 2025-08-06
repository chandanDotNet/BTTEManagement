using POS.Data.Dto;
using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Handlers;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POS.API.Helpers;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ReminderController'
    public class ReminderController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ReminderController'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ReminderController._mediator'
        public IMediator _mediator { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ReminderController._mediator'

        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public ReminderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the reminders.
        /// </summary>
        /// <param name="reminderResource">The reminder resource.</param>
        /// <returns></returns>
        [HttpGet("GetReminders")]
        //[ClaimCheck("REM_VIEW_REMINDERS")]
        [Produces("application/json", "application/xml", Type = typeof(ReminderList))]
        public async Task<IActionResult> GetReminders([FromQuery] ReminderResource reminderResource)
        {
            var getAllReminderQuery = new GetAllReminderQuery
            {
                ReminderResource = reminderResource
            };

            var result = await _mediator.Send(getAllReminderQuery);

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
        /// Creates the reminder.
        /// </summary>
        /// <param name="addReminderCommand">The add reminder command.</param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("REM_ADD_REMINDER")]
        [Produces("application/json", "application/xml", Type = typeof(ReminderDto))]
        public async Task<IActionResult> CreateReminder(AddReminderCommand addReminderCommand)
        {
            var result = await _mediator.Send(addReminderCommand);
            return Ok(result);
        }

        /// <summary>
        /// Gets the reminder.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ClaimCheck("REM_VIEW_REMINDERS")]
        [Produces("application/json", "application/xml", Type = typeof(ReminderList))]
        public async Task<IActionResult> GetReminder(Guid id)
        {
            var getReminderByIdQuery = new GetReminderByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(getReminderByIdQuery);

            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Updates the reminder.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="updateReminderCommand">The update reminder command.</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        [ClaimCheck("REM_ADD_REMINDER")]
        [Produces("application/json", "application/xml", Type = typeof(ReminderDto))]
        public async Task<IActionResult> UpdateReminder(Guid id, UpdateReminderCommand updateReminderCommand)
        {
            var result = await _mediator.Send(updateReminderCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Deletes the reminder.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("REM_DELETE_REMINDER")]
        [Produces("application/json", "application/xml", Type = typeof(bool))]
        public async Task<IActionResult> DeleteReminder(Guid id)
        {
            var deleteReminderCommaond = new DeleteReminderCommand { Id = id };
            var result = await _mediator.Send(deleteReminderCommaond);
            return ReturnFormattedResponse(result);
        }

        
#pragma warning disable CS1572 // XML comment has a param tag for 'id', but there is no parameter by that name
/// <summary>
        /// Gets the reminder.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("notofication/top10")]
#pragma warning restore CS1572 // XML comment has a param tag for 'id', but there is no parameter by that name
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetTop10ReminderNotification()
        {
            var getTop10ReminderNotificationQuery = new GetTop10ReminderNotificationQuery
            {
            };

            var result = await _mediator.Send(getTop10ReminderNotificationQuery);
            return Ok(result);
        }
        
#pragma warning disable CS1572 // XML comment has a param tag for 'id', but there is no parameter by that name
/// <summary>
        /// Gets the reminder.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("notifications")]
#pragma warning restore CS1572 // XML comment has a param tag for 'id', but there is no parameter by that name
        [Produces("application/json", "application/xml")]
#pragma warning disable CS1573 // Parameter 'reminderResource' has no matching param tag in the XML comment for 'ReminderController.GetNotifications(ReminderResource)' (but other parameters do)
        public async Task<IActionResult> GetNotifications([FromQuery] ReminderResource reminderResource)
#pragma warning restore CS1573 // Parameter 'reminderResource' has no matching param tag in the XML comment for 'ReminderController.GetNotifications(ReminderResource)' (but other parameters do)
        {
            var getAllReminderNotificationQuery = new GetAllReminderNotificationQuery
            {
                ReminderResource= reminderResource
            };

            var result = await _mediator.Send(getAllReminderNotificationQuery);

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
        //[HttpGet("notification/markasread")]
        //[Produces("application/json", "application/xml")]
        //public async Task<IActionResult> GetNotificationMarkasRead()
        //{
        //    var markAsReadAllNotificationsCommand = new MarkAsReadAllNotificationsCommand
        //    {
        //    };

        //    var result = await _mediator.Send(markAsReadAllNotificationsCommand);

        //    return Ok(result);
        //}
    }
}
