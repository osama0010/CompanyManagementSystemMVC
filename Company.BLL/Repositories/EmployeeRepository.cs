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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CompanyAppDbContext _dbContext;

        public EmployeeRepository(CompanyAppDbContext dbContext) // Ask CLR to create an object from DbContext
        {
            _dbContext = dbContext;
        }
        public int Add(Employee employee)
        {
            _dbContext.Add(employee);
            return _dbContext.SaveChanges();
        }

        public int Delete(Employee employee)
        {
            _dbContext.Add(employee);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
            => _dbContext.Employees.ToList();

        public Employee GetById(int Id)
            => _dbContext.Employees.Find(Id);


        public int Update(Employee employee)
        {
            _dbContext.Update(employee);
            return _dbContext.SaveChanges();
        }
    }
}
