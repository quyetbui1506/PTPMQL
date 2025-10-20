using Microsoft.EntityFrameworkCore;
using PTPMQL2526.Models;

namespace PTPMQL2526.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Person> Person { get; set; }  
        public DbSet<Employee> Employee { get; set; }
    }
}