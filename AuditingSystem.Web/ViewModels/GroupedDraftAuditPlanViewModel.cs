using AuditingSystem.Entities.AuditPlan;

namespace AuditingSystem.Web.ViewModels
{
    public class GroupedDraftAuditPlanViewModel
    {
        public string? DepartmentName { get; set; }
        public List<DraftAuditPlan>? DraftAuditPlans { get; set; }
    }
}
