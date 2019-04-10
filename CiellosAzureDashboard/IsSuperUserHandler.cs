using CiellosAzureDashboard.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard
{
    public class IsSuperUserHandler : AuthorizationHandler<SuperUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       SuperUserRequirement requirement)
        {
            CADContext dbContext = new CADContext();
            var user = dbContext.Users.FirstOrDefault(U=> U.UserName == context.User.Identity.Name);
            if(user != null)
            {
                if (user.IsSuperUser)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}