using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.ActionFilter
{
    public class SetTokenCookie : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
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

            if (context.HttpContext.Items.TryGetValue("Authentication", out var Auth))
            {
                string cookieValue = Auth.ToString();

                string bearerToken = $"Bearer { cookieValue }";

                context.HttpContext.Response.Cookies.Append(
                        "Authentication",
                        bearerToken,
                        new CookieOptions
                        {
                            Path = "/",
                            HttpOnly = true,
                            Secure = context.HttpContext.Request.IsHttps,
                            Expires = DateTimeOffset.UtcNow.AddDays(1)
                        });

            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

           

        }
    }
}
