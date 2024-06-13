using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.FollowUp;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Services.ViewModels;
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
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly AuditingSystemDbContext db;

        public RiskIdentificationController(
            IBaseRepository<RiskIdentification, int> riskIdentificaionRepository,
            IBaseRepository<RiskCategory, int> riskCategoryRepository,
            IBaseRepository<RiskImpact, int> riskImpactRepository,
            IBaseRepository<RiskLikehood, int> likelihoodRepository,
            IBaseRepository<Function, int> functionRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            AuditingSystemDbContext db,
            IBaseRepository<User, int> userRepository)
        {
            _riskIdentificaionRepository = riskIdentificaionRepository;
            _riskCategoryRepository = riskCategoryRepository;
            _riskImpactRepository = riskImpactRepository;
            _likelihoodRepository = likelihoodRepository;
            _functionRepository = functionRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            this.db = db;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var currentCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments.Where(c => c.IsDeleted == false), "Id", "Name");

                var riskIdentifications = await _riskIdentificaionRepository.ListAsync(
                    new Expression<Func<RiskIdentification, bool>>[] { c => c.IsDeleted == false && c.RiskCategory.IsDeleted == false &&
                                c.RiskImpact.IsDeleted == false && c.RiskLikelihood.IsDeleted == false && c.Company.IsDeleted == false
                                && c.Department.IsDeleted == false},
                     r => r.OrderBy(u => u.Code).ThenBy(u => u.Department.Name).ThenBy(u => u.Function.Name),
                c => c.RiskCategory, c => c.RiskImpact, c => c.RiskLikelihood, c => c.Company, c => c.Department, c => c.Function);
            
            if (currentCompany.CompanyId != 1)
            {
                riskIdentifications = await _riskIdentificaionRepository.ListAsync(
                    new Expression<Func<RiskIdentification, bool>>[] { c => c.IsDeleted == false && c.RiskCategory.IsDeleted == false &&
                                c.RiskImpact.IsDeleted == false && c.RiskLikelihood.IsDeleted == false && c.Company.IsDeleted == false
                                && c.Department.IsDeleted == false && (c.CreatedByCompany == currentCompany.CompanyId || c.CreatedByCompany == 1)},
                     r => r.OrderBy(u => u.Code).ThenBy(u => u.Department.Name).ThenBy(u => u.Function.Name),
                c => c.RiskCategory, c => c.RiskImpact, c => c.RiskLikelihood, c => c.Company, c => c.Department, c => c.Function);
            }


            var model = riskIdentifications.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalRow = riskIdentifications.Count();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(riskIdentifications.Count() / (double)pageSize);
            double totalInherentRisk = 0.0;
            foreach (var inherentRisk in riskIdentifications)
            {
                totalInherentRisk += inherentRisk.InherentRiskRating;
            }
            ViewBag.totalInherentRisk = (riskIdentifications.Count() > 1) ? (Math.Round((double)totalInherentRisk / riskIdentifications.Count(), 2)) : riskIdentifications.Count();

            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

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
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", getCompany.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name");
            ViewBag.FunctionId = new SelectList(function, "Id", "Name");
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
                    //var existingIdentification = await _riskIdentificaionRepository.FindByAsync(r => r.CompanyId == riskIdentification.CompanyId
                    //    && r.DepartmentId == riskIdentification.DepartmentId);

                    //if (existingIdentification == null)
                    //{
                    await _riskIdentificaionRepository.CreateAsync(riskIdentification);

                    // Assuming the repository returns the saved entity with its ID
                    var savedRiskIdentification = await _riskIdentificaionRepository.FindByAsync(riskIdentification.Id);

                    return Ok(new { id = savedRiskIdentification.Id });
                    //}
                    //else
                    //{
                    //    return Conflict(new { error = "DuplicateData", message = "Risk identification already exists." });
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal Server Error");
            }

            return BadRequest("Invalid ModelState");
        }

        [HttpPost]
        public async Task<IActionResult> Edit_AddWithLink(RiskIdentification riskIdentification)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            try
            {
                if (ModelState.IsValid)
                {
                    // var existingIdentification = await _riskIdentificaionRepository.FindByAsync(r => r.CompanyId == riskIdentification.CompanyId
                    //&& r.DepartmentId == riskIdentification.DepartmentId);
                    // if (existingIdentification == null)
                    // {
                    await _riskIdentificaionRepository.UpdateAsync(riskIdentification);

                    // Assuming the repository returns the saved entity with its ID
                    var savedRiskIdentification = await _riskIdentificaionRepository.FindByAsync(riskIdentification.Id);

                    return Ok(new { id = savedRiskIdentification.Id });
                    //}
                    //else
                    //{
                    //    return Conflict(new { error = "DuplicateData", message = "Risk identification already exists." });
                    //}
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
            ViewBag.FunctionId = new SelectList(function, "Id", "Name", riskIdentification.FunctionId);
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
        public JsonResult GetFunctionsByDepartment(int departmentId)
        {
            var functions = _functionRepository.ListAsync(
                new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false, D => D.DepartmentId == departmentId },
                q => q.OrderBy(u => u.Id),
                null).Result;

            var functionList = functions.Select(f => new { Id = f.Id, Name = f.Name }).ToList();

            return Json(functionList);
        }

        [HttpGet]
        public IActionResult FilterRiskIdentificationTable(int CompanyId, int departmentId, int page = 1, int pageSize = 10)
        {
            var query = from Identification in db.RiskIdentifications
                        join company in db.Companies on Identification.CompanyId equals company.Id
                        join department in db.Departments on Identification.DepartmentId equals department.Id
                        join function in db.Functions on Identification.FunctionId equals function.Id
                        join riskCategory in db.RiskCategories on Identification.RiskCategoryId equals riskCategory.Id
                        join riskImpact in db.RiskImpacts on Identification.RiskImpactId equals riskImpact.Id
                        join riskLikelihood in db.RiskLikehoods on Identification.RiskLikelihoodId equals riskLikelihood.Id
                        where
                        Identification.IsDeleted == false &&
                        company.IsDeleted == false && department.IsDeleted == false && riskCategory.IsDeleted == false && riskImpact.IsDeleted == false &&
                        riskLikelihood.IsDeleted == false
                        orderby company.Code,
                                department.Code,
                                Identification.Code
                        select new RiskIdentificationVM
                        {
                            Company = company,
                            Department = department,
                            Function = function,
                            RiskIdentification = Identification,
                            RiskCategory = riskCategory,
                            RiskImpact = riskImpact,
                            RiskLikehood = riskLikelihood
                        };

            if (CompanyId != 0)
            {
                query = query.Where(f => f.RiskIdentification.Company.Id == CompanyId);
            }

            if (departmentId != 0)
            {
                query = query.Where(f => f.RiskIdentification.Department.Id == departmentId && f.RiskIdentification.Company.Id == CompanyId);
            }

            var riskIdentification = query.OrderBy(c => c.RiskIdentification.Code)
                                .ToList();

            //var model = riskIdentification.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //int totalPages = (int)Math.Ceiling(totalRow / (double)pageSize);
            int totalRow = riskIdentification.Count();
            double totalInherentRisk = 0.0;
            foreach (var inherentRisk in query)
            {
                totalInherentRisk += inherentRisk.RiskIdentification.InherentRiskRating;
            }
            var result = new
            {
                Data = riskIdentification.Select(a => new
                {
                    id = a.RiskIdentification.Id,
                    riskTitle = a.RiskIdentification.Name,
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
                    riskCategoryBGColor = a.RiskCategory.BGColor,
                    riskCategoryFontColor = a.RiskCategory.FontColor,
                    riskCategoryName = a.RiskCategory.Name,
                    riskIdentificationDescription = a.RiskIdentification.Description,
                    riskIdentificationRiskRatingRationalization = a.RiskIdentification.RiskRatingRationalization,
                    riskImpactRate = a.RiskImpact.Rate,
                    riskImpactName = a.RiskImpact.Name,
                    riskLikehoodRate = a.RiskLikehood.Rate,
                    riskLikehoodName = a.RiskLikehood.Name,
                    inherentRiskRating = a.RiskIdentification.InherentRiskRating,
                    inherentRiskStatus = a.RiskIdentification.InherentRiskStatus,
                }).ToList(),
                TotalRow = totalRow,
                CurrentPage = page,
                totalInherentRisk = (totalInherentRisk > 1) ? (totalInherentRisk / totalRow) : 0
                //PageSize = (pageSize >= totalPages)?totalPages: pageSize,
                //TotalPages = totalPages
            };


            return Json(result);
        }
    }
}
