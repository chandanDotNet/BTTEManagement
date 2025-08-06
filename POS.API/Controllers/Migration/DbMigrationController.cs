using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace POS.API.Controllers.Migration
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'DbMigrationController'
    public class DbMigrationController : ControllerBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'DbMigrationController'
    {
        private readonly IMediator _mediator;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'DbMigrationController.DbMigrationController(IMediator)'
        public DbMigrationController(IMediator mediator)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'DbMigrationController.DbMigrationController(IMediator)'
        {
            _mediator = mediator;
        }
    }
}
