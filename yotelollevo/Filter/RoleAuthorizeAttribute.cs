using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yotelollevo.Filter
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] roles;

        public RoleAuthorizeAttribute(params string[] roles)
        {
            this.roles = roles ?? Array.Empty<string>();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var rol = httpContext.Session?["Rol"] as string;
            if (string.IsNullOrEmpty(rol)) return false;

            if (roles.Length == 0) return true;
            return roles.Contains(rol, StringComparer.OrdinalIgnoreCase);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session?["Rol"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            filterContext.Result = new HttpStatusCodeResult(403);
        }
    }
}
