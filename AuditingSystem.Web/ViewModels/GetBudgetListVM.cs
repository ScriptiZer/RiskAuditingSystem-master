using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;


namespace AuditingSystem.Web.ViewModels
{
    public class GetBudgetListVM
    {
        public AuditBudget AuditBudget { get; set; }
        public Company Company { get; set; }
        public User User { get; set; }
    }
}
