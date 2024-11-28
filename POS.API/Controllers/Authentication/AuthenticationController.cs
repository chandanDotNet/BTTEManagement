using Azure;
using BTTEM.Data.Entities;
using BTTEM.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
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

        /// <summary>
        /// User Login For HRMS
        /// </summary>
        /// <param name="userLoginCommand"></param>
        /// <returns></returns>
        [HttpPost("HRMSLogin")]
        [Produces("application/json", "application/xml", Type = typeof(UserAuthDto))]
        public async Task<IActionResult> HRMSLogin(HRMSUserLoginCommand userLoginCommand)
        {
            // userLoginCommand.RemoteIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //string Ukey = Guid.NewGuid().ToString().GetHashCode().ToString("X");
            //var plainTextBytes = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Ukey));
           
            var result = await _mediator.Send(userLoginCommand);
            HRMSLoginResponse response = new HRMSLoginResponse();
            if (result.Success)
            {
                response.status = result.Success;
                response.StatusCode = result.StatusCode;
                response.message = "Login Success";
                response.accesskey = result.Data.Accesskey;
            }
            else
            {
                response.status = false;
                response.StatusCode = result.StatusCode;
                response.message = "Login failed. Username/Password incorrect.";
               // response.Data = new UserAuthDto();
                
                return StatusCode(401, response);
            }
            return Ok(response);
        }


        /// <summary>
        /// User Login For HRMS Verify 
        /// </summary>
        /// <param name="userLoginCommand"></param>
        /// <returns></returns>
        [HttpGet("HRMSLoginVerify")]
        [Produces("application/json", "application/xml", Type = typeof(UserAuthDto))]
        public async Task<IActionResult> HRMSLoginVerify(string Accesskey)
        {
            HRMSUserLoginCommand userLoginCommand = new HRMSUserLoginCommand();
            // userLoginCommand.RemoteIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //string Ukey = Guid.NewGuid().ToString().GetHashCode().ToString("X");
            //var plainTextBytes = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Ukey));
            var base64EncodedBytes = System.Convert.FromBase64String(Accesskey);
            string accesskey = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            userLoginCommand.Accesskey = accesskey;
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
                // response.Data = new UserAuthDto();

                return StatusCode(401, response);
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetSignupButtonStatus")]
        public async Task<IActionResult> SignupStatus()
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
