using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.FollowUp
{
    public class FirstSecondDuties:Base
    {
        public int Id { get; set; }
        public virtual int? SODTypeId { get; set; }
        public virtual int? FirstDutyId { get; set; }
        public virtual int? SecondDutyId { get; set; }
        public virtual string? Result { get; set; }
        public virtual string? Risk { get; set; }
        public virtual string? Mitigation { get; set; }

        public virtual SODType? SODType { get; set; }
        public virtual FirstDuty? FirstDuty { get; set; }
        public virtual SecondDuty? SecondDuty { get; set; }
    }
}
