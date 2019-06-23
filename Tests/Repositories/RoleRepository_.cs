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
    public class RoleRepository_ : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private IQueryable<Role> _roles;
        private RoleRepository _roleRepository;

        public RoleRepository_()
        {
            SetUpRoles();
            SetUpRoleRepository();
        }

        [Fact]
        public void does_not_fetch_role_by_user_and_not_existing_type()
        {
            var role = _roleRepository.FindByUserIdAndType(Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "superAdmin");
            Assert.Null(role);
        }

        [Fact]
        public void fetches_role_by_existing_user_and_type()
        {
            var role = _roleRepository.FindByUserIdAndType(Guid.Parse("11111111-1111-1111-1111-111111111111"), "admin");
            Assert.IsType<Role>(role);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), role.Id);
        }

        private void SetUpRoles()
        {
            _roles = new List<Role>()
            {
                new Role()
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Type = "admin",
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    User = new User()
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111")
                    }
                },
                new Role()
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Type = "admin",
                    UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    User = new User()
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222")
                    }
                }
            }.AsQueryable();
        }

        private void SetUpRoleRepository()
        {
            var dbSet = new Mock<DbSet<Role>>();
            dbSet.As<IQueryable<Role>>().Setup(set => set.Provider).Returns(_roles.Provider);
            dbSet.As<IQueryable<Role>>().Setup(set => set.Expression).Returns(_roles.Expression);
            dbSet.As<IQueryable<Role>>().Setup(set => set.ElementType).Returns(_roles.ElementType);
            dbSet.As<IQueryable<Role>>().Setup(set => set.GetEnumerator()).Returns(_roles.GetEnumerator());

            var dbContext = new Mock<AppDbContext>();
            dbContext.Setup(context => context.Set<Role>()).Returns(dbSet.Object);

            _roleRepository = new RoleRepository(dbContext.Object);
        }
    }
}