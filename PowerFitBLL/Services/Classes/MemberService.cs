using PowerFitBLL.Services.Interfaces;
using PowerFitBLL.ViewModels.MemberViewModels;
using PowerFitDAL.Entities;
using PowerFitDAL.UnitOfWork;

namespace PowerFitBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region Helper Methods
        private bool IsEmailExists(string email, int? memberId = null)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == email && (memberId == null || X.Id != memberId)).Any();
        }
        private bool IsPhoneExists(string phone, int? memberId = null)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == phone && (memberId == null || X.Id != memberId)).Any();
        }

        #endregion
        public IEnumerable<MemberListViewModel> GetAllMembers()
        {
            var allMembersFromDb = _unitOfWork.GetRepository<Member>().GetAll();
            if (allMembersFromDb is null || !allMembersFromDb.Any()) return []; // [] Instead of Enumerable.Empty<MemberListViewModel>()

            var membersToVM = allMembersFromDb.Select(M => new MemberListViewModel
            {
                Id = M.Id,
                Name = M.Name,
                Photo = M.Photo,
                Email = M.Email,
                Phone = M.Phone,
                Gender = M.Gender.ToString(),
            });
            return membersToVM;
        }
        public bool CreateMember(CreateMemberViewModel createdMember)
        {
            try
            {
                // (IsEmailExists, IsPhoneExists) Are Methods In Helper Methods Region
                if (IsEmailExists(createdMember.Email) || IsPhoneExists(createdMember.Phone)) return false;

                var memberToDb = new Member()
                {
                    Name = createdMember.Name,
                    Email = createdMember.Email,
                    Phone = createdMember.Phone,
                    Gender = createdMember.Gender,
                    DateOfBirth = createdMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createdMember.BuildingNumber,
                        Street = createdMember.Street,
                        City = createdMember.City,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createdMember.MemberHealthRecordViewModel.Height,
                        Weight = createdMember.MemberHealthRecordViewModel.Weight,
                        BloodType = createdMember.MemberHealthRecordViewModel.BloodType,
                        Note = createdMember.MemberHealthRecordViewModel.Note,
                    }
                };

                _unitOfWork.GetRepository<Member>().Add(memberToDb);
                return _unitOfWork.Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public MemberDetailsViewModel? GetMemberDetails(int memberId)
        {
            var memberFromDb = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (memberFromDb is null) return null;

            var memberDetailsVM = new MemberDetailsViewModel
            {
                Name = memberFromDb.Name,
                Photo = memberFromDb.Photo,
                Phone = memberFromDb.Phone,
                Email = memberFromDb.Email,
                Gender = memberFromDb.Gender.ToString(),
                DateOfBirth = memberFromDb.DateOfBirth.ToShortDateString(),
                MembershipStartDate = memberFromDb.CreatedAt.ToShortDateString(),
                Address = $"{memberFromDb.Address.BuildingNumber} - {memberFromDb.Address.Street} - {memberFromDb.Address.City}",
            };

            // Get Active Membership For (MembershipStartDate & MembershipEndDate ) in View Model
            var activeMembership = _unitOfWork.GetRepository<Membership>().GetAll(X => X.MemberId == memberId && X.Status == "Active")
                                                                          .FirstOrDefault();
            if (activeMembership is not null)
            {
                memberDetailsVM.MembershipStartDate = activeMembership.StartDate.ToLongDateString();
                memberDetailsVM.MembershipEndDate = activeMembership.EndDate.ToLongDateString();

                //Get Plan By PlanId For (PlanName) in View Model
                var plan = _unitOfWork.GetRepository<Plan>().GetById(activeMembership.PlanId);
                memberDetailsVM.PlanName = plan?.Name;
            }

            return memberDetailsVM;
        }
        public MemberHealthRecordViewModel? GetMemberHealthRecordDetails(int memberId)
        {
            var healthRecordFromDb = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
            if (healthRecordFromDb is null) return null;

            var healthRecordVM = new MemberHealthRecordViewModel
            {
                Height = healthRecordFromDb.Height,
                Weight = healthRecordFromDb.Weight,
                BloodType = healthRecordFromDb.BloodType,
                Note = healthRecordFromDb.Note,
            };

            return healthRecordVM;
        }
        public UpdateMemberViewModel? GetMemberToUpdate(int memberId)
        {
            var memberFromDb = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (memberFromDb is null) return null;

            var memberToUpdateVM = new UpdateMemberViewModel
            {
                Name = memberFromDb.Name,
                Photo = memberFromDb.Photo,
                Phone = memberFromDb.Phone,
                Email = memberFromDb.Email,
                BuildingNumber = memberFromDb.Address.BuildingNumber,
                Street = memberFromDb.Address.Street,
                City = memberFromDb.Address.City,
            };

            return memberToUpdateVM;
        }
        public bool UpdateMember(int memberId, UpdateMemberViewModel updatedMember)
        {
            try
            {

                var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
                if (member is null) return false;

                // (IsEmailExists, IsPhoneExists) Are Methods In Helper Methods Region
                if (IsEmailExists(updatedMember.Email, memberId) || IsPhoneExists(updatedMember.Phone, memberId))
                    return false;

                member.Name = updatedMember.Name;
                member.Photo = updatedMember.Photo;
                member.Email = updatedMember.Email;
                member.Phone = updatedMember.Phone;
                member.Address.BuildingNumber = updatedMember.BuildingNumber;
                member.Address.Street = updatedMember.Street;
                member.Address.City = updatedMember.City;

                _unitOfWork.GetRepository<Member>().Update(member);
                return _unitOfWork.Save() > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteMember(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return false;

            var hasActiveMemberSessions = _unitOfWork.GetRepository<MemberSession>()
                                                     .GetAll(X => X.MemberId == memberId && X.BookedSession.StartDate > DateTime.Now)
                                                     .Any();
            if (hasActiveMemberSessions) return false;

            var memberships = _unitOfWork.GetRepository<Membership>().GetAll(X => X.MemberId == memberId);
            
            try
            {
                if (memberships.Any())
                {
                    foreach (var membership in memberships)
                    {
                        _unitOfWork.GetRepository<Membership>().Delete(membership);
                    }
                }
                _unitOfWork.GetRepository<Member>().Delete(member);

                return _unitOfWork.Save() > 0;
                
            }
            catch
            {
                return false;
            }
        }


    }
}
