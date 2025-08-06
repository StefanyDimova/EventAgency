using Microsoft.AspNetCore.Http;
using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Web.Infrastructure.Middlewares
{
    public class AdminRedirectionMiddleware
    {
        private const string IndexPath = "/";
        private const string AdminIndexPath = "/Admin";

        private readonly RequestDelegate next;

        public AdminRedirectionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                string path = context.Request.Path.Value?.ToLower() ?? string.Empty;

                // Ако е в Identity (Login/Register/Account) -> пропускаме middleware-а
                if (!path.StartsWith("/identity") && path == "/")
                {
                    if (context.User.IsInRole(adminRoleName))
                    {
                        context.Response.Redirect("/Admin");
                        return;
                    }
                }
            }

            await this.next(context);
        }
    }
}

