using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventAgency.Web.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected bool isUserAuthenticated()
        {
            bool retRes = false;
            if (this.User.Identity != null)
            {
                retRes = this.User.Identity.IsAuthenticated;
            }

            return retRes;
        }

        protected string? GetUserId()
        {
            string? userId = null;
            if (this.isUserAuthenticated())
            {
                userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return userId;
        }
    }
}
