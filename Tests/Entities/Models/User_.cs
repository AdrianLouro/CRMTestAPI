using System;
using System.Linq;
using Entities.Extensions;
using Entities.Models;
using Xunit;

namespace Tests.Entities.Models
{
    public class User_
    {
        [Fact]
        public void does_not_map_new_user_id()
        {
            User user = new User
            {
                Id = Guid.NewGuid()
            };

            User newUser = new User()
            {
                Id = Guid.NewGuid()
            };
            

            Assert.NotEqual(newUser.Id, user.Id);
        }

        [Fact]
        public void maps_user_fields()
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe"
            };
            user.Roles.Concat(new[] {new Role()});

            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "NewJohn",
                Surname = "NewDoe"
            };

            user.Map(newUser);

            Assert.Equal(newUser.Name, user.Name);
            Assert.Equal(newUser.Surname, user.Surname);
            Assert.Equal(newUser.Roles, user.Roles);
        }
    }
}
