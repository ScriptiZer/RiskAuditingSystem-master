using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;

namespace AuditingSystem.Services.Implements
{
    internal class GetBudgetListViewModel
    {
        public AuditResources AuditResources { get; set; }
        public AuditBudget AuditBudget { get; set; }
        public User User { get; set; }
        public Company Company { get; set; }
        public Department Department { get; set; }
        public AuditResourcesList AuditResourcesList { get; set; }
    }
}