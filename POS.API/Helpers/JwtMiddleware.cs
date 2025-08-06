using POS.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.API.Helpers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JwtMiddleware'
    public class JwtMiddleware
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JwtMiddleware'
    {
        private readonly RequestDelegate _next;
        private JwtSettings _settings = null;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JwtMiddleware.JwtMiddleware(RequestDelegate, JwtSettings)'
        public JwtMiddleware(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JwtMiddleware.JwtMiddleware(RequestDelegate, JwtSettings)'
            RequestDelegate next,
             JwtSettings settings)
        {
            _next = next;
            _settings = settings;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'JwtMiddleware.Invoke(HttpContext)'
        public async Task Invoke(HttpContext context)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'JwtMiddleware.Invoke(HttpContext)'
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_settings.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "sub").Value;
                // attach user to context on successful jwt validation
                //context.Items["User"] = userId;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }

}
