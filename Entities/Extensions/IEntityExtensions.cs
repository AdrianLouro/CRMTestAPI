using System;

namespace Entities.Extensions
{
    public static class IEntityExtensions
    {
        public static bool IsEntityNull(this IEntity entity)
        {
            return entity == null;
        }

        public static bool IsEmptyEntity(this IEntity entity)
        {
            return entity.Id.Equals(Guid.Empty);
        }
    }
}
