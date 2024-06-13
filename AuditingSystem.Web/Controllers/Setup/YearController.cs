using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Controllers.AuditPlan;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class YearController : Controller
    {
        private readonly IBaseRepository<Year, int> _yearRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Quarter, int> _quarterRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly IBaseRepository<DraftAuditPlan, int> _draftRepository;
        private readonly IBaseRepository<DraftAuditPlanList, int> _draftListRepository;
        private readonly AuditingSystemDbContext _db;

        public YearController(
            IBaseRepository<Year, int> yearRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<Quarter, int> quarterRepository,
            IBaseRepository<User, int> userRepository,
            IBaseRepository<DraftAuditPlan, int> draftRepository,
            AuditingSystemDbContext db,
            IBaseRepository<DraftAuditPlanList, int> draftListRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _yearRepository = yearRepository;
            _quarterRepository = quarterRepository;
            _userRepository = userRepository;
            _draftRepository = draftRepository;
            _db = db;
            _draftListRepository = draftListRepository;

        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetString("UserId");
            

            if (userId != null) { 
                var year = await _yearRepository.ListAsync(
                      new Expression<Func<Year, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Name),
                      /*u => u.Company, u => u.Department*/ null);

                var model = year.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.TotalRow = year.Count();
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(year.Count() / (double)pageSize);

                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            ViewBag.UserId = userId;
            ViewBag.Company = getCompany.CompanyId;

            if(userId != null) { 
                var quarter = await _quarterRepository.ListAsync(
                  new Expression<Func<Quarter, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null);

                ViewBag.Quarter = new SelectList(quarter, "Id", "Name");

                return View();
            }
            return RedirectToAction("Login", "Account");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");
            var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            ViewBag.Company = getCompany.CompanyId;

            if (userId != null)
            {
                var year = await _yearRepository.FindByAsync(id);

                var quarter = await _quarterRepository.ListAsync(
                 new Expression<Func<Quarter, bool>>[] { u => u.IsDeleted == false },
                 q => q.OrderBy(u => u.Id),
                 null);

                ViewBag.Quarter = new SelectList(quarter, "Id", "Name", year?.Quarter?.Split(','));

                return View(year);
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");
            var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            ViewBag.Company = getCompany.CompanyId;

            if (userId != null)
            {
                var year = await _yearRepository.FindByAsync(id);

                var quarter = await _quarterRepository.ListAsync(
                 new Expression<Func<Quarter, bool>>[] { u => u.IsDeleted == false },
                 q => q.OrderBy(u => u.Id),
                 null);

                ViewBag.Quarter = new SelectList(quarter, "Id", "Name", year?.Quarter?.Split(','));

                return View(year);
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> GetFinalAuditperYear(string getYear)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                    return RedirectToAction("Login", "Account");

                var user = await _userRepository.FindByAsync(Convert.ToInt32(userId));

                //var list = await _draftRepository.ListAsync(
                //    new Expression<Func<DraftAuditPlan, bool>>[] { u => u.IsDeleted == false, c => c.CompanyId == user.CompanyId },
                //    q => q.OrderBy(u => u.Id),
                //    c => c.Company,
                //    d => d.Department,
                //    f => f.Function,
                //    dr => dr.DraftAuditPlanLists);

                var list = await _draftListRepository.ListAsync(
                    new Expression<Func<DraftAuditPlanList, bool>>[] { u => u.IsDeleted == false, c => c.DraftAuditPlan.CompanyId == user.CompanyId, y=>y.Year == getYear, a=>a.Actual > 0 },
                    q => q.OrderBy(u => u.Id),
                    c => c.DraftAuditPlan,
                    d => d.DraftAuditPlan.Company,
                    f => f.DraftAuditPlan.Department,
                    dr => dr.DraftAuditPlan.Function);

                ViewBag.getYear = getYear;

                var companiesWithYearsAndQuarters = await _db.Companies
                    .Include(c => c.Years).ThenInclude(y => y.Quarters)
                    .Where(u => u.IsDeleted == false)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                var yearQuarterDictionary = new Dictionary<string, List<YearQuarter>>();

                foreach (var company in companiesWithYearsAndQuarters)
                {
                    foreach (var year in company.Years.Where(y=>y.Name == getYear))
                    {
                        var quarters = year.Quarter.Split(',').ToList();
                        var yearQuarters = new List<YearQuarter>();
                        foreach (var quarter in quarters)
                        {
                            yearQuarters.Add(new YearQuarter { YearId = year.Id, Quarter = quarter });
                        }
                        yearQuarterDictionary.Add(year.Name, yearQuarters);
                    }
                }

                ViewBag.yearQuarterDictionary = yearQuarterDictionary;

                return View(list);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
       
    }
}
