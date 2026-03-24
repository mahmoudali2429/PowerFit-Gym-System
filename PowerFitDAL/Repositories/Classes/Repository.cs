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
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            var query = DbSet.AsNoTracking().Where(e => !e.IsDeleted);

            if (condition is null)
                return query.ToList();
            else
                return query.Where(condition).ToList();
        }

        public TEntity? GetById(int id)
        {
            
            var entity = DbSet.Find(id);
            if (entity is null || entity.IsDeleted) 
                return null;
            else 
                return entity;
        }

        public void Update(TEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;
            DbSet.Update(entity);
        }
    }
}
