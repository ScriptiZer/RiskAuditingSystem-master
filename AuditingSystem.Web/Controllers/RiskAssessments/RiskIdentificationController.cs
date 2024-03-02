using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Drawing.Printing;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.RiskAssessments
{
    public class RiskIdentificationController : Controller
    {
        private readonly IBaseRepository<RiskIdentification, int> _riskIdentificaionRepository;
        private readonly IBaseRepository<RiskCategory, int> _riskCategoryRepository;
        private readonly IBaseRepository<RiskImpact, int> _riskImpactRepository;
        private readonly IBaseRepository<RiskLikehood, int> _likelihoodRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;

        public RiskIdentificationController(
            IBaseRepository<RiskIdentification, int> riskIdentificaionRepository,
            IBaseRepository<RiskCategory, int> riskCategoryRepository,
            IBaseRepository<RiskImpact, int> riskImpactRepository,
            IBaseRepository<RiskLikehood, int> likelihoodRepository,
            IBaseRepository<Function, int> functionRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository)
        {
            _riskIdentificaionRepository = riskIdentificaionRepository;
            _riskCategoryRepository = riskCategoryRepository;
            _riskImpactRepository = riskImpactRepository;
            _likelihoodRepository = likelihoodRepository;
            _functionRepository = functionRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var riskIdentifications = await _riskIdentificaionRepository.ListAsync(
                new Expression<Func<RiskIdentification, bool>>[] { c => c.IsDeleted == false},
                q => q.OrderBy(u => u.Id),
            c => c.RiskCategory, c => c.RiskImpact, c => c.RiskLikelihood, c => c.Company, c => c.Department);
            var model = riskIdentifications.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalRow = riskIdentifications.Count();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(riskIdentifications.Count() / (double)pageSize);

            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var riskCategory = await _riskCategoryRepository.ListAsync(
                new Expression<Func<RiskCategory, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var riskImpact = await _riskImpactRepository.ListAsync(
                new Expression<Func<RiskImpact, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var likelihood = await _likelihoodRepository.ListAsync(
                new Expression<Func<RiskLikehood, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var company = await _companyRepository.ListAsync(
                new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var department = await _departmentRepository.ListAsync(
                new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var function = await _functionRepository.ListAsync(
              new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null);

            ViewBag.RiskCategoryId = new SelectList(riskCategory, "Id", "Name");
            ViewBag.CompanyId = new SelectList(company, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name");
            ViewBag.FunctionId = new SelectList(function, "Name", "Name");
            ViewBag.RiskImpactId = new SelectList(riskImpact.Select(r => new { Id = r.Id, Name = $"{r.Rate} - {r.Name}" }), "Id", "Name");
            ViewBag.RiskLikelihoodId = new SelectList(likelihood.Select(l => new { Id = l.Id, Name = $"{l.Rate} - {l.Name}" }), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWithLink(RiskIdentification riskIdentification)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            try
            {
                if (ModelState.IsValid)
                {
                    await _riskIdentificaionRepository.CreateAsync(riskIdentification);

                    // Assuming the repository returns the saved entity with its ID
                    var savedRiskIdentification = await _riskIdentificaionRepository.FindByAsync(riskIdentification.Id);

                    return Ok(new { id = savedRiskIdentification.Id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal Server Error");
            }

            return BadRequest("Invalid ModelState");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var riskIdentification = await _riskIdentificaionRepository.FindByAsync(id);

            var riskCategory = await _riskCategoryRepository.ListAsync(
                new Expression<Func<RiskCategory, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var riskImpact = await _riskImpactRepository.ListAsync(
                new Expression<Func<RiskImpact, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var likelihood = await _likelihoodRepository.ListAsync(
                new Expression<Func<RiskLikehood, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var function = await _functionRepository.ListAsync(
              new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null);

            var company = await _companyRepository.ListAsync(
                new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var department = await _departmentRepository.ListAsync(
                new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            ViewBag.RiskCategoryId = new SelectList(riskCategory, "Id", "Name", riskIdentification.RiskCategoryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", riskIdentification.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", riskIdentification.DepartmentId);
            ViewBag.FunctionId = new SelectList(function, "Name", "Name",riskIdentification.FunctionId);
            ViewBag.RiskImpactId = new SelectList(riskImpact.Select(r => new { Id = r.Id, Name = $"{r.Rate} - {r.Name}" }), "Id", "Name", riskIdentification.RiskImpactId);
            ViewBag.RiskLikelihoodId = new SelectList(likelihood.Select(l => new { Id = l.Id, Name = $"{l.Rate} - {l.Name}" }), "Id", "Name", riskIdentification.RiskLikelihoodId);

            return View(riskIdentification);
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var riskIdentification = await _riskIdentificaionRepository.FindByAsync(id);

            var riskCategory = await _riskCategoryRepository.ListAsync(
                new Expression<Func<RiskCategory, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var riskImpact = await _riskImpactRepository.ListAsync(
                new Expression<Func<RiskImpact, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var likelihood = await _likelihoodRepository.ListAsync(
                new Expression<Func<RiskLikehood, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var function = await _functionRepository.ListAsync(
              new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null);
            ViewBag.RiskCategoryId = new SelectList(riskCategory, "Id", "Name", riskIdentification.RiskCategoryId);
            ViewBag.FunctionId = new SelectList(function, "Name", "Name", riskIdentification.FunctionId);
            ViewBag.RiskImpactId = new SelectList(riskImpact.Select(r => new { Id = r.Id, Name = $"{r.Rate} - {r.Name}" }), "Id", "Name", riskIdentification.RiskImpactId);
            ViewBag.RiskLikelihoodId = new SelectList(likelihood.Select(l => new { Id = l.Id, Name = $"{l.Rate} - {l.Name}" }), "Id", "Name", riskIdentification.RiskLikelihoodId);

            return View(riskIdentification);
        }
    }
}
