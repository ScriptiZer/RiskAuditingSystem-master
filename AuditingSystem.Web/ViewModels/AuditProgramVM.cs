using AuditingSystem.Entities.AuditFieldWork;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;

namespace AuditingSystem.Web.ViewModels
{
    public class AuditProgramVM
    {
        public virtual AuditProgramList? AuditProgramList { get; set; }
        public virtual AuditProgram? AuditProgram { get; set; }
        public virtual Company? Company { get; set; }
        public virtual Department? Department { get; set; }
        public virtual RiskIdentification? RiskIdentification { get; set; }
        public virtual Control? Control { get; set; }
        public virtual RiskCategory? RiskCategory { get; set; }
        public virtual ControlType? ControlType { get; set; }
        public virtual ControlProcess? ControlProcess { get; set; }
        public virtual ControlFrequency? ControlFrequency { get; set; }
        public virtual ControlEffectiveness? ControlEffectiveness { get; set; }
    }
}
