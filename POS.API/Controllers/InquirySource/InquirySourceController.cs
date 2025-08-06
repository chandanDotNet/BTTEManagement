using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.API.Helpers;

namespace POS.API.Controllers.InquirySource
{
    [Route("api")]
    [ApiController]
    [Authorize]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceController'
    public class InquirySourceController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceController'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceController._mediator'
        public IMediator _mediator { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceController._mediator'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceController.InquirySourceController(IMediator)'
        public InquirySourceController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'InquirySourceController.InquirySourceController(IMediator)'
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Inquiry Srouce.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("InquirySource/{id}", Name = "GetInquirySource")]
        [ClaimCheck("INQ_MANAGE_INQ_SOURCE")]
        [Produces("application/json", "application/xml", Type = typeof(InquirySourceDto))]
        public async Task<IActionResult> GetInquirySource(Guid id)
        {
            var getInquirySourceQuery = new GetInquirySourceQuery { Id = id };
            var result = await _mediator.Send(getInquirySourceQuery);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Inquiry Srouces.
        /// </summary>
        /// <returns></returns>
        [HttpGet("InquirySources")]
        [ClaimCheck("INQ_MANAGE_INQ_SOURCE")]
        [Produces("application/json", "application/xml", Type = typeof(List<InquirySourceDto>))]
        public async Task<IActionResult> GetInquirySources()
        {
            var getAllInquirySourceQuery = new GetAllInquirySourceQuery { };
            var result = await _mediator.Send(getAllInquirySourceQuery);
            return Ok(result);
        }

        /// <summary>
        /// Create Inquiry Srouce.
        /// </summary>
        /// <param name="addInquirySourceCommand"></param>
        /// <returns></returns>
        [HttpPost("InquirySource")]
        [ClaimCheck("INQ_MANAGE_INQ_SOURCE")]
        [Produces("application/json", "application/xml", Type = typeof(InquirySourceDto))]
        public async Task<IActionResult> AddInquirySource(AddInquirySourceCommand addInquirySourceCommand)
        {
            var response = await _mediator.Send(addInquirySourceCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetInquirySource", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Update Inquiry Source.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateInquirySourceCommand"></param>
        /// <returns></returns>
        [HttpPut("InquirySource/{Id}")]
        [ClaimCheck("INQ_MANAGE_INQ_SOURCE")]
        [Produces("application/json", "application/xml", Type = typeof(InquirySourceDto))]
        public async Task<IActionResult> UpdateInquirySource(Guid Id, UpdateInquirySourceCommand updateInquirySourceCommand)
        {
            updateInquirySourceCommand.Id = Id;
            var result = await _mediator.Send(updateInquirySourceCommand);
            return ReturnFormattedResponse(result);

        }

        /// <summary>
        /// Delete Inquiry Srouce.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("InquirySource/{Id}")]
        [ClaimCheck("INQ_MANAGE_INQ_SOURCE")]
        public async Task<IActionResult> DeleteInquirySource(Guid Id)
        {
            var deleteInquirySourceCommand = new DeleteInquirySourceCommand { Id = Id };
            var result = await _mediator.Send(deleteInquirySourceCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
