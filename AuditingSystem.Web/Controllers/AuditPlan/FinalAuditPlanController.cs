using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.AuditPlan
{
    public class FinalAuditPlanController : Controller
    {
        //private readonly IBaseRepository<FinalAuditPlan, int> _finalRepository;
        //private readonly AuditingSystemDbContext _db;
        //private readonly IBaseRepository<User, int> _userRepository;
        //private readonly IBaseRepository<Department, int> _departmentRepository;

        //public FinalAuditPlanController(IBaseRepository<FinalAuditPlan, int> finalRepository,
        //    AuditingSystemDbContext db, IBaseRepository<User, int> userRepository, IBaseRepository<Department, int> departmentRepository)
        //{
        //    _finalRepository = finalRepository;
        //    _db = db;
        //    _userRepository = userRepository;
        //    _departmentRepository = departmentRepository;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        var userId = HttpContext.Session.GetInt32("UserId");

        //        if (userId == null)
        //            return RedirectToAction("Login", "Account");

        //        var user = await _userRepository.FindByAsync(Convert.ToInt32(userId));

        //        var list = await _finalRepository.ListAsync(
        //            new Expression<Func<FinalAuditPlan, bool>>[] { u => u.IsDeleted == false, c => c.CompanyId == user.CompanyId },
        //            q => q.OrderBy(u => u.Id),
        //            c => c.Company,
        //            d => d.Department,
        //            f => f.Function);

        //        //var groupedData = list.GroupBy(d => d.Department.Name)
        //        //      .Select(g => new GroupedDraftAuditPlanViewModel
        //        //      {
        //        //          DepartmentName = g.Key,
        //        //          DraftAuditPlans = g.ToList()
        //        //      })
        //        //      .ToList();

        //        var userGroupedData = GetDraftPlanData();
        //        ViewBag.DraftGroupedData = userGroupedData;



        //        var companiesWithYearsAndQuarters = await _db.Companies
        //            .Include(c => c.Years).ThenInclude(y => y.Quarters)
        //            .Where(u => u.IsDeleted == false)
        //            .OrderBy(u => u.Id)
        //            .ToListAsync();

        //        var yearQuarterDictionary = new Dictionary<string, List<YearQuarter>>();

        //        foreach (var company in companiesWithYearsAndQuarters)
        //        {
        //            foreach (var year in company.Years.OrderBy(y => y.Name).TakeLast(3))
        //            {
        //                var quarters = year.Quarter.Split(',').ToList();
        //                var yearQuarters = new List<YearQuarter>();
        //                foreach (var quarter in quarters)
        //                {
        //                    yearQuarters.Add(new YearQuarter { YearId = year.Id, Quarter = quarter });
        //                }
        //                yearQuarterDictionary.Add(year.Name, yearQuarters);
        //            }
        //        }

        //        ViewBag.yearQuarterDictionary = yearQuarterDictionary;

        //        return View(list);
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Error", "Home");
        //    }
        //}

        //public async Task<IActionResult> Archives(string departmentName)
        //{
        //    try
        //    {
        //        var userId = HttpContext.Session.GetInt32("UserId");

        //        if (userId == null)
        //            return RedirectToAction("Login", "Account");

        //        var user = await _userRepository.FindByAsync(Convert.ToInt32(userId));
        //        var department = await _departmentRepository.FindByAsync(d => d.Name == departmentName);

        //        var list = await _finalRepository.ListAsync(
        //            new Expression<Func<FinalAuditPlan, bool>>[] { u => u.IsDeleted == false, c => c.CompanyId == user.CompanyId,
        //            d=>d.DepartmentId == department.Id},
        //            q => q.OrderBy(u => u.Id),
        //            c => c.Company,
        //            d => d.Department,
        //            f => f.Function);

        //        var groupedData = list.GroupBy(d => d.Department.Name)
        //              .Select(g => new GroupedDraftAuditPlanViewModel
        //              {
        //                  DepartmentName = g.Key,
        //                  DraftAuditPlans = g.ToList()
        //              })
        //              .ToList();

        //        var userGroupedData = GetDraftPlanData();
        //        ViewBag.DraftGroupedData = userGroupedData;



        //        var companiesWithYearsAndQuarters = await _db.Companies
        //            .Include(c => c.Years).ThenInclude(y => y.Quarters)
        //            .Where(u => u.IsDeleted == false)
        //            .OrderBy(u => u.Id)
        //            .ToListAsync();

        //        var yearQuarterDictionary = new Dictionary<string, List<YearQuarter>>();

        //        foreach (var company in companiesWithYearsAndQuarters)
        //        {
        //            foreach (var year in company.Years)
        //            {
        //                var quarters = year.Quarter.Split(',').ToList();
        //                var yearQuarters = new List<YearQuarter>();
        //                foreach (var quarter in quarters)
        //                {
        //                    yearQuarters.Add(new YearQuarter { YearId = year.Id, Quarter = quarter });
        //                }
        //                yearQuarterDictionary.Add(year.Name, yearQuarters);
        //            }
        //        }

        //        ViewBag.yearQuarterDictionary = yearQuarterDictionary;

        //        return View(groupedData);
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Error", "Home");
        //    }
        //}
        //private List<dynamic> GetDraftPlanData()
        //{
        //    var auditDraftPlanData = _db.FinalAuditPlanLists
        //        .GroupBy(item => new { item.FinalAuditPlanId, item.Year, item.Quarter })
        //        .Select(group => new
        //        {
        //            DraftAuditPlanId = group.Key.FinalAuditPlanId,
        //            Year = group.Key.Year,
        //            Quarter = group.Key.Quarter,
        //            Plan = group.Sum(item => item.Plan),
        //            Actual = group.Sum(item => item.Actual)
        //        })
        //        .ToList<dynamic>();

        //    return auditDraftPlanData;
        //}
        private readonly IBaseRepository<FinalAuditPlan, int> _finalRepository;
        private readonly AuditingSystemDbContext _db;
        private readonly IBaseRepository<User, int> _userRepository;

        public FinalAuditPlanController(IBaseRepository<FinalAuditPlan, int> finalRepository, AuditingSystemDbContext db,
            IBaseRepository<User, int> userRepository)
        {
            _finalRepository = finalRepository;
            _db = db;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                    return RedirectToAction("Login", "Account");

                var user = await _userRepository.FindByAsync(Convert.ToInt32(userId));
                var list = await _finalRepository.ListAsync(
                    new Expression<Func<FinalAuditPlan, bool>>[] { u => u.IsDeleted == false, c => c.CompanyId == user.CompanyId },
                    q => q.OrderBy(u => u.Id),
                    c => c.Company,
                    d => d.Department,
                    f => f.Function);

                var groupedData = list.GroupBy(d => d.Department.Name)
                      .Select(g => new GroupedFinalAuditPlanViewModel
                      {
                          DepartmentName = g.Key,
                          FinalAuditPlans = g.ToList()
                      })
                      .ToList();

                var userGroupedData = GetFinalPlanData();
                ViewBag.FinalGroupedData = userGroupedData;

                var companiesWithYearsAndQuarters = await _db.Companies
                    .Include(c => c.Years).ThenInclude(y => y.Quarters)
                    .Where(u => u.IsDeleted == false)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                var yearQuarterDictionary = new Dictionary<string, List<YearQuarter>>();

                foreach (var company in companiesWithYearsAndQuarters)
                {
                    foreach (var year in company.Years.OrderBy(y => y.Name).TakeLast(3))
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

                return View(groupedData);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        private List<dynamic> GetFinalPlanData()
        {
            var auditFinalPlanData = _db.FinalAuditPlanLists
                .GroupBy(item => new { item.FinalAuditPlanId, item.Year, item.Quarter })
                .Select(group => new
                {
                    FinalAuditPlanId = group.Key.FinalAuditPlanId,
                    Year = group.Key.Year,
                    Quarter = group.Key.Quarter,
                    Plan = group.Sum(item => item.Plan),
                    Actual = group.Sum(item => item.Actual)
                })   
                .ToList<dynamic>();

            return auditFinalPlanData;
        }
    }
}
