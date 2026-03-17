using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Entities
{
    public class Member : GymUser
    {
        //'JoinDate' Property == 'CreatedAt' Property of BaseEntity
        public string? Photo { get; set; }

        #region Member > HealthRecord Relationship
        public HealthRecord HealthRecord { get; set; } = default!;
        #endregion

        #region Member > Membership Relationship 
        public ICollection<Membership> Memberships { get; set; } = default!;
        #endregion

        #region Member > MemberSession Relationship 
        
        public ICollection<MemberSession> MemberSessions { get; set; } = default!;
        #endregion
    }
}
