using AuditingSystem.Entities.Lockups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditFieldWork
{
    public class AuditProgramList: Base
    {
        public virtual int Id { get; set; }
        public virtual int AuditProgramId { get; set; }
        public virtual string? MajorProcess { get; set; }
        public virtual string? SubProcess { get; set; }
        public virtual int RiskCategoryId { get; set; }
        public virtual string? RiskDescription { get; set; }
        public virtual string? ControlDescription { get; set; }
        public virtual string? TestDescription { get; set; }
        public virtual AuditProgram? AuditProgram { get; set; }
        public virtual RiskCategory? RiskCategory { get; set; }

    }
}
