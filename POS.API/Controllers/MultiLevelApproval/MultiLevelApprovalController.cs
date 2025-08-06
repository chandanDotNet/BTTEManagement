using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS.API.Controllers;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using System.Threading.Tasks;
using System;
using MediatR;
using BTTEM.MediatR.CommandAndQuery;
using BTTEM.Data.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using BTTEM.MediatR.Command;

namespace BTTEM.API.Controllers.MultiLevelApproval
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalController'
    public class MultiLevelApprovalController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalController'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalController._mediator'
        public IMediator _mediator { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalController._mediator'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalController.MultiLevelApprovalController(IMediator)'
        public MultiLevelApprovalController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MultiLevelApprovalController.MultiLevelApprovalController(IMediator)'
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Multi Level Approval.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("MultiLevelApproval/{id}", Name = "GetMultiLevelApproval")]
        [Produces("application/json", "application/xml", Type = typeof(MultiLevelApprovalDto))]
        public async Task<IActionResult> GetMultiLevelApproval(Guid id)
        {
            var getMultiLevelApprovalQuery = new GetMultiLevelApprovalQuery { Id = id };
            var result = await _mediator.Send(getMultiLevelApprovalQuery);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Multi Level Approvals.
        /// </summary>
        /// <returns></returns>
        [HttpGet("MultiLevelApprovals")]
        [Produces("application/json", "application/xml", Type = typeof(List<MultiLevelApprovalDto>))]
        public async Task<IActionResult> MultiLevelApprovals()
        {
            var getAllMultiLevelApprovalQuery = new GetAllMultiLevelApprovalQuery { };
            var result = await _mediator.Send(getAllMultiLevelApprovalQuery);
            return Ok(result);
        }

        /// <summary>
        /// Create Multi Level Approval.
        /// </summary>
        /// <param name="addMultiLevelApprovalCommand"></param>
        /// <returns></returns>
        [HttpPost("MultiLevelApproval")]
        [Produces("application/json", "application/xml", Type = typeof(MultiLevelApprovalDto))]
        public async Task<IActionResult> AddMultiLevelApproval(AddMultiLevelApprovalCommand addMultiLevelApprovalCommand)
        {
            var response = await _mediator.Send(addMultiLevelApprovalCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return CreatedAtAction("GetMultiLevelApproval", new { id = response.Data.Id }, response.Data);
        }

        /// <summary>
        /// Update Multi evel Approval.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateMultiLevelApprovalCommand"></param>
        /// <returns></returns>
        [HttpPut("MultiLevelApproval/{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(MultiLevelApprovalDto))]
        public async Task<IActionResult> UpdateMultiLevelApproval(Guid Id, UpdateMultiLevelApprovalCommand updateMultiLevelApprovalCommand)
        {
            updateMultiLevelApprovalCommand.Id = Id;
            var result = await _mediator.Send(updateMultiLevelApprovalCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete Multi Level Approval.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("MultiLevelApproval/{Id}")]
        public async Task<IActionResult> DeleteMultiLevelApproval(Guid Id)
        {
            var deleteMultiLevelApprovalCommand = new DeleteMultiLevelApprovalCommand { Id = Id };
            var result = await _mediator.Send(deleteMultiLevelApprovalCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
