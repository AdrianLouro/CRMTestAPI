using System;
using Entities.Models;

namespace Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        User FindById(Guid id);

        User FindByEmail(string email);

        User FindWithRolesByEmail(string email);

        User FindWithRolesById(Guid id);
    }
}