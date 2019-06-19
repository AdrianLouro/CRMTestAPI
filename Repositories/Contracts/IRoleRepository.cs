using System;
using Entities.Models;

namespace Repositories.Contracts
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        Role FindById(Guid id);

        Role FindByUserIdAndType(Guid userId, string type);
    }
}