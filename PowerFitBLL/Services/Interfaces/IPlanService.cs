using PowerFitBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitBLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanDetails(int planId);
        UpdatePlanViewModel? GetPlanToUpdate(int planId);
        bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan);
        bool TogglePlanStatus(int planId);

    }
}
