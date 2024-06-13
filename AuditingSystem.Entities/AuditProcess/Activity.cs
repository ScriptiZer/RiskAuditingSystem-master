using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.AuditProcess
{
    public class Activity:Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public virtual int Id { get; set; }
        public virtual int? IndustryId { get; set; }
        public virtual int? CompanyId { get; set; }
        public virtual int? DepartmentId { get; set; }
        public virtual int? FunctionId { get; set; }
        public string? Source { get; set; }
        public virtual Industry? Industry { get; set; }
        public virtual Company? Company { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Function? Function { get; set; }
        public virtual IEnumerable<Objective>? Objectives { get; set; }
    }
}
