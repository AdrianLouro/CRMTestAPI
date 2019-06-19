using System;
using Entities;
using Entities.Models;
using Tests.Contracts;

namespace Tests
{
    public class RoleSeeder : IEntitySeeder
    {
        private AppDbContext _dbContext;

        public RoleSeeder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            _dbContext.Roles.Add(new Role()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                UserId = _dbContext.Users.Find(Guid.Parse("11111111-1111-1111-1111-111111111111")).Id
            });

            _dbContext.SaveChanges();
        }
    }
}