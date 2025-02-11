using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> ,IEmployeeRepository
    {
        private readonly CompanyAppDbContext dbContext;

        public EmployeeRepository(CompanyAppDbContext dbContext):base(dbContext) // CLR injects an object from DbContext then chain it to base (one object created in Employee and passed as parameter to GenericRepo)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeeByName(string name)
            => dbContext.Employees.Where(E => E.Name == name);

    }
}
