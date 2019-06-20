using System;
using System.Linq;
using Entities.Models;

namespace Repositories.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        IQueryable<User> FindAllNotDeletedWithRoles();

        User FindById(Guid id);

        User FindByEmail(string email);

        User FindWithRolesByEmail(string email);

        User FindWithRolesById(Guid id);
    }
}