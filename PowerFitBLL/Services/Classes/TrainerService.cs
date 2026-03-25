using PowerFitBLL.Services.Interfaces;
using PowerFitBLL.ViewModels.TrainerViewModels;
using PowerFitDAL.Entities;
using PowerFitDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region Helper Methods
        private bool IsEmailExists(string email, int? memberId = null)
        {
            return _unitOfWork.GetRepository<Trainer>()
                              .GetAll(X => X.Email == email && (memberId == null || X.Id != memberId))
                              .Any();
        }
        private bool IsPhoneExists(string phone, int? memberId = null)
        {
            return _unitOfWork.GetRepository<Trainer>()
                              .GetAll(X => X.Phone == phone && (memberId == null || X.Id != memberId))
                              .Any();
        }

        private bool HasActiveSessions(int trainerId)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>()
                                            .GetAll(X => X.TrainerId == trainerId && X.StartDate > DateTime.Now)
                                            .Any();
            return activeSessions;
        }

        #endregion

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainersFromDb = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainersFromDb is null || !trainersFromDb.Any()) return [];

            var trainersVM = trainersFromDb.Select(T => new TrainerViewModel
            {
                Id = T.Id,
                Name = T.Name,
                Email = T.Email,
                Phone = T.Phone,
                Specialties = T.Specialties.ToString(),
            });

            return trainersVM;
        }

        public TrainerDetailsViewModel? GetTrainerDetails(int trainerId)
        {
            var trainerFromDb = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainerFromDb is null) return null;

            var trainerVM = new TrainerDetailsViewModel
            {
                Name = trainerFromDb.Name,
                Phone = trainerFromDb.Phone,
                Photo = trainerFromDb.Photo,
                Email = trainerFromDb.Email,
                Specialties = trainerFromDb.Specialties.ToString(),
                HireDate = trainerFromDb.CreatedAt.ToShortDateString(),
                DateOfBirth = trainerFromDb.DateOfBirth.ToShortDateString(),
                Gender = trainerFromDb.Gender.ToString(),
                Address = $"{trainerFromDb.Address.BuildingNumber} - {trainerFromDb.Address.Street} - {trainerFromDb.Address.City}",

            };
            return trainerVM;
        }

        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            if (IsEmailExists(createdTrainer.Email) || IsPhoneExists(createdTrainer.Phone)) return false;
            try
            {
                var trainer = new Trainer
                {
                    Name= createdTrainer.Name,
                    Email= createdTrainer.Email,
                    Phone= createdTrainer.Phone,
                    DateOfBirth= createdTrainer.DateOfBirth,
                    Specialties = createdTrainer.Specialties,
                    Gender = createdTrainer.Gender,
                    Address = new Address
                    {
                        BuildingNumber = createdTrainer.BuildingNumber,
                        Street = createdTrainer.Street,
                        City = createdTrainer.City,
                    }
                };
                TrainerRepo.Add(trainer);
                return _unitOfWork.Save() > 0;
            }
            catch
            {
                return false;
            }
        }

        public UpdateTrainerViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainerToUpdate = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if(trainerToUpdate is null) return null;

            var trainerVM = new UpdateTrainerViewModel
            {
                Name = trainerToUpdate.Name, //For Display Only
                Photo = trainerToUpdate.Photo, //For Display Only
                Phone = trainerToUpdate.Phone,
                Email = trainerToUpdate.Email,
                BuildingNumber = trainerToUpdate.Address.BuildingNumber,
                Street = trainerToUpdate.Address.Street,
                City = trainerToUpdate.Address.City,
                specialties = trainerToUpdate.Specialties,
            };

            return trainerVM;

        }

        public bool UpdateTrainer(int trainerId, UpdateTrainerViewModel updatedTrainer)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var trainerToUpdate = Repo.GetById(trainerId);
            if (trainerToUpdate is null || IsEmailExists(trainerToUpdate.Email) || IsPhoneExists(trainerToUpdate.Phone))
                return false;

            try
            {
                trainerToUpdate.Email = updatedTrainer.Email;
                trainerToUpdate.Phone = updatedTrainer.Phone;
                trainerToUpdate.Address.BuildingNumber = updatedTrainer.BuildingNumber;
                trainerToUpdate.Address.Street = updatedTrainer.Street;
                trainerToUpdate.Address.City = updatedTrainer.City;
                trainerToUpdate.Specialties = updatedTrainer.specialties;
                
                Repo.Update(trainerToUpdate);
                return _unitOfWork.Save() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveTrainer(int trainerId)
        {
            var trainerToRemove = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainerToRemove is null || HasActiveSessions(trainerId)) return false;
            try
            {
                _unitOfWork.GetRepository<Trainer>().Delete(trainerToRemove);
                return _unitOfWork.Save() > 0;
            }
            catch
            {
                return false;
            }
        }

    }
}
