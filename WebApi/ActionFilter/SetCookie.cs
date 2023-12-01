using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.ActionFilter
{
    public class SetCookie : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

    public void OnActionExecuted(ActionExecutedContext context)
    {
            if (context.HttpContext.Items.TryGetValue("CookieValue", out var value))
            {
                string cookieValue = value.ToString();

                context.HttpContext.Response.Cookies.Append(
                        "Auth",
                        cookieValue,
                        new CookieOptions
                        {
                            Path = "/",
                            HttpOnly = true,
                            Secure = context.HttpContext.Request.IsHttps,
                            Expires = DateTimeOffset.UtcNow.AddDays(1)
                        }
                    );
            }

        }
    }
}

