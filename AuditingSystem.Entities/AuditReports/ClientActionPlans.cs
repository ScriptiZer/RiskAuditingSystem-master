using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditReports
{
    public class ClientActionPlans:Base
    {
        public virtual int? Id { get; set; }
        public virtual string? ManagementAcceptance { get; set; }

        [Display(Name = "Responsible Entity")]
        public virtual int? UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual int? DraftAuditReportsId { get; set; }
        public virtual DraftAuditReports? DraftAuditReports { get; set; }
        public virtual DateTime? CompletionDate { get; set; }
    }
}
