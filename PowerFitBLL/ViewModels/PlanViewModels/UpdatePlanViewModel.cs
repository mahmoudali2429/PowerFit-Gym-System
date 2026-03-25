using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        [Required(ErrorMessage = "Plan name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Plan name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Plan name can contain only letters and spaces")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 200 characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Duration Days is required")]
        [Range(1, 365, ErrorMessage = "Duration Days must be between 1 and 365 days ")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price Days is required")]
        public decimal Price { get; set; }
    }
}
