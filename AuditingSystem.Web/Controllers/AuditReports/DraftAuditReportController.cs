using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.AuditReports;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.AuditReports
{
    public class DraftAuditReportController : Controller
    {
        private readonly IBaseRepository<Industry, int> _industryRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly IBaseRepository<Finding, int> _findingRepository;
        private readonly IBaseRepository<Observation, int> _observationRepository;
        private readonly IBaseRepository<RiskImpact, int> _riskImpactRepository;
        private readonly IBaseRepository<RiskCategory, int> _riskCategoryRepository;
        private readonly IBaseRepository<DraftAuditReports, int> _draftAuditReports;
        public DraftAuditReportController(IBaseRepository<DraftAuditReports, int> draftAuditReports,
            IBaseRepository<Industry, int> industryRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<Function, int> functionRepository,
            IBaseRepository<Finding, int> findingRepository,
            IBaseRepository<Observation, int> observationRepository,
            IBaseRepository<RiskImpact, int> riskImpactRepository,
            IBaseRepository<RiskCategory, int> riskCategoryRepository)
        {
            _draftAuditReports = draftAuditReports;
            _industryRepository = industryRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _functionRepository = functionRepository;
            _findingRepository = findingRepository;
            _observationRepository = observationRepository;
            _riskImpactRepository = riskImpactRepository;
            _riskCategoryRepository = riskCategoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var draftAuditReport = await _draftAuditReports.ListAsync(
                       new Expression<Func<DraftAuditReports, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id),
                       d=>d.Department,f=>f.Function,f=>f.Finding,o=>o.Observation,c=>c.RiskCategory);

                return View(draftAuditReport);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            var industry = _industryRepository.ListAsync(
              new Expression<Func<Industry, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var company = _companyRepository.ListAsync(
              new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var department = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var function = _functionRepository.ListAsync(
              new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var finding = _findingRepository.ListAsync(
              new Expression<Func<Finding, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var observation = _observationRepository.ListAsync(
                  new Expression<Func<Observation, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var riskCategory = _riskCategoryRepository.ListAsync(
              new Expression<Func<RiskCategory, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            ViewBag.IndustryId = new SelectList(industry, "Id", "Name");
            ViewBag.CompanyId = new SelectList(company, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name");
            ViewBag.FunctionId = new SelectList(function, "Id", "Name");
            ViewBag.FindingId = new SelectList(finding, "Id", "Name");
            ViewBag.ObservationId = new SelectList(observation, "Id", "Name");
            //ViewBag.RiskImpactId = new SelectList(riskImpact, "Id", "Name");
            ViewBag.RiskCategoryId = new SelectList(riskCategory, "Id", "Name");
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var draftAuditReport = await _draftAuditReports.FindByAsync(id);
            var industry = _industryRepository.ListAsync(
              new Expression<Func<Industry, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var company = _companyRepository.ListAsync(
              new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var department = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var function = _functionRepository.ListAsync(
              new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var finding = _findingRepository.ListAsync(
              new Expression<Func<Finding, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var observation = _observationRepository.ListAsync(
                  new Expression<Func<Observation, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var riskImpact = _riskImpactRepository.ListAsync(
                  new Expression<Func<RiskImpact, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var riskCategory = _riskCategoryRepository.ListAsync(
              new Expression<Func<RiskCategory, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
          //  ViewBag.IndustryId = new SelectList(industry, "Id", "Name", draftAuditReport.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", draftAuditReport.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", draftAuditReport.DepartmentId);
            ViewBag.FunctionId = new SelectList(function, "Id", "Name", draftAuditReport.FunctionId);
            ViewBag.FindingId = new SelectList(finding, "Id", "Name", draftAuditReport.FindingId);
            ViewBag.ObservationId = new SelectList(observation, "Id", "Name", draftAuditReport.ObservationId);
            //ViewBag.RiskImpactId = new SelectList(riskImpact, "Id", "Name", draftAuditReport.RiskImpactId);
            ViewBag.RiskCategoryId = new SelectList(riskCategory, "Id", "Name", draftAuditReport.RiskCategoryId);
            return View(draftAuditReport);
        }

        public async Task<IActionResult> View(int id)
        {
            var draftAuditReport = await _draftAuditReports.FindByAsync(id);
            var industry = _industryRepository.ListAsync(
              new Expression<Func<Industry, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var company = _companyRepository.ListAsync(
              new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var department = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var function = _functionRepository.ListAsync(
              new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var finding = _findingRepository.ListAsync(
              new Expression<Func<Finding, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var observation = _observationRepository.ListAsync(
                  new Expression<Func<Observation, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            //var riskImpact = _riskImpactRepository.ListAsync(
            //      new Expression<Func<RiskImpact, bool>>[] { u => u.IsDeleted == false },
            //      q => q.OrderBy(u => u.Id),
            //      null).Result;
            var riskCategory = _riskCategoryRepository.ListAsync(
              new Expression<Func<RiskCategory, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            //ViewBag.IndustryId = new SelectList(industry, "Id", "Name", draftAuditReport.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", draftAuditReport.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", draftAuditReport.DepartmentId);
            ViewBag.FunctionId = new SelectList(function, "Id", "Name", draftAuditReport.FunctionId);
            ViewBag.FindingId = new SelectList(finding, "Id", "Name", draftAuditReport.FindingId);
            ViewBag.ObservationId = new SelectList(observation, "Id", "Name", draftAuditReport.ObservationId);
            //ViewBag.RiskImpactId = new SelectList(riskImpact, "Id", "Name", draftAuditReport.RiskImpactId);
            ViewBag.RiskCategoryId = new SelectList(riskCategory, "Id", "Name", draftAuditReport.RiskCategoryId);
            return View(draftAuditReport);
        }

        [HttpGet]
        public JsonResult GetCompanyByIndustry(int industryId)
        {
            var companies = _companyRepository.ListAsync(
                new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false, C => C.IndustryId == industryId },
                q => q.OrderBy(u => u.Id),
                null).Result;

            var companyList = companies.Select(d => new { Id = d.Id, Name = d.Name }).ToList();

            return Json(companyList);
        }

        [HttpGet]
        public JsonResult GetDepartmentsByCompany(int companyId)
        {
            var departments = _departmentRepository.ListAsync(
                new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false, C => C.CompanyId == companyId },
                q => q.OrderBy(u => u.Id),
                null).Result;

            var departmentList = departments.Select(d => new { Id = d.Id, Name = d.Name }).ToList();

            return Json(departmentList);
        }

        [HttpGet]
        public JsonResult GetFunctionsByDepartment(int departmentId)
        {

            var functions = _functionRepository.ListAsync(
                new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false, C => C.DepartmentId == departmentId },
                q => q.OrderBy(u => u.Id),
                null).Result;

            var departmentList = functions.Select(d => new { Id = d.Id, Name = d.Name }).ToList();

            return Json(departmentList);
        }
    }
}
