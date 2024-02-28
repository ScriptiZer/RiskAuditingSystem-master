using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AuditingSystem.Entities.Setup
{
    public class Year : Base
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual int CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public virtual string? Quarter { get; set; }
        public virtual IEnumerable<Quarter>? Quarters { get; set; }

        public virtual IEnumerable<AuditResourcesList>? AuditResourcesList { get; set; }
    }
}
