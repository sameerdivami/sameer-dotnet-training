using Microsoft.EntityFrameworkCore;
using PolicyManagement.Entities;

namespace PolicyManagementApp.Data
{
    public class AppDbContext : DbContext  
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        
        public DbSet<Policy> Policies { get; set; }  
        public DbSet<User> Users { get; set; }

        public DbSet<UserPolicy> UserPolicies { get; set; }  

    }
}