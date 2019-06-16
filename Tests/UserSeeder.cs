using System;
using System.Collections.Generic;
using Contracts;
using Entities;
using Entities.Models;

namespace Tests
{
    public class UserSeeder : IEntitySeeder
    {
        private AppDbContext _dbContext;

        public UserSeeder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            User admin = new User()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "John",
                Surname = "Admin Doe"
            };

            User nonAdmin = new User()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "John",
                Surname = "Doe"
            };
            
            _dbContext.Users.Add(admin);
            _dbContext.Users.Add(nonAdmin);
            _dbContext.SaveChanges();
        }
    }
}