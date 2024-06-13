using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Services.ViewModels
{
    public class RiskIdentificationVM
    {
        public Company? Company { get; set; }
        public Department? Department { get; set; }
        public Function? Function { get; set; }
        public RiskIdentification? RiskIdentification { get; set; }
        public RiskCategory? RiskCategory { get; set; }
        public RiskImpact? RiskImpact { get; set; }
        public RiskLikehood? RiskLikehood { get; set; }
    }
}
