using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditFieldWork
{
    public class AuditProgram: Base
    {
        public virtual int Id { get; set; }
        public virtual int CompanyId { get; set; }
        public virtual int DepartmentId { get; set; }
        public virtual int AuditorId { get; set; }
        public virtual DateTime? ActualDate { get; set; }
        public virtual DateTime? Period { get; set; }
        public virtual Company? Company { get; set; }
        public virtual Department? Department { get; set; }
        public virtual User? User { get; set; }
        public virtual List<AuditProgramList>? AuditProgramLists { get; set; }
    }
}
