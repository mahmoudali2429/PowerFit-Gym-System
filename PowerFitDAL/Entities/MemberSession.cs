using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Entities
{
    public class MemberSession : BaseEntity
    {
        //BookingDate == CreatedAt Of BaseEntity
        public bool IsAttended { get; set; }

        #region MemberSession > Session Relationship
        [ForeignKey(nameof(Session))]
        public int SessionId { get; set; }
        public Session BookedSession { get; set; } = default!;
        #endregion

        #region MemberSession > Member Relationship
        [ForeignKey(nameof(Member))]
        public int MemberId { get; set; }
        public Member Member { get; set; } = default!;
        #endregion
    }
}
