using POS.Helper;
using Microsoft.AspNetCore.Mvc;

namespace POS.API.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BaseController'
    public class BaseController : ControllerBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BaseController'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BaseController.ReturnFormattedResponse<T>(ServiceResponse<T>)'
        public IActionResult ReturnFormattedResponse<T>(ServiceResponse<T> response)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BaseController.ReturnFormattedResponse<T>(ServiceResponse<T>)'
        {
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return StatusCode(response.StatusCode, response.Errors);
        }
    }
}