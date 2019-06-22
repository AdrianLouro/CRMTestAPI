using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace CRMTestAPI.Security
{
    public class NotDeletedAuthorizationHandler : AuthorizationHandler<NotDeletedRequirement>
    {
        private readonly AppDbContext _dbContext;

        public NotDeletedAuthorizationHandler(AppDbContext context)
        {
            _dbContext = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            NotDeletedRequirement requirement)
        {
            if (await requirement.Pass(_dbContext, context))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}