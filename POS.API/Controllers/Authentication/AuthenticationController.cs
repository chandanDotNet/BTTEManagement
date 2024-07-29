using Azure;
using BTTEM.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using System.Threading.Tasks;

namespace POS.API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        public IMediator _mediator;
        private IConfiguration _configuration;
        public AuthenticationController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="userLoginCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(UserAuthDto))]
        public async Task<IActionResult> Login(UserLoginCommand userLoginCommand)
        {
            userLoginCommand.RemoteIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _mediator.Send(userLoginCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// User Login For App
        /// </summary>
        /// <param name="userLoginCommand"></param>
        /// <returns></returns>
        [HttpPost("AppLogin")]
        [Produces("application/json", "application/xml", Type = typeof(UserAuthDto))]
        public async Task<IActionResult> AppLogin(UserLoginCommand userLoginCommand)
        {
            userLoginCommand.RemoteIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _mediator.Send(userLoginCommand);
            LoginResponse response = new LoginResponse();
            if (result.Success)
            {
                response.status = result.Success;
                response.StatusCode = result.StatusCode;
                response.message = "Login Success";
                response.Data = result.Data;
            }
            else
            {
                response.status = false;
                response.StatusCode = result.StatusCode;
                response.message = "Login failed. Username/Password incorrect.";
                response.Data = new UserAuthDto();
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Signup()
        {
            var result = this._configuration.GetSection("SignUpCheck")["Signup"];
            ResponseData response = new ResponseData();
            if (result == "True")
            {
                response.status = true;
                response.StatusCode = 200;
                response.message = "Success";
            }
            else
            {
                response.status = false;
                response.StatusCode = 404;
                response.message = "Failed";
            }
            return Ok(response);
        }
    }
}
