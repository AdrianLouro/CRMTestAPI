using System;
using Entities.Extensions;
using Entities.Models;
using Xunit;

namespace Tests.Entities.Models
{
    public class Role_
    {
        [Fact]
        public void does_not_map_new_role_id()
        {
            Role role = new Role
            {
                Id = Guid.NewGuid()
            };

            Role newRole = new Role()
            {
                Id = Guid.NewGuid()
            };
            

            Assert.NotEqual(newRole.Id, role.Id);
        }

        [Fact]
        public void maps_role_fields()
        {
            Role role = new Role
            {
                Id = Guid.NewGuid(),
                Type = "admin",
                User = new User()
            };
 
            Role newRole = new Role
            {
                Id = Guid.NewGuid(),
                Type = "not_an_admin",
                User = new User()
            };

            role.Map(newRole);

            Assert.Equal(newRole.Type, role.Type);
            Assert.NotSame(newRole.User, role.User);
        }
    }
}
