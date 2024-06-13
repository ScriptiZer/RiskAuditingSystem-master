using AuditingSystem.Entities.AuditPlan;

namespace AuditingSystem.Web.ViewModels
{
    public class CombinedAuditResourcesModel
    {
        public int? ResourceId { get; set; }
        public int? AssignedToUserId { get; set; }
       // public List<AuditResourcesList>?  auditResourcesLists { get; set; }
        public List<AuditResourcesListStartEndDates>? StartDateEndDateList { get; set; }
    }
}
