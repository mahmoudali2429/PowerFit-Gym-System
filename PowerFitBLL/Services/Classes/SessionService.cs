using AutoMapper;
using PowerFitBLL.Services.Interfaces;
using PowerFitBLL.ViewModels.SessionViewModel;
using PowerFitDAL.Entities;
using PowerFitDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace PowerFitBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Helper Methods
        private bool IsTrainerExists(int trainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return false;
            return true;
        }

        private bool IsCategoryExists(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate;
        }

        private bool IsSessionAvailableToUpdating(Session session)
        {

            if (session is null) return false;
            //If Session Completed - No Update Allowed
            if (session.EndDate < DateTime.Now) return false;
            //If Session Started - No Update Allowed
            if (session.StartDate <= DateTime.Now) return false;
            //If Session Has Active Booking - No Update Allowed
            var hasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (hasActiveBooking) return false;

            return true;
        }
        private bool IsSessionAvailableToRemoving(Session session)
        {

            if (session is null) return false;
            //If Session Started And Not Completed - No Update Allowed
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            //If Session In Upcoming - No Update Allowed
            if (session.StartDate > DateTime.Now) return false;
            //If Session Has Active Booking - No Update Allowed
            var hasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (hasActiveBooking) return false;

            return true;
        }
        #endregion

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!sessions.Any()) return [];

            //var sessionsVM = sessions.Select(S => new SessionViewModel
            //{
            //    Id = S.Id,
            //    Description = S.Description,
            //    StartDate = S.StartDate,
            //    EndDate = S.EndDate,
            //    Capacity = S.Capacity,
            //    TrainerName = S.Trainer.Name,
            //    CategoryName = S.Category.CategoryName,
            //    AvailableSlots = S.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(S.Id),

            //});
            //return sessionsVM;
            
            var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSessions)
            {
                var bookedSlots = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
                var availableSlots = session.Capacity - bookedSlots;
                session.AvailableSlots = availableSlots;
            }
            return mappedSessions;
        }
        public SessionViewModel? GetSessionDetails(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if(session is null) return null;
            //var sessionVM = new SessionViewModel
            //{
            //    Description = session.Description,
            //    CategoryName = session.Category.CategoryName,
            //    TrainerName = session.Trainer.Name,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    Capacity= session.Capacity,
            //    AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(sessionId)
            //};
            //return sessionVM;
            var bookedSlots = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            var availableSlots = session.Capacity - bookedSlots;

            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = availableSlots;

            return mappedSession;
        }
        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            //Check if trainer exists
            if (!IsTrainerExists(createdSession.TrainerId)) return false;
            //Check if category exists
            if (!IsCategoryExists(createdSession.CategoryId)) return false;
            //Check if StartDate before EndDate
            if (!IsDateTimeValid(createdSession.StartDate, createdSession.EndDate)) return false;
            //Check if 0 > Capacity > 25
            if (createdSession.Capacity > 25 || createdSession.Capacity < 0) return false;

            try
            {
                var sessionEntity = _mapper.Map<Session>(createdSession);
                _unitOfWork.GetRepository<Session>().Add(sessionEntity);
                return _unitOfWork.Save() > 0;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Create Session Failed {ex}");
                return false;
            }
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (session is null || !IsSessionAvailableToUpdating(session)) return null; 

            return _mapper.Map<UpdateSessionViewModel>(session);
        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel updatedSession)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (session is null || !IsSessionAvailableToUpdating(session)) return false;
            if (!IsTrainerExists(updatedSession.TrainerId)) return false;
            if (!IsDateTimeValid(updatedSession.StartDate, updatedSession.EndDate)) return false;
            
            try
            {
                _mapper.Map(updatedSession, session);
                _unitOfWork.SessionRepository.Update(session);
                return _unitOfWork.Save() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Failed : {ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (session is null || !IsSessionAvailableToRemoving(session)) return false;

            try
            {
                _unitOfWork.SessionRepository.Delete(session);
                return _unitOfWork.Save() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Failed : {ex}");
                return false;
            }
        }
    }
}
