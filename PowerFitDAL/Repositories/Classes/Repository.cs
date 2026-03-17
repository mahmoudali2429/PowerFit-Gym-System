using Microsoft.EntityFrameworkCore;
using PowerFitDAL.Data.Contexts;
using PowerFitDAL.Entities;
using PowerFitDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Repositories.Classes
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly PowerFitDbContext _dbcontext;

        internal DbSet<T> DbSet;

        public Repository(PowerFitDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            DbSet = _dbcontext.Set<T>();
        }
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(int id)
        {
            var entityToDelete = DbSet.Find(id);
            if (entityToDelete != null)
                DbSet.Remove(entityToDelete);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.AsNoTracking().ToList();
        }

        public T? GetById(int id)
        {
            return DbSet.Find(id);
        }

        public int Save()
        {
           return _dbcontext.SaveChanges();
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }
    }
}
