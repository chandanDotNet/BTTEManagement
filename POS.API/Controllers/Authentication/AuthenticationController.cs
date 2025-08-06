using Azure;
using BTTEM.API.Models;
using BTTEM.API.Service;
using BTTEM.Data.Entities;
using BTTEM.MediatR.CommandAndQuery;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;

namespace POS.API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        public IMediator _mediator;
        private IConfiguration _configuration;

        private readonly INotificationService _notificationService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IMediator mediator, IConfiguration configuration, INotificationService notificationService, IWebHostEnvironment webHostEnvironment, ILogger<AuthenticationController> logger)
        {
            _mediator = mediator;
            _configuration = configuration;
            _notificationService = notificationService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
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

        [AllowAnonymous]
        [HttpPost("SendPushNotification")]
        public async Task<IActionResult> SendPushNotification([FromBody] MessageRequest request)
        {
            try
            {
                _logger.LogInformation("SendPushNotification called with DeviceToken: {DeviceToken}", request?.DeviceToken);

                if (_notificationService == null)
                {
                    _logger.LogError("NotificationService is null.");
                    return StatusCode(500, "Internal server error: NotificationService is null.");
                }

                if (request == null)
                {
                    _logger.LogWarning("Request is null.");
                    return BadRequest("Request is null.");
                }

                if (string.IsNullOrEmpty(request.DeviceToken))
                {
                    _logger.LogWarning("DeviceToken is missing.");
                    return BadRequest("DeviceToken is missing.");
                }


                //NotificationModel message = null;
                ResponseModel resultNotification = new ResponseModel();
                if (!string.IsNullOrEmpty(request.DeviceToken) && request.DeviceType == false)
                {
                    var message = new NotificationModel()
                    {
                        Message = new BTTEM.API.Models.Message
                        {
                            Token = request.DeviceToken,

                            Notification = new BTTEM.API.Models.Notification()
                            {
                                Title = request.Title,
                                Body = request.Body
                            },

                            Data = new BTTEM.API.Models.Data
                            {
                                NotificationTitle = request.Title,
                                NotificationBody = request.Body,
                                UserId = request.UserId,
                                Screen = "Profile",
                                CustomKey = request.CustomKey,
                                Id = request.Id
                            },

                            Android = new Android()
                            {
                                Priority = "high"
                            }
                        }
                    };

                    if (message == null)
                    {
                        return BadRequest("Invalid push notification request. DeviceToken is missing or invalid.");
                    }

                    resultNotification = await _notificationService.SendNotification(message);
                }
                else if (!string.IsNullOrEmpty(request.DeviceToken) && request.DeviceType == true)
                {
                    var message = new NotificationModel()
                    {
                        Message = new BTTEM.API.Models.Message
                        {
                            Token = request.DeviceToken,

                            Data = new BTTEM.API.Models.Data
                            {
                                NotificationTitle = request.Title,
                                NotificationBody = request.Body,
                                UserId = request.UserId,
                                Screen = "Profile",
                                CustomKey = request.CustomKey,
                                Id = request.Id
                            },

                            Android = new Android()
                            {
                                Priority = "high"
                            }
                        }
                    };

                    if (message?.Message == null)
                    {
                        _logger.LogError("Message object is null.");
                        return BadRequest("Message object is null.");
                    }


                    if (message == null)
                    {
                        return BadRequest("Invalid push notification request. DeviceToken is missing or invalid.");
                    }

                    if (_notificationService == null)
                    {
                        return StatusCode(500, "Internal server error: NotificationService is null.");
                    }

                    resultNotification = await _notificationService.SendNotification(message);


                    if (resultNotification == null)
                    {
                        _logger.LogError("Notification service returned null.");
                        return StatusCode(500, "Notification service returned null.");
                    }

                    _logger.LogInformation("Notification sent. Success: {Success}", resultNotification.IsSuccess);

                    return resultNotification.IsSuccess ? Ok(resultNotification) : BadRequest(resultNotification);

                }

                if (resultNotification.IsSuccess)
                {
                    return Ok(resultNotification);
                }
                else
                {
                    return BadRequest(resultNotification);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in SendPushNotification.");
                return StatusCode(500, $"Unhandled exception: {ex.Message}");
            }
        }


        //[AllowAnonymous]
        //[HttpPost("PushNotify")]
        //public async Task<IActionResult> SendNotify()
        //{
        //    string serviceAccountPath = Path.Combine(_webHostEnvironment.ContentRootPath, "sft-travel-desk-firebase-adminsdk-fbsvc-8a20b9aba1.json");
        //    string deviceToken = "fNy1DQ8sR9av9h3UzzDRLR:APA91bETQAERz6Ta0cUcPsAiYrClPEuhOz-YbWFl5uCfZ_p1KEhRqrJPYUqzvbPvoPWqP1dmBZ8tcJ5zEZDpThPaW_uUQlDgKwWKqW3vcKjsovcaOEr_pv8";
        //    string projectId = "sft-travel-desk";

        //    var credential = GoogleCredential.FromFile(serviceAccountPath)
        //    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

        //    var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

        //    var message = new
        //    {
        //        message = new
        //        {
        //            token = deviceToken,
        //            notification = new
        //            {
        //                title = "Hello from .NET!",
        //                body = "This is a push notification using FCM HTTP v1"
        //            },
        //            android = new
        //            {
        //                priority = "high"
        //            }
        //        }
        //    };

        //    string jsonMessage = JsonConvert.SerializeObject(message);

        //    using var httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    var content = new StringContent(jsonMessage, System.Text.Encoding.UTF8, "application/json");

        //    var response = await httpClient.PostAsync(
        //    $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send",
        //    content
        //    );

        //    string result = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine($"Response: {result}");

        //    return new EmptyResult();
        //}
    }
}

