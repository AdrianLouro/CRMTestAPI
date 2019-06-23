using Entities.Models;
using Entities.Models.Reduced;

namespace Entities.Extensions
{
    public static class UserExtensions
    {
        public static void Map(this User dbUser, UserProfile user)
        {
            dbUser.Name = user.Name;
            dbUser.Surname = user.Surname;
        }

        public static ReducedUser ToReducedUser(this User user)
        {
            return new ReducedUser()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Roles = user.Roles
            };
        }
    }
}
