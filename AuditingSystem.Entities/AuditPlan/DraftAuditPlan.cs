using AuditingSystem.Entities.AuditProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditPlan
{
    public class DraftAuditPlan : Base
    {
        public virtual int Id { get; set; }
        public virtual int? CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public virtual int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public virtual int? FunctionId { get; set; }
        public virtual Function? Function { get; set; }

        public virtual bool? IsOneYear { get; set; }
        public virtual bool? IsTwoYear { get; set; }
        public virtual bool? IsThreeYear { get; set;}
        public virtual bool? IA { get; set; }
        public virtual bool? GovAudit { get; set; }
        public virtual bool? ELC { get; set; }
        public virtual bool? RpetitiveFindings { get; set; }
        public virtual int? Insource { get; set; }
        public virtual int? Outsource { get; set; }
        public virtual int? Manager { get; set; }
        public virtual int? AuditResourceId { get; set; }
        public virtual AuditResources? AuditResources { get; set; }
        public virtual int? AuditResourceListId { get; set; }
        public virtual AuditResourcesList? AuditResourcesList { get; set; }
        public virtual IEnumerable<DraftAuditPlanList>? DraftAuditPlanLists { get; set; }
    }
}
