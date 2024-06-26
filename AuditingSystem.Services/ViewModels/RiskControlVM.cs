﻿using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Services.ViewModels
{
    public class RiskControlVM
    {
        public Control? Control { get; set; }
        public ControlType? ControlType { get; set; }
        public ControlProcess? ControlProcess { get; set; }
        public ControlFrequency? ControlFrequency { get; set; }
        public ControlEffectiveness? ControlEffectiveness { get; set; }
        public RiskIdentification? RiskIdentification { get; set; }
    }
}
