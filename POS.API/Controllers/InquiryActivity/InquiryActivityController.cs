using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.API.Helpers;

namespace POS.API.Controllers.InquiryActivity
{
    [Route("api/[controller]")]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquiryActivityController'
    public class InquiryActivityController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquiryActivityController'
    {
        private readonly IMediator _mediator;
        
#pragma warning disable CS1574 // XML comment has cref attribute 'InquiryNoteController' that could not be resolved
/// <summary>
        /// Initializes a new instance of the <see cref="InquiryNoteController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public InquiryActivityController(IMediator mediator)
#pragma warning restore CS1574 // XML comment has cref attribute 'InquiryNoteController' that could not be resolved
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets the Inquiry Activities .
        /// </summary>
        /// <returns></returns>
        [HttpGet("{inquiryId}")]
        [ClaimCheck("INQ_VIEW_INQUIRIES")]
        [Produces("application/json", "application/xml", Type = typeof(List<InquiryActivityDto>))]
        public async Task<IActionResult> GetInquiryActivities(Guid inquiryId)
        {
            var query = new GetInquiryActivitiesQuery { InquiryId = inquiryId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        /// <summary>
        /// Inserts the Inquiry Activity
        /// </summary>
        /// <param name="addInquiryActivityCommand">The add Inquiry Note command.</param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("INQ_ADD_INQUIRY")]
        [Produces("application/json", "application/xml", Type = typeof(InquiryActivityDto))]
        public async Task<IActionResult> AddInquiryActivity([FromBody] AddInquiryActivityCommand addInquiryActivityCommand)
        {
            var result = await _mediator.Send(addInquiryActivityCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Updates the Inquiry Activity By Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="updateInquiryActivityCommand">The update Inquiry By Id command.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("INQ_ADD_INQUIRY")]
        [Produces("application/json", "application/xml", Type = typeof(InquiryActivityDto))]
        public async Task<IActionResult> UpdateInquiryActivity(Guid id, [FromBody] UpdateInquiryActivityCommand updateInquiryActivityCommand)
        {
            updateInquiryActivityCommand.Id = id;
            var result = await _mediator.Send(updateInquiryActivityCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Deletes the Inquiry Activity By Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("INQ_DELETE_INQUIRY")]
        public async Task<IActionResult> DeleteInquiryActivity(Guid id)
        {
            var command = new DeleteInquiryActivityCommand() { Id = id };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }
    }
}
