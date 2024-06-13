using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AuditingSystem.Entities
{
    public abstract class Base
    {
        [AllowHtml]
        public virtual string? Name { get; set; }
        public virtual int? Code { get; set; }


        [AllowHtml]
        public virtual string? Description { get; set; }
        public virtual bool? IsDeleted { get; set; } = false;
        public virtual int? CreatedBy { get; set; }
        public virtual DateTime? CreationDate { get; set; }
        public virtual int? UpdatedBy { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }
        public virtual int? CurrentYear { get; set; }
        public virtual int? CreatedByCompany { get; set; }

    }
}
