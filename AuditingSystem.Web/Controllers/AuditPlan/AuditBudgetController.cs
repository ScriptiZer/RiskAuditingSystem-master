using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuditingSystem.Web.Controllers.AuditPlan
{
    public class AuditBudgetController : Controller
    {
        private readonly AuditingSystemDbContext db;
        private readonly IBaseRepository<User, int> _userRepository;

        public AuditBudgetController(AuditingSystemDbContext db,
            IBaseRepository<User, int> userRepository)
        {
            this.db = db;
            this._userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                    return RedirectToAction("Login", "Account");

                var user = await _userRepository.FindByAsync(Convert.ToInt32(userId));

                var companiesWithYearsAndQuarters = await db.Companies
                    .Include(c => c.Years).ThenInclude(y => y.Quarters)
                    .Where(u => u.IsDeleted == false && u.Id == user.CompanyId)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                var budgetGroupedData = GetAuditBudgetData();
                ViewData["BudgetGroupedData"] = budgetGroupedData;

                var yearQuarterDictionary = new Dictionary<string, List<YearQuarter>>();

                foreach (var company in companiesWithYearsAndQuarters)
                {
                    var budgetPlanYearIds = company.BudgetPlanYear.Split(',').Select(int.Parse);

                    foreach (var budgetPlanYearId in budgetPlanYearIds)
                    {
                        var year = await db.Years.FirstOrDefaultAsync(y => y.Id == budgetPlanYearId);

                        if (year != null)
                        {
                            var quarters = year.Quarter.Split(',').ToList();
                            var yearQuarters = quarters.Select(quarter => new YearQuarter { YearId = year.Id, Quarter = quarter }).ToList();

                            yearQuarterDictionary.Add(year.Name, yearQuarters);
                        }
                    }
                }

                ViewData["YearQuarterDictionary"] = yearQuarterDictionary;

                var list = await (
                    from b in db.AuditBudgets
                    join u in db.Users on b.ResourceId equals u.Id
                    join c in db.Companies on b.CompanyId equals c.Id
                    where c.Id == user.CompanyId // modify
                    select new GetBudgetListVM { AuditBudget = b, Company = c, User = u })
                    .AsNoTracking().ToListAsync();

                var groupedByResourceType = list.GroupBy(item => item.AuditBudget.ResourceType);
                ViewData["GroupedByResourceType"] = groupedByResourceType;

                return View(list);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Archives(string resourceType)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                    return RedirectToAction("Login", "Account");

                var user = await _userRepository.FindByAsync(Convert.ToInt32(userId));

                var companiesWithYearsAndQuarters = await db.Companies
                    .Include(c => c.Years).ThenInclude(y => y.Quarters)
                    .Where(u => u.IsDeleted == false && u.Id == user.CompanyId)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                var budgetGroupedData = GetAuditBudgetData();
                ViewData["BudgetGroupedData"] = budgetGroupedData;

                var yearQuarterDictionary = new Dictionary<string, List<YearQuarter>>();

                foreach (var company in companiesWithYearsAndQuarters)
                {
                    var budgetPlanYearIds = company.BudgetPlanYear.Split(',').Select(int.Parse);

                    foreach (var budgetPlanYearId in budgetPlanYearIds)
                    {
                        var year = await db.Years.FirstOrDefaultAsync(y => y.Id == budgetPlanYearId);

                        if (year != null)
                        {
                            var quarters = year.Quarter.Split(',').ToList();
                            var yearQuarters = quarters.Select(quarter => new YearQuarter { YearId = year.Id, Quarter = quarter }).ToList();

                            yearQuarterDictionary.Add(year.Name, yearQuarters);
                        }
                    }
                }

                ViewData["YearQuarterDictionary"] = yearQuarterDictionary;

                var list = await (
                    from b in db.AuditBudgets
                    join u in db.Users on b.ResourceId equals u.Id
                    join c in db.Companies on b.CompanyId equals c.Id
                    where c.Id == user.CompanyId && b.ResourceType == resourceType // modify
                    select new GetBudgetListVM { AuditBudget = b, Company = c, User = u })
                    .AsNoTracking().ToListAsync();

                var groupedByResourceType = list.GroupBy(item => item.AuditBudget.ResourceType);
                ViewData["GroupedByResourceType"] = groupedByResourceType;

                return View(list);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        private List<dynamic> GetAuditBudgetData()
        {
            var auditBudgetData = db.AuditBudgetLists
                .GroupBy(item => new { item.BudgetId, item.Year, item.Quarter, item.Month })
                .Select(group => new
                {
                    BudgetId = group.Key.BudgetId,
                    Year = group.Key.Year,
                    Quarter = group.Key.Quarter,
                    Month = group.Key.Month,
                    Estimated = group.Sum(item => item.Estimated),
                    Actual = group.Sum(item => item.Actual)
                })
                .ToList<dynamic>();

            return auditBudgetData;
        }
    }

}
