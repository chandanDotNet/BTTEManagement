using System;
using System.Threading.Tasks;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POS.API.Controllers
{
    /// <summary>
    /// UserClaim
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserClaimController : BaseController
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'UserClaimController._mediator'
        public IMediator _mediator { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'UserClaimController._mediator'
        /// <summary>
        /// UserClaim
        /// </summary>
        /// <param name="mediator"></param>
        public UserClaimController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Update User Claim By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addUserCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json", "application/xml", Type = typeof(UserClaimDto))]
        public async Task<IActionResult> UpdateUserClaim(Guid id, UpdateUserClaimCommand addUserCommand)
        {
            addUserCommand.Id = id;
            var result = await _mediator.Send(addUserCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
