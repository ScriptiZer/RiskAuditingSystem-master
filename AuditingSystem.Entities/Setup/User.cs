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
        [Required(ErrorMessage = "The Title field is required")]
        public virtual string? Title { get; set; }

        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress]
        public virtual string? Email { get; set; }

        [Required(ErrorMessage = "The Contact No field is required")]
        public virtual string? ContactNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public virtual string? Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public virtual string? ConfirmPassword { get; set; }

        public virtual int DaysNumber { get; set; }

        [Required(ErrorMessage = "The Role field is required")]
        public virtual int? RoleId { get; set; }
        public virtual Role? Role { get; set; }

        [Required(ErrorMessage = "The Company field is required")]
        public  virtual int? CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        [Required(ErrorMessage = "The Department field is required")]
        public virtual int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public virtual IEnumerable<AuditResourcesList>? AuditResourcesList { get; set; }
        public virtual IEnumerable<AuditBudget>? AuditBudgets { get; set; }

    }
}
