using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditPlan
{
    public class FinalAuditPlanList : Base
    {
        public virtual int Id { get; set; }
        public virtual int? FinalAuditPlanId { get; set; }
        public virtual FinalAuditPlan? FinalAuditPlan { get; set; }

        public virtual string? Year { get; set; }
        public virtual string? Quarter { get; set; }
        public virtual int? Plan { get; set; }
        public virtual int? Actual { get; set; }
    }
}
