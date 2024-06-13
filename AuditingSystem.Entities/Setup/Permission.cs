using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.Setup
{
    public class Permission:Base
    {
        public int Id { get; set; }
        public virtual bool? View { get; set; }
        public virtual bool? Add { get; set; }
        public virtual bool? Edit { get; set; }
        public virtual bool? Delete { get; set; }
        public virtual bool? ExportToPDF { get; set; }
        public virtual bool? ExportToWord { get; set; }
        public virtual bool? ExportToExcel { get; set; }
        public virtual IEnumerable<RolesPermissions>? RolesPermissions { get; set; }
    }
}
