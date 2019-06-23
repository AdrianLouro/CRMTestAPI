using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Extensions;
using Entities.Models;
using Entities.Models.Reduced;
using Xunit;

namespace Tests.Entities.Models
{
    public class User_
    {
        [Fact]
        public void maps_user_profile_fields()
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
            };

            UserProfile newUser = new UserProfile
            {
                Name = "NewJohn",
                Surname = "NewDoe"
            };

            user.Map(newUser);

            Assert.Equal(newUser.Name, user.Name);
            Assert.Equal(newUser.Surname, user.Surname);
        }

        [Fact]
        public void maps_fields_to_reduced_user()
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = "user1@user.es",
                Name = "John",
                Surname = "Doe",
                Roles = new List<Role>()
                {
                    new Role()
                    {
                        Id = Guid.NewGuid()
                    }
                }
            };

            ReducedUser reducedUser = user.ToReducedUser();

            Assert.Equal(user.Id, reducedUser.Id);
            Assert.Equal(user.Email, reducedUser.Email);
            Assert.Equal(user.Name, reducedUser.Name);
            Assert.Equal(user.Surname, reducedUser.Surname);
            Assert.Equal(1, reducedUser.Roles.Count);
            Assert.Equal(user.Roles.First().Id, reducedUser.Roles.First().Id);
        }

        [Fact]
        public void returns_true_when_asking_if_deleted_user_is_deleted()
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
                DeletedAt = DateTime.Now
            };

            Assert.True(user.IsDeleted());
        }

        [Fact]
        public void returns_false_when_asking_if_not_deleted_user_is_deleted()
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
            };

            Assert.False(user.IsDeleted());
        }
    }
}