using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASPNETCoreForBeginners.Authorization
{
    public class AgeAuthorizationHeadler : AuthorizationHandler<AgeGreaterThan25Requirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeGreaterThan25Requirment requirement)
        {
            var dob = DateTime.Parse(context.User.FindFirstValue("DateOfBirth"));

            if (DateTime.Today.Year - dob.Year > 25)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
