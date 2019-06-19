using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}