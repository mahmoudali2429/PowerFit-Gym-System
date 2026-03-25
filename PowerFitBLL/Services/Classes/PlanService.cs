using PowerFitBLL.Services.Interfaces;
using PowerFitBLL.ViewModels.PlanViewModels;
using PowerFitDAL.Entities;
using PowerFitDAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Helper Methods
        private bool HasActiveMemberships(int planId)
        {
            var ActiveMemberships = _unitOfWork.GetRepository<Membership>()
                                               .GetAll(X => X.PlanId == planId && X.Status == "Active");
            return ActiveMemberships.Any();
        }
        #endregion
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var allPlansFromDb = _unitOfWork.GetRepository<Plan>().GetAll();
            if (!allPlansFromDb.Any()) return [];

            var plansToVM = allPlansFromDb.Select(P => new PlanViewModel
            {
                Id = P.Id,
                Name = P.Name,
                Description = P.Description,
                DurationDays = P.DurationDays,
                Price = P.Price,
                IsActive = P.IsActive,
            });

            return plansToVM;
        }

        public PlanViewModel? GetPlanDetails(int planId)
        {
            var planFromDb = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if(planFromDb is null) return null;

            var planVM = new PlanViewModel
            {
                Id = planFromDb.Id,
                Name = planFromDb.Name,
                Description = planFromDb.Description,
                DurationDays = planFromDb.DurationDays,
                Price = planFromDb.Price,
                IsActive = planFromDb.IsActive,
            };
            return planVM;
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || plan.IsActive == false || HasActiveMemberships(planId)) return null;

            var planVM = new UpdatePlanViewModel
            {
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
            };
            return planVM;
        }
        public bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if(plan is null || HasActiveMemberships(planId)) return false;
            try
            {
                plan.Description = updatedPlan.Description;
                plan.Price = updatedPlan.Price;
                plan.DurationDays = updatedPlan.DurationDays;

                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.Save() > 0;
            }
            catch 
            {
                return false;
            }
        }

        public bool TogglePlanStatus(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || HasActiveMemberships(planId)) return false;

            try
            {
                plan.IsActive = !plan.IsActive;
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.Save() > 0;
            }
            catch
            {
                return false;
            }
        }


    }
}
