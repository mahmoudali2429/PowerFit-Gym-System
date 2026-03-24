using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitBLL.ViewModels.MemberViewModels
{
    public class MemberHealthRecordViewModel
    {
        [Required(ErrorMessage = "Height is required")]
        [Range(0.1 , 300, ErrorMessage ="Height between 0 and 300 Cm")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(0.1, 400, ErrorMessage = "Weight between 0 and 400 Kg")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type is required")]
        [StringLength(3, ErrorMessage ="Must be 3 characters or less ")]
        public string BloodType { get; set; } = null!;

        [StringLength(300, ErrorMessage = "Note must be less than 300 characters")]
        public string? Note { get; set; } 
    }
}
