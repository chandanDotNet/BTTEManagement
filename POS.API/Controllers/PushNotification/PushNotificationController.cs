using BTTEM.API.Models;
using BTTEM.API.Service;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace BTTEM.API.Controllers.PushNotification
{
    public class PushNotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public PushNotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("PushNotification")]
        public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
        {
            var result = await _notificationService.SendNotification(notificationModel);
            return Ok(result);
        }

        [HttpGet(".well-known/apple-app-site-association")]
        public async Task<IActionResult> AppLinkFile()
        {
            var myfile = System.IO.File.ReadAllBytes("wwwroot/.well-known/apple-app-site-association.json");
            return new FileContentResult(myfile, "application/json");
        }
    }
}
