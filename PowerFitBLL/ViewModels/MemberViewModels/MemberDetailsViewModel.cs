using PowerFitDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitBLL.ViewModels.MemberViewModels
{
    public class MemberDetailsViewModel
    {
        public string? Photo { get; set; }
        public string Name { get; set; } = null!;
        public string? PlanName { get; set; } 
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string? DateOfBirth { get; set; }
        public string? MembershipStartDate { get; set; }
        public string? MembershipEndDate { get; set; }
        public string? Address { get; set; } = null!;

    }
}
