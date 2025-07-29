using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static EventAgency.GCommon.ApplicationConstants;

namespace EventAgency.Web.Areas.Admin.Controllers
{
    [Area(adminAreaName)]
    [Authorize(Roles = adminRoleName)]
    public abstract class BaseAdminController : Controller
    {
        private bool isUserAuthenticated()
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
