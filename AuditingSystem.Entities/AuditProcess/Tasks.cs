using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditProcess
{
    public class Tasks:Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        public int? IndustryId { get; set; }
        public int? CompanyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? FunctionId { get; set; }
        public int? ActivityId { get; set; }
        public int? ObjectiveId { get; set; }
        public string? Source { get; set; }
        public virtual Industry? Industry { get; set; }
        public virtual Company? Company { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Function? Function { get; set; }
        public virtual Activity? Activity { get; set; }
        public virtual Objective? Objective { get; set; }
        public virtual IEnumerable<Practice>? Practices { get; set; }
    }
}
