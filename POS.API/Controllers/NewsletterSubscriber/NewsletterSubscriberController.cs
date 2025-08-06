using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace POS.API.Controllers.NewsletterSubscriber
{
    [Route("api/[controller]")]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'NewsletterSubscriberController'
    public class NewsletterSubscriberController : ControllerBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'NewsletterSubscriberController'
    {
        private readonly IMediator _mediator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'NewsletterSubscriberController.NewsletterSubscriberController(IMediator)'
        public NewsletterSubscriberController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'NewsletterSubscriberController.NewsletterSubscriberController(IMediator)'
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates the newsletter subscriber.
        /// </summary>
        /// <param name="addNewsletterSubscriberCommand">The add newsletter subscriber command.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewsletterSubscriber([FromBody] AddNewsletterSubscriberCommand addNewsletterSubscriberCommand)
        {
            await _mediator.Send(addNewsletterSubscriberCommand);
            return Ok();
        }
    }
}
