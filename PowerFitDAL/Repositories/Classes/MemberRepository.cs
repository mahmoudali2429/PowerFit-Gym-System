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
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        private readonly PowerFitDbContext dbContext;

        public MemberRepository(PowerFitDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
