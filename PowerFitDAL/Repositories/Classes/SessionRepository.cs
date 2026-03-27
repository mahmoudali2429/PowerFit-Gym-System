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
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        private readonly PowerFitDbContext _dbContext;

        public SessionRepository(PowerFitDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _dbContext.Sessions.Include(X => X.Trainer)
                                      .Include(X => X.Category)   
                                      .ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(x => x.SessionId == sessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _dbContext.Sessions.Include(X => X.Trainer)
                                      .Include(X => X.Category)
                                      .FirstOrDefault(X => X.Id == sessionId);
        }
    }
}
