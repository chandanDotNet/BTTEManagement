using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ClaimCheckAttribute'
    public class ClaimCheckAttribute : Attribute, IActionFilter
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ClaimCheckAttribute'
    {
        private readonly string _claimName;
        private StringValues auth;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ClaimCheckAttribute.ClaimCheckAttribute(string)'
        public ClaimCheckAttribute(string claimName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ClaimCheckAttribute.ClaimCheckAttribute(string)'
        {
            _claimName = claimName;

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ClaimCheckAttribute.OnActionExecuted(ActionExecutedContext)'
        public void OnActionExecuted(ActionExecutedContext context)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ClaimCheckAttribute.OnActionExecuted(ActionExecutedContext)'
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ClaimCheckAttribute.OnActionExecuting(ActionExecutingContext)'
        public void OnActionExecuting(ActionExecutingContext context)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ClaimCheckAttribute.OnActionExecuting(ActionExecutingContext)'
        {
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out auth);

            var tokenValue = auth[0].Replace("Bearer ", "");
            string[] claimArray = new string[] { };
            JwtSecurityToken token;
            Claim claim = null;

            if (_claimName.Contains(","))
            {
                claimArray = _claimName.Split(",");
            }
            token = new JwtSecurityTokenHandler().ReadJwtToken(tokenValue);
            if (claimArray.Count() > 0)
            {
                for (int i = 0; i < claimArray.Length; i++)
                {
                    claim = token.Claims.Where(c => c.Type.Trim() == claimArray[i].Trim() && c.Value == "true").FirstOrDefault();

                    if (claim != null)
                    {
                        break;
                    }
                }
            }
            else
            {
                claim = token.Claims.Where(c => c.Type == _claimName && c.Value == "true").FirstOrDefault();
            }

            if (claim == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);

            }
        }
    }
}