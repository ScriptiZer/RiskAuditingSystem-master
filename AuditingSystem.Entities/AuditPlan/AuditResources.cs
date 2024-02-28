using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditPlan
{
    public class AuditResources: Base
    {
        public virtual int Id { get; set; }
        public virtual int? CompanyId { get; set; }
        public virtual int? UserId { get; set; }
        public virtual string? DepartmentId { get; set; }
        public virtual string? FunctionId { get; set; } // 21,22,23,24,...
        public virtual DateTime? PlanStartDate { get; set; }
        public virtual DateTime? PlanEndDate { get; set; }

        public virtual IEnumerable<AuditResourcesList>? AuditResourcesList { get; set; }

        public virtual Company? Company { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Function? Function { get; set; }
      
    }
}
