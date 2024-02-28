using AuditingSystem.Entities.AuditPlan;

namespace AuditingSystem.Web.ViewModels
{
    public class CombinedAuditResourcesModel
    {
        public List<AuditResourcesList>?  auditResourcesLists { get; set; }
        public List<AuditResourcesListStartEndDates>? StartDateEndDateList { get; set; }
    }
}
