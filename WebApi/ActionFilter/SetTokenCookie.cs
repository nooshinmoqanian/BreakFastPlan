using Azure.Core;
using BusinessLogic.Validators;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;

namespace WebApi.ActionFilter
{
    public class SetTokenCookie : Attribute, IActionFilter
    { 
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var jwtSettings = context.HttpContext.RequestServices.GetRequiredService<IOptions<JwtSettings>>().Value;

            if (context.HttpContext.Items.TryGetValue("RefreshToken", out var value))
            {
                string cookieValue = value.ToString();

                context.HttpContext.Response.Cookies.Append(
                        "refreshToken",
                        cookieValue,
                        new CookieOptions
                        {
                            Path = "/",
                            HttpOnly = true,
                            Secure = context.HttpContext.Request.IsHttps,
                            Expires = DateTimeOffset.UtcNow.AddDays(1)
                        });
            }

            ///Authentication
            
            if (context.HttpContext.Items.TryGetValue("Authorization", out var Auth))
            {
                string cookieValue = Auth.ToString();

                string bearerToken = $"Bearer{" "}{ cookieValue }";

                context.HttpContext.Response.Cookies.Append(
                        "Authorization",
                        bearerToken,
                        new CookieOptions
                        {
                            Path = "/",
                            HttpOnly = true,
                            Secure = context.HttpContext.Request.IsHttps,
                            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes)
                        });

                context.HttpContext.Response.Headers.Add("Authorization", $"Bearer {cookieValue}");
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

           

        }
    }
}
