using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.Setup
{
    public class User : Base
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string? Title { get; set; }

        [EmailAddress]
        public virtual string? Email { get; set; }

        public virtual string? ContactNo { get; set; }

        public virtual string? ResourceType { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public virtual string? Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public virtual string? ConfirmPassword { get; set; }

        public virtual int DaysNumber { get; set; }

        public virtual int? RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public  virtual int? CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public virtual int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public virtual IEnumerable<AuditResourcesList>? AuditResourcesList { get; set; }
        public virtual IEnumerable<AuditBudget>? AuditBudgets { get; set; }

    }
}
