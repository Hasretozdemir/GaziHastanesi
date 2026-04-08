using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GaziHastane.Security
{
    public class AdminPagePermissionFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var area = context.RouteData.Values["area"]?.ToString();
            if (!string.Equals(area, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var user = context.HttpContext.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            if (!AdminPanelPermissions.CanAccessController(user, controller, action))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", new { area = "Admin" });
            }
        }
    }
}
