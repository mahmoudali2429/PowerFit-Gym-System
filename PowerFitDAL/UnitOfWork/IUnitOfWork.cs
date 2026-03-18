using PowerFitDAL.Entities;
using PowerFitDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        public int Save();
    }
}
