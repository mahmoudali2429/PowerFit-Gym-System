using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Entities
{
    public class Membership : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //Read Only Property 
        public string Status
        {
            get
            {
                if (EndDate >= DateTime.Now)
                    return "Espired";
                else
                    return "Active";
            }
        }

        #region Membership > Member Relationship 
        [ForeignKey (nameof(Member))]
        public int MemberId { get; set; }
        public Member Member { get; set; } = default!;
        #endregion

        #region Membership > Plan Relationship
        [ForeignKey (nameof (Plan))]
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = default!;
        #endregion
    }
}
