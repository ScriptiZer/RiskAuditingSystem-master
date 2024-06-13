using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.FollowUp
{
    public class FirstDuty:Base
    {
        public int Id { get; set; }
        public int? SODTypeId { get; set; }
        public SODType? SODType { get; set; }
        public virtual IEnumerable<FirstSecondDuties>? FirstSecondDuties { get; set; }
    }
}
