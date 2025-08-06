using BTTEM.API.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime;
using System;
using System.Threading.Tasks;
using static BTTEM.API.Models.GoogleNotification;
using CorePush.Apple;
using CorePush.Google;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace BTTEM.API.Service
{
    public interface INotificationService
    {
        Task<ResponseModel> SendNotification(NotificationModel notificationModel);
    }
    public class NotificationService : INotificationService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IWebHostEnvironment webHostEnvironment, ILogger<NotificationService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<ResponseModel> SendNotification(NotificationModel notificationModel)
        {
            ResponseModel responseModel = null;
            try
            {
                _logger.LogInformation("SendNotification started.");
                HttpClient httpClient = new HttpClient();
                string serviceAccountPath = Path.Combine(_webHostEnvironment.ContentRootPath, "sft-travel-desk-firebase-adminsdk-fbsvc-8a20b9aba1.json");
                string projectId = "sft-travel-desk";

                _logger.LogInformation("Using service account path: {Path}", serviceAccountPath);

                var credential = GoogleCredential.FromFile(serviceAccountPath)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                string jsonMessage = JsonConvert.SerializeObject(notificationModel);

                _logger.LogDebug("Serialized notification message: {Message}", jsonMessage);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var content = new StringContent(jsonMessage, System.Text.Encoding.UTF8, "application/json");

                var result = await httpClient.PostAsync(
                $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send",
                content
                );

                string response = await result.Content.ReadAsStringAsync();


                _logger.LogInformation("FCM response: {Response}", response);


                responseModel = new ResponseModel()
                {
                    IsSuccess = result.IsSuccessStatusCode,
                    Message = result.IsSuccessStatusCode ? "Notification sent successfully!!!" : $"Notification failed: {response}"
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending notification.");

                responseModel = new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Exception occurred: {ex.Message}"
                };
            }


            _logger.LogInformation("SendNotification completed with success: {Success}", responseModel?.IsSuccess);


            return responseModel;
        }

    }
}
