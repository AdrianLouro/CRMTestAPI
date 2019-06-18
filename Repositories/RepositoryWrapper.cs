using Contracts;
using Entities;

namespace Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private AppDbContext _dbContext;
        private IUserRepository _user;
        private IRoleRepository _role;

        public RepositoryWrapper(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserRepository User => _user ?? (_user = new UserRepository(_dbContext));

        public IRoleRepository Role => _role ?? (_role = new RoleRepository(_dbContext));
    }
}