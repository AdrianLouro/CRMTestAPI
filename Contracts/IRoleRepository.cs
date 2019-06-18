using System;
using Entities.Models;

namespace Contracts
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        Role FindById(Guid id);

        Role FindByUserIdAndType(Guid userId, string type);
    }
}