using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Controllers.FileUpload
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        /// <summary>
        /// Posts the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Post(IFormFileCollection files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }

        /// <summary>
        /// Download Android APK File.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("APKAdnroid")]
        public IActionResult DownloadAndroid()
        {
            return Redirect("https://play.google.com/store/apps/details?id=com.travel.sfdesk");
        }

        /// <summary>
        /// Download iOS APK File.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("APKiOS")]
        public IActionResult DownloadiOS()
        {
            return Redirect("https://apps.apple.com/in/app/sftravel-desk/id6593685111");
        }
    }
}
