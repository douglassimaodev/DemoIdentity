using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace DemoIdentity.Presentation.Helpers
{
    public static class PermissionHelper
    {

        private static IHttpContextAccessor httpContextAccessor;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }
        
        public const string Create = "Create";
        public const string Read = "Read";
        public const string Update = "Update";
        public const string Delete = "Delete";
          
        #region Permissao Especifica

        public const string PodeVerBotaoGlobal = "PodeVerBotaoGlobal";
        public const string PodeVerAdminPage = "PodeVerAdminPage";

        #endregion      

        //public static MvcHtmlString IfClaimHelper(this MvcHtmlString value, string claimName, string claimValue)
        //{
        //    return ValidadePermission(claimName, claimValue) ? value : MvcHtmlString.Empty;
        //}

        //public static MvcHtmlString IfClaimHelper(this MvcHtmlString value, string claimName)
        //{
        //    return ValidadePermission(claimName) ? value : MvcHtmlString.Empty;
        //}

        //public static bool IfClaim(this WebViewPage page, string claimName, string claimValue)
        //{
        //    return ValidadePermission(claimName, claimValue);
        //}

        //public static bool IfClaim(this WebViewPage page, string claimName)
        //{
        //    return ValidadePermission(claimName);
        //}

        public static bool IfClaim(string claimName, string claimValue)
        {
            return ValidadePermission(claimName, claimValue);
        }

        public static bool IfClaim(string claimName)
        {
            return ValidadePermission(claimName);
        }

        private static bool ValidadePermission(string claimName, string claimValue)
        {
            var cn = claimName != null ? claimName.Split(',') : new String[0];

            var identity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
            var claim = identity.Claims.FirstOrDefault(c => cn.Contains(c.Type));
            return claim != null && claim.Value.Contains(claimValue);
        }

        public static ClaimsIdentity GetIdentity()
        {
            return (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
        }

        public static string GetClaim(string claimName)
        {
            var identity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
            var claim = identity.Claims.Where(x => x.Type == claimName).FirstOrDefault();
            return claim != null ? claim.Value : "";
        }

        private static bool ValidadePermission(string claimName)
        {
            var cn = claimName != null ? claimName.Split(',') : new String[0];
            var identity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
            var claim = identity.Claims.FirstOrDefault(c => cn.Contains(c.Type));
            return claim != null;
        }

    }
}