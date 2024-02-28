﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.Lockups
{
    public class ControlProcess: Base
    {
        [Key]
        public int Id { get; set; }
        public string? BGColor { get; set; }
        public string? FontColor { get; set; }

        
    }
}
