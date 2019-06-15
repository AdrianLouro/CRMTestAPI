using Entities.Models;

namespace Entities.Extensions
{
    public static class RoleExtensions
    {
        public static void Map(this Role dbRole, Role role)
        {
            dbRole.Type = role.Type;
        }
    }
}