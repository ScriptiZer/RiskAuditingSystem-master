using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.RiskAssessments
{
    public class RiskAssessmentController : Controller
    {
        private readonly IBaseRepository<RiskAssessmentList, int> _riskAssessmentListRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly AuditingSystemDbContext db;
        public RiskAssessmentController(
            IBaseRepository<RiskAssessmentList, int> riskAssessmentList, AuditingSystemDbContext db,
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<User, int> userReposiotry)
        {
            _riskAssessmentListRepository = riskAssessmentList;
            this.db = db;
            _departmentRepository = departmentRepository;
            _userRepository = userReposiotry;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var currentCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments.Where(c => c.IsDeleted == false), "Id", "Name");

            var list = from riskAssessment in db.RiskAssessmentsList
                       join riskIdentification in db.RiskIdentifications on riskAssessment.RiskIdentificationId equals riskIdentification.Id
                       join company in db.Companies on riskIdentification.CompanyId equals company.Id
                       join department in db.Departments on riskIdentification.DepartmentId equals department.Id
                       join function in db.Functions on riskIdentification.FunctionId equals function.Id
                       join control in db.Controls on riskAssessment.ControlId equals control.Id
                       join riskCategory in db.RiskCategories on riskIdentification.RiskCategoryId equals riskCategory.Id
                       join riskImpact in db.RiskImpacts on riskIdentification.RiskImpactId equals riskImpact.Id
                       join riskLikelihood in db.RiskLikehoods on riskIdentification.RiskLikelihoodId equals riskLikelihood.Id
                       join controlType in db.ControlTypes on control.ControlTypeId equals controlType.Id
                       join controlProcess in db.ControlProcesses on control.ControlProcessId equals controlProcess.Id
                       join controlFrequency in db.ControlFrequencies on control.ControlFrequencyId equals controlFrequency.Id
                       join controlEffectiveness in db.ControlEffectivenesses on control.ControlEffectivenessId equals controlEffectiveness.Id
                       where riskAssessment.RiskIdentificationId == riskIdentification.Id && riskAssessment.ControlId == control.Id &&
                       riskIdentification.IsDeleted == false && control.IsDeleted == false && riskAssessment.IsDeleted == false &&
                       company.IsDeleted == false && department.IsDeleted == false && riskCategory.IsDeleted == false && riskImpact.IsDeleted == false &&
                       riskLikelihood.IsDeleted == false && controlType.IsDeleted == false && controlProcess.IsDeleted == false &&
                       controlFrequency.IsDeleted == false && controlEffectiveness.IsDeleted == false && function.IsDeleted == false
                       orderby riskIdentification.Code
                       select new RiskAssessmentVM
                       {
                           Company = company,
                           Department = department,
                           Function = function,
                           RiskAssessmentList = riskAssessment,
                           RiskIdentification = riskIdentification,
                           RiskCategory = riskCategory,
                           RiskImpact = riskImpact,
                           RiskLikehood = riskLikelihood,
                           Control = control,
                           ControlType = controlType,
                           ControlProcess = controlProcess,
                           ControlFrequency = controlFrequency,
                           ControlEffectiveness = controlEffectiveness
                       };

            if(currentCompany.CompanyId != 1)
            {
                list = from riskAssessment in db.RiskAssessmentsList
                           join riskIdentification in db.RiskIdentifications on riskAssessment.RiskIdentificationId equals riskIdentification.Id
                           join company in db.Companies on riskIdentification.CompanyId equals company.Id
                           join department in db.Departments on riskIdentification.DepartmentId equals department.Id
                           join function in db.Functions on riskIdentification.FunctionId equals function.Id
                           join control in db.Controls on riskAssessment.ControlId equals control.Id
                           join riskCategory in db.RiskCategories on riskIdentification.RiskCategoryId equals riskCategory.Id
                           join riskImpact in db.RiskImpacts on riskIdentification.RiskImpactId equals riskImpact.Id
                           join riskLikelihood in db.RiskLikehoods on riskIdentification.RiskLikelihoodId equals riskLikelihood.Id
                           join controlType in db.ControlTypes on control.ControlTypeId equals controlType.Id
                           join controlProcess in db.ControlProcesses on control.ControlProcessId equals controlProcess.Id
                           join controlFrequency in db.ControlFrequencies on control.ControlFrequencyId equals controlFrequency.Id
                           join controlEffectiveness in db.ControlEffectivenesses on control.ControlEffectivenessId equals controlEffectiveness.Id
                           where riskAssessment.RiskIdentificationId == riskIdentification.Id && riskAssessment.ControlId == control.Id &&
                           riskIdentification.IsDeleted == false && control.IsDeleted == false && riskAssessment.IsDeleted == false &&
                           company.IsDeleted == false && department.IsDeleted == false && riskCategory.IsDeleted == false && riskImpact.IsDeleted == false &&
                           riskLikelihood.IsDeleted == false && controlType.IsDeleted == false && controlProcess.IsDeleted == false &&
                           controlFrequency.IsDeleted == false && controlEffectiveness.IsDeleted == false && function.IsDeleted == false
                           && (riskAssessment.CreatedByCompany == currentCompany.CompanyId || riskAssessment.CreatedByCompany == 1)
                       orderby riskIdentification.Code
                           select new RiskAssessmentVM
                           {
                               Company = company,
                               Department = department,
                               Function = function,
                               RiskAssessmentList = riskAssessment,
                               RiskIdentification = riskIdentification,
                               RiskCategory = riskCategory,
                               RiskImpact = riskImpact,
                               RiskLikehood = riskLikelihood,
                               Control = control,
                               ControlType = controlType,
                               ControlProcess = controlProcess,
                               ControlFrequency = controlFrequency,
                               ControlEffectiveness = controlEffectiveness
                           };
            }

            int totalRow = list.Count();
            double totalControl = 0.0;
            double totalInherentRisk = 0.0;
            foreach (var assessment in list)
            {
                totalInherentRisk += Convert.ToDouble(assessment.RiskIdentification.InherentRiskRating);
                totalControl += Convert.ToDouble(assessment.Control.ControlEffectivenessRating);
            }

            var tRiskControl = (totalControl > 1) ? (totalControl / totalRow) : 0;
            var tInherentRiskRisk = (totalInherentRisk > 1) ? (totalInherentRisk / totalRow) : 0;
            var tResidualRisk = ((tRiskControl + tInherentRiskRisk) > 1) ? ((tRiskControl + tInherentRiskRisk) / 2) : 0;
            var rRiskName = "";
            var rRiskColor = "";
            if (tResidualRisk <= 2)
            {
                rRiskName = "No major concern";
                rRiskColor = "#66FF33";
            }
            else if (tResidualRisk <= 4)
            {
                rRiskName = "Periodic Monitoring";
                rRiskColor = "#FFFF00";
            }
            else if (tResidualRisk < 6)
            {
                rRiskName = "Continuous Review";
                rRiskColor = "#FF9933";
            }
            else if (tResidualRisk >= 6)
            {
                rRiskName = "Active Management";
                rRiskColor = "#C00000";
            }
            ViewBag.tRiskControl = tRiskControl;
            ViewBag.tInherentRiskRisk = tInherentRiskRisk;
            ViewBag.tResidualRisk = tResidualRisk;
            ViewBag.rRiskName = rRiskName;
            ViewBag.rRiskColor = rRiskColor;

            var model = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalRow = list.Count();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(list.Count() / (double)pageSize);

            return View(model);

        }

        [HttpGet]
        public JsonResult GetDepartmentsByCompany(int CompanyId)
        {
            var departments = _departmentRepository.ListAsync(
                new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false, C => C.CompanyId == CompanyId },
                q => q.OrderBy(u => u.Id),
                null).Result;

            var departmentList = departments.Select(d => new { Id = d.Id, Name = d.Name }).ToList();

            return Json(departmentList);
        }

        [HttpGet]
        public IActionResult FilterAssessmentTable(int CompanyId, int departmentId, int year)
        {
            var query = from riskAssessment in db.RiskAssessmentsList
                        join riskIdentification in db.RiskIdentifications on riskAssessment.RiskIdentificationId equals riskIdentification.Id
                        join company in db.Companies on riskIdentification.CompanyId equals company.Id
                        join department in db.Departments on riskIdentification.DepartmentId equals department.Id
                        join control in db.Controls on riskAssessment.ControlId equals control.Id
                        join riskCategory in db.RiskCategories on riskIdentification.RiskCategoryId equals riskCategory.Id
                        join riskImpact in db.RiskImpacts on riskIdentification.RiskImpactId equals riskImpact.Id
                        join riskLikelihood in db.RiskLikehoods on riskIdentification.RiskLikelihoodId equals riskLikelihood.Id
                        join controlType in db.ControlTypes on control.ControlTypeId equals controlType.Id
                        join controlProcess in db.ControlProcesses on control.ControlProcessId equals controlProcess.Id
                        join controlFrequency in db.ControlFrequencies on control.ControlFrequencyId equals controlFrequency.Id
                        join controlEffectiveness in db.ControlEffectivenesses on control.ControlEffectivenessId equals controlEffectiveness.Id
                        where riskAssessment.RiskIdentificationId == riskIdentification.Id && riskAssessment.ControlId == control.Id &&
                        riskIdentification.IsDeleted == false && control.IsDeleted == false && riskAssessment.IsDeleted == false &&
                        company.IsDeleted == false && department.IsDeleted == false && riskCategory.IsDeleted == false && riskImpact.IsDeleted == false &&
                        riskLikelihood.IsDeleted == false && controlType.IsDeleted == false && controlProcess.IsDeleted == false &&
                        controlFrequency.IsDeleted == false && controlEffectiveness.IsDeleted == false
                        orderby company.Code,
                                department.Code,
                                riskIdentification.Code
                        select new RiskAssessmentVM
                        {
                            Company = company,
                            Department = department,
                            RiskAssessmentList = riskAssessment,
                            RiskIdentification = riskIdentification,
                            RiskCategory = riskCategory,
                            RiskImpact = riskImpact,
                            RiskLikehood = riskLikelihood,
                            Control = control,
                            ControlType = controlType,
                            ControlProcess = controlProcess,
                            ControlFrequency = controlFrequency,
                            ControlEffectiveness = controlEffectiveness
                        };

            if (CompanyId != 0)
            {
                query = query.Where(f => f.RiskIdentification.Company.Id == CompanyId);
            }

            if (departmentId != 0)
            {
                query = query.Where(f => f.RiskIdentification.Department.Id == departmentId && f.RiskIdentification.Company.Id == CompanyId);
            }

            if(year != 0)
            {
                query = query.Where(y => y.RiskAssessmentList.CurrentYear == year);
            }

            var riskAssessments = query.OrderBy(c => c.RiskIdentification.Code)
                                .ToList();

            int totalRow = riskAssessments.Count();
            double totalControl = 0.0;
            double totalInherentRisk = 0.0;
            foreach (var assessment in query)
            {
                totalInherentRisk += Convert.ToDouble(assessment.RiskIdentification.InherentRiskRating);
                totalControl += Convert.ToDouble(assessment.Control.ControlEffectivenessRating);
            }

            var tRiskControl = (totalControl > 1) ? (totalControl / totalRow) : 0;
            var tInherentRiskRisk = (totalInherentRisk > 1) ? (totalInherentRisk / totalRow) : 0;
            var tResidualRisk = ((tRiskControl + tInherentRiskRisk) > 1) ? ((tRiskControl + tInherentRiskRisk) / 2) : 0;
            var rRiskName = "";
            var rRiskColor = "";
            if (tResidualRisk <= 2)
            {
                rRiskName = "No major concern";
                rRiskColor = "#66FF33";
            }
            else if (tResidualRisk <= 4)
            {
                rRiskName = "Periodic Monitoring";
                rRiskColor = "#FFFF00";
            }
            else if (tResidualRisk < 6)
            {
                rRiskName = "Continuous Review";
                rRiskColor = "#FF9933";
            }
            else if (tResidualRisk >= 6)
            {
                rRiskName = "Active Management";
                rRiskColor = "#C00000";
            }

            var result = new
            {
                Data = riskAssessments.Select(a => new
                {
                    id = a.RiskAssessmentList.Id,
                    riskIdentificationCode = a.RiskIdentification.Code,
                    companyCode = a.RiskIdentification.Company.Code,
                    CompanyId = a.RiskIdentification.Company.Id,
                    companyName = a.RiskIdentification.Company.Name,
                    departmentId = a.RiskIdentification.Department.Id,
                    departmentCode = a.RiskIdentification.Department.Code,
                    departmentName = a.RiskIdentification.Department.Name,
                    functionId = a.RiskIdentification.Function.Id,
                    functionCode = a.RiskIdentification.Function.Code,
                    functionName = a.RiskIdentification.Function.Name,
                    riskIdentificationName = a.RiskIdentification.Name,
                    riskCategoryBGColor = a.RiskCategory.BGColor,
                    riskCategoryFontColor = a.RiskCategory.FontColor,
                    riskCategoryName = a.RiskCategory.Name,
                    riskIdentificationDescription = a.RiskIdentification.Description,
                    riskIdentificationRiskRatingRationalization = a.RiskIdentification.RiskRatingRationalization,
                    inherentRiskStatus = a.RiskIdentification.InherentRiskStatus,
                    riskImpactRate = a.RiskImpact.Rate,
                    riskImpactName = a.RiskImpact.Name,
                    riskLikehoodRate = a.RiskLikehood.Rate,
                    riskLikehoodName = a.RiskLikehood.Name,
                    riskIdentificationInherentRiskRating = a.RiskIdentification.InherentRiskRating,
                    controlName = a.Control.Name,
                    controlDescription = a.Control.Description,
                    controlTypeBGColor = a.ControlType.BGColor,
                    controlTypeFontColor = a.ControlType.FontColor,
                    controlTypeName = a.ControlType.Name,
                    controlProcessBGColor = a.ControlProcess.BGColor,
                    controlProcessFontColor = a.ControlProcess.FontColor,
                    controlProcessName = a.ControlProcess.Name,
                    controlFrequencyBGColor = a.ControlFrequency.BGColor,
                    controlFrequencyFontColor = a.ControlFrequency.FontColor,
                    controlFrequencyName = a.ControlFrequency.Name,
                    controlEffectivenessBGColor = a.ControlEffectiveness.BGColor,
                    controlEffectivenessFontColor = a.ControlEffectiveness.FontColor,
                    controlEffectivenessRate = a.ControlEffectiveness.Rate,
                    controlEffectivenessName = a.ControlEffectiveness.Name,
                    controlControlEffectivenessRating = a.Control.ControlEffectivenessRating,
                    riskAssessmentListResidualRiskRatingNumber = a.RiskAssessmentList.ResidualRiskRatingNumber,
                    riskAssessmentListResidualRiskRating = a.RiskAssessmentList.ResidualRiskRating
                }).ToList(),
                totalControlRow = totalRow,
                totalRiskControl = tRiskControl,
                totalInherentRisk = tInherentRiskRisk,
                totalResidualRisk = tResidualRisk,
                residualRiskName = rRiskName,
                residualRiskColor = rRiskColor
            };

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> TransferToYear(int assessmentId, int year)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var currentCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));
            var assessment = db.RiskAssessmentsList.Find(assessmentId);

            var riskAssessment = new RiskAssessmentList
            {
                RiskIdentificationId = assessment.RiskIdentificationId,
                ControlId = assessment.ControlId,
                ResidualRiskRating = assessment.ResidualRiskRating,
                Name = "Test",
                Description = "Test",
                IsDeleted = false,
                CreatedBy = assessment.CreatedBy,
                CreationDate = assessment.CreationDate,
                UpdatedBy = assessment.UpdatedBy,
                UpdatedDate = assessment.UpdatedDate,
                ResidualRiskRatingNumber = assessment.ResidualRiskRatingNumber,
                CreatedByCompany = currentCompany.CompanyId,
                CurrentYear = assessment.CurrentYear,
                IsTransfer = true,
                TransferByCompany = currentCompany.CompanyId,
                TransferByUser = userId,
                TransferDate = DateTime.Now,
                TransferToYear = year
            };

            db.RiskAssessmentsList.Add(riskAssessment);
            await db.SaveChangesAsync();

            return Ok(new { success = true });
        }

        public async Task<IActionResult> AnnualRiskAssessment(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var currentCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments.Where(c => c.IsDeleted == false), "Id", "Name");

            var list = from riskAssessment in db.RiskAssessmentsList
                       join riskIdentification in db.RiskIdentifications on riskAssessment.RiskIdentificationId equals riskIdentification.Id
                       join company in db.Companies on riskIdentification.CompanyId equals company.Id
                       join department in db.Departments on riskIdentification.DepartmentId equals department.Id
                       join function in db.Functions on riskIdentification.FunctionId equals function.Id
                       join control in db.Controls on riskAssessment.ControlId equals control.Id
                       join riskCategory in db.RiskCategories on riskIdentification.RiskCategoryId equals riskCategory.Id
                       join riskImpact in db.RiskImpacts on riskIdentification.RiskImpactId equals riskImpact.Id
                       join riskLikelihood in db.RiskLikehoods on riskIdentification.RiskLikelihoodId equals riskLikelihood.Id
                       join controlType in db.ControlTypes on control.ControlTypeId equals controlType.Id
                       join controlProcess in db.ControlProcesses on control.ControlProcessId equals controlProcess.Id
                       join controlFrequency in db.ControlFrequencies on control.ControlFrequencyId equals controlFrequency.Id
                       join controlEffectiveness in db.ControlEffectivenesses on control.ControlEffectivenessId equals controlEffectiveness.Id
                       where riskAssessment.RiskIdentificationId == riskIdentification.Id && riskAssessment.ControlId == control.Id &&
                       riskIdentification.IsDeleted == false && control.IsDeleted == false && riskAssessment.IsDeleted == false &&
                       company.IsDeleted == false && department.IsDeleted == false && riskCategory.IsDeleted == false && riskImpact.IsDeleted == false &&
                       riskLikelihood.IsDeleted == false && controlType.IsDeleted == false && controlProcess.IsDeleted == false &&
                       controlFrequency.IsDeleted == false && controlEffectiveness.IsDeleted == false && function.IsDeleted == false &&
                       riskAssessment.IsTransfer == true
                       orderby riskIdentification.Code
                       select new RiskAssessmentVM
                       {
                           Company = company,
                           Department = department,
                           Function = function,
                           RiskAssessmentList = riskAssessment,
                           RiskIdentification = riskIdentification,
                           RiskCategory = riskCategory,
                           RiskImpact = riskImpact,
                           RiskLikehood = riskLikelihood,
                           Control = control,
                           ControlType = controlType,
                           ControlProcess = controlProcess,
                           ControlFrequency = controlFrequency,
                           ControlEffectiveness = controlEffectiveness
                       };

            if (currentCompany.CompanyId != 1)
            {
                list = from riskAssessment in db.RiskAssessmentsList
                       join riskIdentification in db.RiskIdentifications on riskAssessment.RiskIdentificationId equals riskIdentification.Id
                       join company in db.Companies on riskIdentification.CompanyId equals company.Id
                       join department in db.Departments on riskIdentification.DepartmentId equals department.Id
                       join function in db.Functions on riskIdentification.FunctionId equals function.Id
                       join control in db.Controls on riskAssessment.ControlId equals control.Id
                       join riskCategory in db.RiskCategories on riskIdentification.RiskCategoryId equals riskCategory.Id
                       join riskImpact in db.RiskImpacts on riskIdentification.RiskImpactId equals riskImpact.Id
                       join riskLikelihood in db.RiskLikehoods on riskIdentification.RiskLikelihoodId equals riskLikelihood.Id
                       join controlType in db.ControlTypes on control.ControlTypeId equals controlType.Id
                       join controlProcess in db.ControlProcesses on control.ControlProcessId equals controlProcess.Id
                       join controlFrequency in db.ControlFrequencies on control.ControlFrequencyId equals controlFrequency.Id
                       join controlEffectiveness in db.ControlEffectivenesses on control.ControlEffectivenessId equals controlEffectiveness.Id
                       where riskAssessment.RiskIdentificationId == riskIdentification.Id && riskAssessment.ControlId == control.Id &&
                       riskIdentification.IsDeleted == false && control.IsDeleted == false && riskAssessment.IsDeleted == false &&
                       company.IsDeleted == false && department.IsDeleted == false && riskCategory.IsDeleted == false && riskImpact.IsDeleted == false &&
                       riskLikelihood.IsDeleted == false && controlType.IsDeleted == false && controlProcess.IsDeleted == false &&
                       controlFrequency.IsDeleted == false && controlEffectiveness.IsDeleted == false && function.IsDeleted == false
                       && (riskAssessment.CreatedByCompany == currentCompany.CompanyId || riskAssessment.CreatedByCompany == 1) &&
                       riskAssessment.IsTransfer == true
                       orderby riskIdentification.Code
                       select new RiskAssessmentVM
                       {
                           Company = company,
                           Department = department,
                           Function = function,
                           RiskAssessmentList = riskAssessment,
                           RiskIdentification = riskIdentification,
                           RiskCategory = riskCategory,
                           RiskImpact = riskImpact,
                           RiskLikehood = riskLikelihood,
                           Control = control,
                           ControlType = controlType,
                           ControlProcess = controlProcess,
                           ControlFrequency = controlFrequency,
                           ControlEffectiveness = controlEffectiveness
                       };
            }

            int totalRow = list.Count();
            double totalControl = 0.0;
            double totalInherentRisk = 0.0;
            foreach (var assessment in list)
            {
                totalInherentRisk += Convert.ToDouble(assessment.RiskIdentification.InherentRiskRating);
                totalControl += Convert.ToDouble(assessment.Control.ControlEffectivenessRating);
            }

            var tRiskControl = (totalControl > 1) ? (totalControl / totalRow) : 0;
            var tInherentRiskRisk = (totalInherentRisk > 1) ? (totalInherentRisk / totalRow) : 0;
            var tResidualRisk = ((tRiskControl + tInherentRiskRisk) > 1) ? ((tRiskControl + tInherentRiskRisk) / 2) : 0;
            var rRiskName = "";
            var rRiskColor = "";
            if (tResidualRisk <= 2)
            {
                rRiskName = "No major concern";
                rRiskColor = "#66FF33";
            }
            else if (tResidualRisk <= 4)
            {
                rRiskName = "Periodic Monitoring";
                rRiskColor = "#FFFF00";
            }
            else if (tResidualRisk < 6)
            {
                rRiskName = "Continuous Review";
                rRiskColor = "#FF9933";
            }
            else if (tResidualRisk >= 6)
            {
                rRiskName = "Active Management";
                rRiskColor = "#C00000";
            }
            ViewBag.tRiskControl = tRiskControl;
            ViewBag.tInherentRiskRisk = tInherentRiskRisk;
            ViewBag.tResidualRisk = tResidualRisk;
            ViewBag.rRiskName = rRiskName;
            ViewBag.rRiskColor = rRiskColor;

            var model = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalRow = list.Count();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(list.Count() / (double)pageSize);

            return View(model);

        }

    }
}
