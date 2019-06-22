using System;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using static System.Security.Claims.ClaimTypes;
using static System.Threading.Tasks.Task;

namespace CRMTestAPI.Security
{
    public class NotDeletedRequirement : IAuthorizationRequirement
    {
        private AppDbContext _dbContext;
        private AuthorizationHandlerContext _authorizationContext;

        public async Task<bool> Pass(AppDbContext dbContext, AuthorizationHandlerContext authorizationContext)
        {
            _dbContext = dbContext;
            _authorizationContext = authorizationContext;

            return await FromResult(UserExists());
        }

        private bool UserExists()
        {
            return UserIdIsSet() && UserExistsInDbAndIsNotDeleted();
        }

        private bool UserIdIsSet()
        {
            return _authorizationContext.User.FindFirst(NameIdentifier) != null;
        }

        private bool UserExistsInDbAndIsNotDeleted()
        {
            return _dbContext.Set<User>().Any(
                user => user.Id.Equals(GetUserId()) && !user.IsDeleted()
            );
        }

        private Guid GetUserId()
        {
            return Guid.Parse(_authorizationContext.User.FindFirst(NameIdentifier).Value);
        }
    }
}