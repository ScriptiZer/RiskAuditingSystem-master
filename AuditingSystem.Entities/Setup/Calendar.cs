using AuditingSystem.Entities.AuditProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.Setup
{
    public class Calendar:Base
    {
        public virtual int Id { get; set; }
        public virtual int? CompanyId { get; set; }
        public virtual int? DepartmentId { get; set; }
        public virtual int? FunctionId { get; set; }
        public virtual int? UserId { get; set; }
        public virtual int? YearId { get; set; }
        public virtual DateTime? FromDate { get; set; }
        public virtual DateTime? ToDate { get; set; }
        public virtual int? DaysNumber { get; set; }


        public virtual int DaysNumberInYear { get; set; }
        public virtual int Weekends { get; set; }
        public virtual int HolidaysNumber { get; set; }
        public virtual int NoofInternationalHlidays { get; set; }
        public virtual int NoofLeavesDays { get; set; }
        public virtual int SpecialWorkingHours { get; set; }
        public virtual int EstimatedSickLeaves { get; set; }
        public virtual int BalancefromPreviousYear { get; set; }
        public virtual int WorkingDays { get; set; }

        public virtual Company? Company { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Function? Function { get; set; }
        public virtual User? User { get; set; }
        public virtual Year? Year { get; set; }
    }
}
