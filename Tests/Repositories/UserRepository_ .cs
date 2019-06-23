using System;
using System.Collections.Generic;
using System.Linq;
using CRMTestAPI;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories;
using Xunit;

namespace Tests.Repositories
{
    public class UserRepository_ : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private IQueryable<User> _users;
        private UserRepository _userRepository;

        public UserRepository_()
        {
            SetUpUsers();
            SetUpUserRepository();
        }

        [Fact]
        public void fetches_all_users()
        {
            Assert.IsType<User>(_userRepository.FindById(Guid.Parse("11111111-1111-1111-1111-111111111111")));
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), _userRepository.FindAll().ElementAt(0).Id);
        }

        [Fact]
        public void does_not_fetch_user_by_not_existing_email()
        {
            var user = _userRepository.FindByEmail("fake@mail.es");
            Assert.Null(user);
        }

        [Fact]
        public void fetches_user_by_existing_email()
        {
            var user = _userRepository.FindByEmail("admin@admin.es");
            Assert.IsType<User>(user);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), user.Id);

        }

        [Fact]
        public void does_not_fetch_user_with_roles_by_not_existing_email()
        {
            var user = _userRepository.FindWithRolesByEmail("fake@mail.es");
            Assert.Null(user);
        }

        [Fact]
        public void fetches_user_with_roles_by_existing_email()
        {
            var user = _userRepository.FindWithRolesByEmail("admin@admin.es");
            Assert.IsType<User>(user);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), user.Id);
            Assert.Single(user.Roles);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), user.Roles.First().Id);
        }

        [Fact]
        public void does_not_fetch_deleted_users()
        {
            var users = _userRepository.FindAllNotDeletedWithRoles();
            Assert.Single(users);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), users.First().Id);
        }

        private void SetUpUsers()
        {
            _users = new List<User>()
            {
                new User()
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Email = "admin@admin.es",
                    Password = "password",
                    Name = "John",
                    Surname = "Admin Doe",
                    Roles = new List<Role>()
                    {
                        new Role()
                        {
                            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            Type = "admin",
                        }
                    }
                },
                new User()
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Email = "user@user.es",
                    Password = "password",
                    Name = "John",
                    Surname = "Doe",
                    DeletedAt = DateTime.Now
                }
            }.AsQueryable();
        }

        private void SetUpUserRepository()
        {
            var dbSet = new Mock<DbSet<User>>();
            dbSet.As<IQueryable<User>>().Setup(set => set.Provider).Returns(_users.Provider);
            dbSet.As<IQueryable<User>>().Setup(set => set.Expression).Returns(_users.Expression);
            dbSet.As<IQueryable<User>>().Setup(set => set.ElementType).Returns(_users.ElementType);
            dbSet.As<IQueryable<User>>().Setup(set => set.GetEnumerator()).Returns(_users.GetEnumerator());

            var dbContext = new Mock<AppDbContext>();
            dbContext.Setup(context => context.Set<User>()).Returns(dbSet.Object);

            _userRepository = new UserRepository(dbContext.Object);
        }
    }
}