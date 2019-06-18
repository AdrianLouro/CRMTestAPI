using System;
using System.Linq;
using Contracts;
using Entities;
using Entities.Models;

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