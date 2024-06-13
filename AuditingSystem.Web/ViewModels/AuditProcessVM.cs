using System.Web.Mvc;

namespace AuditingSystem.Web.ViewModels
{
    public class AuditProcessVM
    {
        public int ResourceId { get; set; }
        public int? AssignedToUserId { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public List<string> Functions { get; set; }
        public virtual DateTime? PlanStartDate { get; set; }
        public virtual DateTime? PlanEndDate { get; set; }

        [AllowHtml]
        public string? Description { get; set; }
    }

}
