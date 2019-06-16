using Contracts;
using Entities;

namespace Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private AppDbContext _dbContext;
        private IUserRepository _user;

        public RepositoryWrapper(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserRepository User => _user ?? (_user = new UserRepository(_dbContext));
    }
}