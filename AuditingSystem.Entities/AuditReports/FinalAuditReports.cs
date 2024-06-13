using AuditingSystem.Entities.AuditPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditReports
{
    public class FinalAuditReports:Base
    {
        public virtual int Id { get; set; }
        public virtual int? DraftAuditPlanId { get; set; }
        public virtual DraftAuditPlan? DraftAuditPlan { get; set; }
        public virtual ClientActionPlans? ClientActionPlans { get; set; }
    }
}
