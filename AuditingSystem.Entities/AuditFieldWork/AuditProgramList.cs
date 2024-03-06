using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
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
        public virtual AuditProgram? AuditProgram { get; set; }

        public virtual int RiskIdenticationId { get; set; }
        public virtual RiskIdentification? RiskIdentification { get; set; }

        public virtual int ControlId { get; set; }
        public virtual Control? Control { get; set; }

    }
}
