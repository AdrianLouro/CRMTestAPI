using System;
using Entities.Contracts;

namespace Entities.Extensions
{
    public static class IEntityExtensions
    {
        public static bool IsNull(this IEntity entity)
        {
            return entity == null;
        }

        public static bool IsEmpty(this IEntity entity)
        {
            return entity.Id.Equals(Guid.Empty);
        }
    }
}
