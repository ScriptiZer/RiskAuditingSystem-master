using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.AuditPlan
{
    public class AuditBudgetController : Controller
    {
        private readonly AuditingSystemDbContext db;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly IBaseRepository<OperationBudgetType, int> _operationBudgetType;

        public AuditBudgetController(AuditingSystemDbContext db,
            IBaseRepository<User, int> userRepository,
            IBaseRepository<OperationBudgetType, int> operationBudgetType)
        {
            this.db = db;
            this._userRepository = userRepository;
            _operationBudgetType = operationBudgetType;
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
                    join u in db.Users on b.ResourceId equals u.Id into userJoin 
                    from u in userJoin.DefaultIfEmpty() 
                    join c in db.Companies on b.CompanyId equals c.Id
                    where c.Id == user.CompanyId
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

        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");
            var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            var operationBudgetType = _operationBudgetType.ListAsync(
                  new Expression<Func<OperationBudgetType, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            var resources = _userRepository.ListAsync(
              new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;

            ViewBag.OperationBudgetTypeId = new SelectList(operationBudgetType, "Id", "Name");
            ViewBag.ResourceId = new SelectList(resources, "Id", "Name");
            ViewBag.CompanyId = getCompany.CompanyId;
            ViewBag.UserId = userId;
            return View();
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
