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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly PowerFitDbContext _dbcontext;

        internal DbSet<TEntity> DbSet;

        public Repository(PowerFitDbContext dbcontext)
        {
            _dbcontext = dbcontext;
            DbSet = _dbcontext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition is null)
                return DbSet.AsNoTracking().ToList();
            else
                return DbSet.AsNoTracking().Where(condition).ToList();
        }

        public TEntity? GetById(int id)
        {
            return DbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
    }
}
