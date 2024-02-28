using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.Setup
{
    public class Role : Base
    {
        [Key]
        public virtual int Id { get; set; }
        
        public virtual IEnumerable<User>? Users { get; set; }
        public virtual IEnumerable<RolesPermissions>? RolesPermissions { get; set; }
    }
}
