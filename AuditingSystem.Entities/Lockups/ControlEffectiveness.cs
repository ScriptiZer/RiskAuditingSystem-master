using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.Lockups
{
    public class ControlEffectiveness: Base
    {
        [Key]
        public int Id { get; set; }
        public int? Rate { get; set; }
        public string? BGColor { get; set; }
        public string? FontColor { get; set; }

    
    }
}
