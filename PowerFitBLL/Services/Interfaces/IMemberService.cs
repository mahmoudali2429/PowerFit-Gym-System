using PowerFitBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitBLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberListViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel createdMember);
        MemberDetailsViewModel? GetMemberDetails(int memberId);
        MemberHealthRecordViewModel? GetMemberHealthRecordDetails(int memberId);
        UpdateMemberViewModel? GetMemberToUpdate(int memberId);
        bool UpdateMember(int memberId, UpdateMemberViewModel updatedMember);
        bool DeleteMember(int memberId);
    }
}
