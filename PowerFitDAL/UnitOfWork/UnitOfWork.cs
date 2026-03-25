using PowerFitDAL.Data.Contexts;
using PowerFitDAL.Entities;
using PowerFitDAL.Repositories.Classes;
using PowerFitDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly PowerFitDbContext _dbContext;
        public UnitOfWork(PowerFitDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var entityType = typeof(TEntity);
            if(_repositories.TryGetValue(entityType, out var repository))
                return (IRepository<TEntity>) repository;

            var newRepository = new Repository<TEntity>(_dbContext);
            _repositories[entityType] = newRepository;
            return newRepository;
        }

        public int Save()
        {
           return _dbContext.SaveChanges();
        }
    }
}
