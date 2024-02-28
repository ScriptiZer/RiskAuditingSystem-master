using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditProcess
{
    public class Company : Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? ContactNo { get; set; }
        public string? Email { get; set; }
        public string? Source { get; set; }

        /// <summary>
        /// Plan year for resources
        /// </summary>
        public string? PlanYear { get; set; }
        /// <summary>
        /// Plan out sources for resources
        /// </summary>
        public string? Outsources { get; set; }
        /// <summary>
        /// Plan in sources for resources
        /// </summary>
        public string? Insources { get; set; }
        /// <summary>
        /// Plan manager for resources
        /// </summary>
        public string? Manager { get; set; }
        public int? IndustryId { get; set; }
        public virtual Industry? Industry { get; set; }

        /// <summary>
        /// Plan year for budget
        /// </summary>
        public string? BudgetPlanYear { get; set; }
        /// <summary>
        /// Plan out sources for budget
        /// </summary>
        public string? BudgetOutsources { get; set; }
        /// <summary>
        /// Plan in sources for budget
        /// </summary>
        public string? BudgetInsources { get; set; }
        /// <summary>
        /// Plan manager for budget
        /// </summary>
        public string? BudgetManager { get; set; }

        public virtual IEnumerable<User>? User { get; set; }
        public virtual IEnumerable<Year>? Years { get; set; }
        public virtual IEnumerable<Department>? Departments { get; set; }
    }
}
