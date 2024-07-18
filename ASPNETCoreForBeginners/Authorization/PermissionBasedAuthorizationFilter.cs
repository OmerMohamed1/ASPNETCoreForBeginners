using ASPNETCoreForBeginners.Data;
using ASPNETCoreForBeginners.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace ASPNETCoreForBeginners.Authorization
{
    public class PermissionBasedAuthorizationFilter(ApplicationDbContext dbContext) : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attribute = (CheckPermissionAttirbute)context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is CheckPermissionAttirbute);
            if (attribute != null)
            {
                var claimIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (claimIdentity == null || !claimIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var userId = int.Parse(claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var hasPermission = dbContext.Set<UserPermission>().Any(x => x.UserId == userId &&
                    x.PermissionId == attribute.Permission);
                    if (!hasPermission)
                    {
                        context.Result = new ForbidResult();
                    }
                }
            }
        }
    }
}
