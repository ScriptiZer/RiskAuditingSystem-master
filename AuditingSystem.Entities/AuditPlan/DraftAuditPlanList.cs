using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditPlan
{
    public class DraftAuditPlanList : Base
    {
        public virtual int Id { get; set; }
        public virtual int? DraftAuditPlanId { get; set; }
        public virtual DraftAuditPlan? DraftAuditPlan { get; set; }

        public virtual string? Year { get; set; }
        public virtual string? Quarter { get; set; }
        public virtual int? Plan { get; set; }
        public virtual int? Actual { get; set; }


    }
}
