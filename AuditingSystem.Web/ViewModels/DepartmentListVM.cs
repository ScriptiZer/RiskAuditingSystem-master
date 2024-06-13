using AuditingSystem.Entities.AuditProcess;

namespace AuditingSystem.Web.ViewModels
{
    public class DepartmentListVM
    {
        public Company? Company { get; set; }
        public Department? Department { get; set; }
        public IEnumerable<Function>? Functions { get; set; }
    }
}
