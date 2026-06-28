using Microsoft.EntityFrameworkCore;
using MinimalAPIsProtgress.Models;

namespace MinimalAPIsProtgress.Data
{
    public class OfficeDb : DbContext
    {

        public OfficeDb(DbContextOptions<OfficeDb> options) : base(options)
        {

        }

        public DbSet<Employee> Employees => Set<Employee>();

    }
}
