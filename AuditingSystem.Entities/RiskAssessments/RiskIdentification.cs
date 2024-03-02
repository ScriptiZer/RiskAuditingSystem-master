using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Entities.RiskAssessments
{
    public class RiskIdentification:Base
    {
        [Key]
        public int Id { get; set; }
        public int? RiskCategoryId { get; set; }
        public virtual RiskCategory? RiskCategory { get; set; }

        public int? RiskImpactId { get; set; }
        public virtual RiskImpact? RiskImpact { get; set; }

        public int? RiskLikelihoodId { get; set; }
        public virtual RiskLikehood? RiskLikelihood { get; set; }

        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }


        public virtual int FunctionId { get; set; }
        public virtual Function? Function { get; set; }
        /// <summary>
        /// Contains value per sum for Risk Impact and Likelihood
        /// </summary>
        public int InherentRiskRating { get; set; }
        
        /// <summary>
        /// Description to select reason Risk Impact and Likelihood
        /// </summary>
        public string? RiskRatingRationalization { get; set; }

        public virtual IEnumerable<RiskAssessmentList>? RiskAssessments { get; set; }


    }
}
