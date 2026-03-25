using PowerFitDAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Entities
{
    public class Trainer : GymUser
    {
        // 'HireDate' Property == 'CreatedAt' of BaseEntity  
        public Specialties Specialties { get; set; }

        #region Trainer > Session Relationship 
        public ICollection<Session> TrainerSessions { get; set; } = default!;
        #endregion
    }
}
