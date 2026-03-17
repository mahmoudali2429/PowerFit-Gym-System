using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Entities
{
    // Has 1 to 1 Relationship With Member [Shared PK]
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string BloodType { get; set; } = default!;
        public string? Note { get; set; }
    }
}
