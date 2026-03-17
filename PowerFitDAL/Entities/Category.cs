using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = default!;

        #region Category > Session Relationship
        public ICollection<Session> Sessions { get; set; } = default!;
        #endregion
    }
}
