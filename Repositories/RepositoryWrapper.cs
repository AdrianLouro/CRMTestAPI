using Entities;
using Repositories.Contracts;

namespace Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private AppDbContext _dbContext;
        private IUserRepository _user;
        private IRoleRepository _role;
        private ICustomerRepository _customer;

        public RepositoryWrapper(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserRepository User => _user ?? (_user = new UserRepository(_dbContext));

        public IRoleRepository Role => _role ?? (_role = new RoleRepository(_dbContext));

        public ICustomerRepository Customer => _customer ?? (_customer = new CustomerRepository(_dbContext));
    }
}