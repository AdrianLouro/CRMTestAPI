using System;
using Entities.Models;

namespace Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        User FindById(Guid id);

        User FindWithRolesByEmail(string email);
    }
}