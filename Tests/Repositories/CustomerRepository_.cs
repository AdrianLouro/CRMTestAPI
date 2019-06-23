using System;
using System.Collections.Generic;
using System.Linq;
using CRMTestAPI;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories;
using Xunit;

namespace Tests.Repositories
{
    public class CustomerRepository_ : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private IQueryable<Customer> _customers;
        private CustomerRepository _customerRepository;

        public CustomerRepository_()
        {
            SetUpCustomers();
            SetUpCustomerRepository();
        }

       private void SetUpCustomers()
        {
            _customers = new List<Customer>()
            {
                new Customer()
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "John",
                    Surname = "Doe 1",
                },
                new Customer()
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "John",
                    Surname = "Doe 2",
                }
            }.AsQueryable();
        }

        private void SetUpCustomerRepository()
        {
            var dbSet = new Mock<DbSet<Customer>>();
            dbSet.As<IQueryable<Customer>>().Setup(set => set.Provider).Returns(_customers.Provider);
            dbSet.As<IQueryable<Customer>>().Setup(set => set.Expression).Returns(_customers.Expression);
            dbSet.As<IQueryable<Customer>>().Setup(set => set.ElementType).Returns(_customers.ElementType);
            dbSet.As<IQueryable<Customer>>().Setup(set => set.GetEnumerator()).Returns(_customers.GetEnumerator());

            var dbContext = new Mock<AppDbContext>();
            dbContext.Setup(context => context.Set<Customer>()).Returns(dbSet.Object);

            _customerRepository = new CustomerRepository(dbContext.Object);
        }
    }
}