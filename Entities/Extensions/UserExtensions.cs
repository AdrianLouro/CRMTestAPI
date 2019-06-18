using System;
using System.Linq;
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
    }
}
