using System;
using System.Linq;
using Entities;
using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Role FindById(Guid id)
        {
            return FindByCondition(role => role.Id.Equals(id)).FirstOrDefault();
        }

        public Role FindByUserIdAndType(Guid userId, string type)
        {
            return FindByCondition(role => role.UserId.Equals(userId) && role.Type.ToLower().Equals(type.ToLower()))
                .FirstOrDefault();
        }
    }
}