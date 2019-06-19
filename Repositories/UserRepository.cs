using System;
using System.Linq;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public User FindById(Guid id)
        {
            return FindByCondition(user => user.Id.Equals(id)).FirstOrDefault();
        }

        public User FindByEmail(string email)
        {
            return FindByCondition(user => user.Email.Equals(email)).AsNoTracking().FirstOrDefault();
        }

        public User FindWithRolesByEmail(string email)
        {
            return FindByCondition(user => user.Email.Equals(email)).AsNoTracking()
                .Include(user => user.Roles)
                .FirstOrDefault();
        }

        public User FindWithRolesById(Guid id)
        {
            return FindByCondition(user => user.Id.Equals(id)).AsNoTracking()
                .Include(user => user.Roles)
                .FirstOrDefault();
        }
    }
}