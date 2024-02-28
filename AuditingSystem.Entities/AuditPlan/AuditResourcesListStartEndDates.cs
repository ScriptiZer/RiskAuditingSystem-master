using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditPlan
{
    public class AuditResourcesListStartEndDates
    {
        public virtual int Id { get; set; }
        public virtual int? AuditResourceId { get; set; }
        public virtual string? YearId { get; set; }
        public virtual string? QuarterId { get; set; }
        //public virtual DateTime? PlanStartDate { get; set; }
        //public virtual DateTime? PlanEndDate { get; set; }
        public virtual DateTime? ActualStartDate { get; set; }
        public virtual DateTime? ActualEndDate { get; set; }
        public virtual int? AssignedToStartActualId { get; set; }
        public virtual int? AssignedToEndActualId { get; set; }
        public virtual AuditResources? AuditResources { get; set; }
        public virtual Year? Year { get; set; }
        public virtual Quarter? Quarter { get; set; }


    }
}
