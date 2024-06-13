using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditReports
{
    public class DraftAuditReports:Base
    {
        public virtual int Id { get; set; }
        //public virtual int? IndustryId { get; set; }
        //public virtual Industry? Industry { get; set; }
        public virtual int? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public virtual int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        public virtual int? FunctionId { get; set; }
        public virtual Function? Function { get; set; }
        public virtual int? FindingId { get; set; }
        public virtual Finding? Finding { get; set; }
        public virtual int? ObservationId { get; set; }
        public virtual Observation? Observation { get; set; }
        public virtual string? Significance { get; set; }
        public virtual string? RiskImpact { get; set; }
        public virtual int? RiskCategoryId { get; set; }
        public virtual RiskCategory? RiskCategory { get; set; }
        public virtual string? Recomendation { get; set; }
    }
}
