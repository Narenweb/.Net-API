using Microsoft.EntityFrameworkCore;
using TaskAPI.Model;

namespace TaskAPI.Data
{
    public class GetDbContext:DbContext
    {
        public GetDbContext(DbContextOptions<GetDbContext> options)
           : base(options)
        {

        }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Users> Users{ get; set; }

        //protected override void OnModelCreating(ModelBuilder mb)
        //{
        //    mb.Entity<Employee>().HasData(
        //        new Employee()
        //        {
        //            EmpId = 1,
        //            EmpCode = 22,
        //            EmpName = "Narayanan",
        //            DeptId = 2,
        //            Salary = 50000
        //        });
        //    mb.Entity<Department>().HasData(
        //        new Department()
        //        {
        //            DeptId = 1,
        //            DeptCode = 40,
        //            DeptName = "Software"

        //        });
        //}

    }
}
