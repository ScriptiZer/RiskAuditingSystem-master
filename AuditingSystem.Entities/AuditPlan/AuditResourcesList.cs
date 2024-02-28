using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditPlan
{
    public class AuditResourcesList:Base
    {
        public virtual int Id { get; set; }
        public virtual int? AuditResourceId { get; set; }
        public virtual AuditResources? AuditResources { get; set; }         

        public int? Plan { get; set; }
        public int? Actual { get; set; }
                  
        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual string? ResourceType { get; set; }

        public virtual IEnumerable<AuditBudget>? AuditBudgets { get; set; }
    }
}
