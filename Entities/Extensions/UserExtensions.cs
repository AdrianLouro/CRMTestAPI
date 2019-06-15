using Entities.Models;

namespace Entities.Extensions
{
    public static class UserExtensions
    {
        public static void Map(this User dbUser, User user)
        {
            dbUser.Name = user.Name;
            dbUser.Surname = user.Surname;
            dbUser.Roles = user.Roles;
        }
    }
}
