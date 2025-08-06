using POS.Data.Resources;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace POS.API.Controllers.ContactUs
{
    [Route("api/[controller]")]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ContactUsController'
    public class ContactUsController : BaseController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ContactUsController'
    {
        private readonly IMediator _mediator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ContactUsController.ContactUsController(IMediator)'
        public ContactUsController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ContactUsController.ContactUsController(IMediator)'
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates the specified add contact us.
        /// </summary>
        /// <param name="addContactUsCommand">The add contact us command.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddContactUsCommand addContactUsCommand)
        {
            var result = await _mediator.Send(addContactUsCommand);
            return ReturnFormattedResponse(result);
        }

        [HttpGet]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ContactUsController.GetContactUsList(ContactUsResource)'
        public async Task<IActionResult> GetContactUsList([FromQuery] ContactUsResource contactUsResource)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ContactUsController.GetContactUsList(ContactUsResource)'
        {

            var getAllContactUsQuery = new GetAllContactUsQuery
            {
                ContactUsResource = contactUsResource
            };
            var result = await _mediator.Send(getAllContactUsQuery);

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

        [HttpDelete("{id}")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ContactUsController.Delete(Guid)'
        public async Task<IActionResult> Delete(Guid id)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ContactUsController.Delete(Guid)'
        {
            var deleteContactUsCommand = new DeleteContactUsCommand
            {
                Id = id
            };
            var result = await _mediator.Send(deleteContactUsCommand);
            return ReturnFormattedResponse(result);
        }

    }
}
