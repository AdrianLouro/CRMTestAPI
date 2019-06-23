using System;
using Entities.Extensions;
using Entities.Models;
using Entities.Models.Reduced;
using Xunit;

namespace Tests.Entities.Models
{
    public class Customer_
    {
        [Fact]
        public void maps_customer_profile_fields()
        {
            Customer customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe"
            };

            CustomerProfile customerProfile = new CustomerProfile
            {
                Name = "NewJohn",
                Surname = "NewDoe"
            };

            customer.Map(customerProfile);

            Assert.Equal(customerProfile.Name, customer.Name);
            Assert.Equal(customerProfile.Surname, customer.Surname);
        }
    }
}