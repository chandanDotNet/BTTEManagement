using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using POS.API.Helpers;

namespace POS.API.Controllers.LoginAudit
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LoginAuditController'
    public class LoginAuditController : ControllerBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LoginAuditController'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LoginAuditController._mediator'
        public IMediator _mediator { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LoginAuditController._mediator'
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'LoginAuditController.LoginAuditController(IMediator)'
        public LoginAuditController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'LoginAuditController.LoginAuditController(IMediator)'
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Get All Login Audit detail
        /// </summary>
        /// <param name="loginAuditResource"></param>
        /// <returns></returns>
        [HttpGet]
        [ClaimCheck("LOGS_VIEW_LOGIN_AUDITS")]
        [Produces("application/json", "application/xml", Type = typeof(LoginAuditList))]
        public async Task<IActionResult> GetLoginAudit([FromQuery] LoginAuditResource loginAuditResource)
        {
            var getAllLoginAuditQuery = new GetAllLoginAuditQuery
            {
                LoginAuditResource = loginAuditResource
            };
            var result = await _mediator.Send(getAllLoginAuditQuery);

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
    }
}
