using AuditingSystem.Entities.AuditPlan;

namespace AuditingSystem.Web.ViewModels
{
    public class CombinedDraftAuditPlan
    {
        public int? Id { get; set; }
        public bool? IsOneYear { get; set; }
        public bool? IsTwoYear { get; set; }
        public bool? IsThreeYear { get; set; }
        public bool? IA { get; set; }
        public bool? GovAudit { get; set; }
        public bool? ELC { get; set; }
        public bool? RpetitiveFindings { get; set; }
        public IList<DraftAuditPlanList>? DraftAuditPlanList { get; set; }
    }
}
