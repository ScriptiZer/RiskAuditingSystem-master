using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.AuditReports;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.Setup;

namespace AuditingSystem.Web.ViewModels
{
    public class FinalAuditReportVM
    {
        public virtual DraftAuditReports DraftAuditReports { get; set; }
        public virtual ClientActionPlans ClientActionPlans { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual Company Company { get; set; }
        public virtual Department Department { get; set; }
        public virtual Function Function { get; set; }
        public virtual Finding Finding { get; set; }
        public virtual Observation Observation { get; set; }
        public virtual RiskCategory RiskCategory { get; set; }
        public virtual User User { get; set; }
    }
}
