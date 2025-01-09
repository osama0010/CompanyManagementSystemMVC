using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly CompanyAppDbContext dbContext;

        public DepartmentRepository(CompanyAppDbContext dbContext) // Asking CLR to create object from DbContext (Dependency Injection)
        {
            this.dbContext = dbContext;
        }
        public int Add(Department department)
        {
            dbContext.Add(department);
            return dbContext.SaveChanges();
        }

        public int Delete(Department department)
        {
            dbContext.Remove(department);
            return dbContext.SaveChanges();
        }

        public IEnumerable<Department> GetAll()
            => dbContext.Departments.ToList();

        public Department GetById(int Id)
            => dbContext.Departments.Find(Id);

        public int Update(Department department)
        {
            dbContext.Update(department);
            return dbContext.SaveChanges();
        }
    }
}
