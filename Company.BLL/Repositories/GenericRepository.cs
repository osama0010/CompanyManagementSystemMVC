using Company.BLL.Interfaces;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CompanyAppDbContext _dbContext;

        public GenericRepository(CompanyAppDbContext dbContext) // Asking CLR to create object from DbContext (DI)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
        {
            _dbContext.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) _dbContext.Employees.Include(X => X.department).ToList();
            }
            return _dbContext.Set<T>().ToList();
        }

        public T GetById(int Id)
            => _dbContext.Set<T>().Find(Id);

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }
    }
}
