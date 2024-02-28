using AuditingSystem.Entities.AuditPlan;

namespace AuditingSystem.Web.ViewModels
{
    public class GroupedFinalAuditPlanViewModel
    {
        public string? DepartmentName { get; set; }
        public List<FinalAuditPlan>? FinalAuditPlans { get; set; }
    }
}
