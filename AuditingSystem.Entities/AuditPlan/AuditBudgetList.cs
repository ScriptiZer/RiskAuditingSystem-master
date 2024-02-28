using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditPlan
{
    public class AuditBudgetList: Base
    {
        public virtual int Id { get; set; }
        public virtual double? Estimated { get; set; }
        public virtual double? Actual { get; set; }
        public virtual int? Year { get; set; }
        public virtual string? Quarter { get; set; }
        public virtual string? Month { get; set; } // Jan,Feb,Mar, ...
        public virtual int BudgetId { get; set; }
        public virtual int CompanyId { get; set; }
        public virtual AuditBudget? AuditBudget { get; set; }
    }
}
