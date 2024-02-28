using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditPlan
{
    public class AuditBudget : Base
    {
        public virtual int Id { get; set; }
        public int CompanyId { get; set; }
        public virtual int? ResourceId { get; set; }
        public virtual int? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual string? BudgetType { get; set; }
        public virtual decimal? TotalEstmated { get; set; }
        public virtual decimal? TotalActual { get; set; }
        public virtual decimal? Variance { get; set; }
        public virtual string? ResourceType { get; set; }

        public virtual IEnumerable<AuditBudgetList>? AuditBudgetLists { get; set; }
    }
}
