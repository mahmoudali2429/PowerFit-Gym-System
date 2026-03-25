using PowerFitBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreateTrainerViewModel createdTrainer);
        TrainerDetailsViewModel? GetTrainerDetails(int trainerId);
        UpdateTrainerViewModel? GetTrainerToUpdate(int trainerId);
        bool UpdateTrainer(int trainerId, UpdateTrainerViewModel updatedTrainer);
        bool RemoveTrainer(int trainerId);
    }
}
