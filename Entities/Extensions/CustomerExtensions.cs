using Entities.Models;
using Entities.Models.Reduced;

namespace Entities.Extensions
{
    public static class CustomerExtensions
    {
        public static void Map(this Customer dbCustomer, CustomerProfile customer)
        {
            dbCustomer.Name = customer.Name;
            dbCustomer.Surname = customer.Surname;
        }
    }
}
