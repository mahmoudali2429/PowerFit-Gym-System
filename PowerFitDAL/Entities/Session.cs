using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = default!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region Session > Category Relationship
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        #endregion

        #region Session > Trainer Relationship
        [ForeignKey(nameof(Trainer))]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = default!;
        #endregion

        #region Session > MemberSession Relationship 
        public ICollection<MemberSession> SessionMembers { get; set; } = default!;
        #endregion
    }
}
