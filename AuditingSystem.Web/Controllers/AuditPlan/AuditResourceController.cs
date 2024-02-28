using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.AuditPlan
{
    class YearQuarter
    {
        public int YearId { get; set; }
        public string Quarter { get; set; }
        public List<Monthes> Months { get; set; }
    }

    public class AuditResourceController : Controller
    {
        private readonly IBaseRepository<AuditResources, int> _auditResources;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly IBaseRepository<Year, int> _yearRepository;
        private readonly IBaseRepository<Quarter, int> _quarterRepository;
        
        private readonly ISplitRepository<int> _splitRepository;
        private readonly AuditingSystemDbContext _db;
        private readonly IMemoryCache _memoryCache;

        public AuditResourceController(
            IBaseRepository<AuditResources, int> auditResources,
            AuditingSystemDbContext db,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<Function, int> functionRepository,
            IBaseRepository<User, int> userRepository,
            IBaseRepository<Year, int> yearRepository,
            IBaseRepository<Quarter, int> quarterRepository,
            ISplitRepository<int> splitRepository,
            IMemoryCache memoryCache)
        {
            _auditResources = auditResources;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _functionRepository = functionRepository;
            _userRepository = userRepository;
            _yearRepository = yearRepository;
            _quarterRepository = quarterRepository;
            _splitRepository = splitRepository;
            _db = db;
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                    return RedirectToAction("Login", "Account");

                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));


                var list = await _auditResources.ListAsync(
                    new Expression<Func<AuditResources, bool>>[] { u => u.IsDeleted == false, a=>a.CompanyId == getCompany.CompanyId},
                    q => q.OrderBy(u => u.Id),
                    c => c.Company,
                    c => c.AuditResourcesList);

                var companyViewModelList = list.Select(auditResource => new AuditProcessVM
                {
                    ResourceId = auditResource.Id,
                    CompanyName = auditResource.Company?.Name,
                    DepartmentName = GetDepartmentName(auditResource.DepartmentId),
                    Functions = GetFunctionNames(auditResource.FunctionId),
                    PlanStartDate = auditResource.PlanStartDate,
                    PlanEndDate = auditResource.PlanEndDate,
                    Description= auditResource.Description
                }).ToList();

                var groupedData = companyViewModelList.GroupBy(auditResource => auditResource.DepartmentName);

                ViewBag.DepartmentGroups = groupedData;

                var userGroupedData = GetAuditResourceData();
                ViewBag.UserGroupedData = userGroupedData;

                var startEndDateGroupedData = GetAuditResourceStartEndDateData();
                ViewBag.StartEndDateGroupedData = startEndDateGroupedData;

                var companyIds = getCompany.CompanyId;//list.Select(a => a.CompanyId).Distinct().ToList();
                var companiesWithYearsAndQuarters = await _db.Companies
                    .Include(c => c.Years).ThenInclude(y => y.Quarters)
                    .Where(u => u.IsDeleted == false)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                var companiesWithInAndOutsources = await _db.Companies
                    .Include(c => c.User)
                    .Where(u => u.IsDeleted == false && u.Id == companyIds)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                if (companiesWithInAndOutsources.Count() > 0)
                {
                    var company = companiesWithInAndOutsources.First();
                    var insourceIds = _splitRepository.SplitData(company.Insources);
                    var outsourceIds = _splitRepository.SplitData(company.Outsources);
                    var managerUsers = _splitRepository.SplitData(company.Manager);

                    if (insourceIds.Any())
                    {
                        var insourceUsers = await _db.Users
                            .Where(u => insourceIds.Contains(u.Id))
                            .ToListAsync();
                        var outsourceUsers = await _db.Users
                            .Where(u => outsourceIds.Contains(u.Id))
                            .ToListAsync();
                        var ManagerUsers = await _db.Users
                            .Where(u => managerUsers.Contains(u.Id))
                            .ToListAsync();
                        ViewBag.InsourceUsers = insourceUsers;
                        ViewBag.outsourceUsers = outsourceUsers;
                        ViewBag.ManagerUsers = ManagerUsers;
                    }
                }

                var yearQuarterDictionary = new Dictionary<string, List<YearQuarter>>();

                foreach (var company in companiesWithYearsAndQuarters)
                {
                    foreach (var year in company.Years.TakeLast(3))
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

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Archives(string departmentName)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                    return RedirectToAction("Login", "Account");

                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));
                var department = await _departmentRepository.FindByAsync(d => d.Name == departmentName);

                var list = await _auditResources.ListAsync(
                    new Expression<Func<AuditResources, bool>>[] { u => u.IsDeleted == false, a => a.CompanyId == getCompany.CompanyId,
                        d=>Convert.ToInt32(d.DepartmentId) == department.Id},
                    q => q.OrderBy(u => u.Id),
                    c => c.Company,
                    c => c.AuditResourcesList);

                var companyViewModelList = list.Select(auditResource => new AuditProcessVM
                {
                    ResourceId = auditResource.Id,
                    CompanyName = auditResource.Company?.Name,
                    DepartmentName = GetDepartmentName(auditResource.DepartmentId),
                    Functions = GetFunctionNames(auditResource.FunctionId),
                    PlanStartDate = auditResource.PlanStartDate,
                    PlanEndDate = auditResource.PlanEndDate,
                    Description = auditResource.Description
                }).ToList();

                var groupedData = companyViewModelList.GroupBy(auditResource => auditResource.DepartmentName);

                ViewBag.DepartmentGroups = groupedData;

                var userGroupedData = GetAuditResourceData();
                ViewBag.UserGroupedData = userGroupedData;

                var startEndDateGroupedData = GetAuditResourceStartEndDateData();
                ViewBag.StartEndDateGroupedData = startEndDateGroupedData;

                var companyIds = getCompany.CompanyId;//list.Select(a => a.CompanyId).Distinct().ToList();
                var companiesWithYearsAndQuarters = await _db.Companies
                    .Include(c => c.Years).ThenInclude(y => y.Quarters)
                    .Where(u => u.IsDeleted == false)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                var companiesWithInAndOutsources = await _db.Companies
                    .Include(c => c.User)
                    .Where(u => u.IsDeleted == false && u.Id == companyIds)
                    .OrderBy(u => u.Id)
                    .ToListAsync();

                if (companiesWithInAndOutsources.Count() > 0)
                {
                    var company = companiesWithInAndOutsources.First();
                    var insourceIds = _splitRepository.SplitData(company.Insources);
                    var outsourceIds = _splitRepository.SplitData(company.Outsources);
                    var managerUsers = _splitRepository.SplitData(company.Manager);

                    if (insourceIds.Any())
                    {
                        var insourceUsers = await _db.Users
                            .Where(u => insourceIds.Contains(u.Id))
                            .ToListAsync();
                        var outsourceUsers = await _db.Users
                            .Where(u => outsourceIds.Contains(u.Id))
                            .ToListAsync();
                        var ManagerUsers = await _db.Users
                            .Where(u => managerUsers.Contains(u.Id))
                            .ToListAsync();
                        ViewBag.InsourceUsers = insourceUsers;
                        ViewBag.outsourceUsers = outsourceUsers;
                        ViewBag.ManagerUsers = ManagerUsers;
                    }
                }

                var yearQuarterDictionary = new Dictionary<string, List<YearQuarter>>();

                foreach (var company in companiesWithYearsAndQuarters)
                {
                    foreach (var year in company.Years)
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

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        private List<dynamic> GetAuditResourceData()
        {
            var auditResourceData = _db.AuditResourcesLists
                .GroupBy(item => new { item.AuditResourceId, item.UserId, item.ResourceType })
                .Select(group => new
                {
                    AuditResourceId = group.Key.AuditResourceId,
                    UserId = group.Key.UserId,
                    ResourceType = group.Key.ResourceType,
                    PlannedDays = group.Sum(item => item.Plan),
                    ActualDays = group.Sum(item => item.Actual)
                })
                .ToList<dynamic>();
           

            return auditResourceData;
        }

        private List<dynamic> GetAuditResourceStartEndDateData()
        {
            var auditResourceStartEndDateData = _db.AuditResourcesListStartEndDates
                .GroupBy(item => new { item.AuditResourceId, item.YearId, item.QuarterId })
                .Select(group => new
                {
                    AuditResourceId = group.Key.AuditResourceId,
                    YearId = group.Key.YearId,
                    QuarterId = group.Key.QuarterId,
                    //PlanStartDate = group.Min(item => item.PlanStartDate),
                    //PlanEndDate = group.Max(item => item.PlanEndDate),
                    ActualStartDate = group.Min(item => item.ActualStartDate),
                    ActualEndDate = group.Max(item => item.ActualEndDate),
                    AssignedToStartActualId = group.Min(item => item.AssignedToStartActualId),
                    AssignedToEndActualId = group.Max(item => item.AssignedToEndActualId)
                })
                .ToList<dynamic>();

            var user = _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null).Result;

            ViewBag.AssignedToStartActual = new SelectList(user, "Id", "Name");
            ViewBag.AssignedToEndActual = new SelectList(user, "Id", "Name");

            return auditResourceStartEndDateData;
        }



        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (userId != null)
            {
                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));
                var companies = _companyRepository.ListAsync(
                      new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false, C => C.Id == getCompany.CompanyId },
                      q => q.OrderBy(u => u.Id),
                      null).Result;
                var departments = _departmentRepository.ListAsync(
                      new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null).Result;
                var functions = _functionRepository.ListAsync(
                      new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null).Result;                 

                ViewBag.CompanyId = new SelectList(companies, "Id", "Name");
                ViewBag.DepartmentId = new SelectList(departments, "Id", "Name");
                ViewBag.FunctionId = new SelectList(functions, "Id", "Name");
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (userId != null)
            {
                var auditResource =await _auditResources.FindByAsync(id);
                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));
                var companies = _companyRepository.ListAsync(
                      new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false, C => C.Id == getCompany.CompanyId },
                      q => q.OrderBy(u => u.Id),
                      null).Result;
                var departments = _departmentRepository.ListAsync(
                      new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null).Result;
                var functions = _functionRepository.ListAsync(
                      new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null).Result;

                ViewBag.CompanyId = new SelectList(companies, "Id", "Name", auditResource.CompanyId);
                ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", auditResource.DepartmentId);
                ViewBag.FunctionId = new SelectList(functions, "Id", "Name", auditResource.FunctionId);
                return View(auditResource);
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (userId != null)
            {
                var auditResource = await _auditResources.FindByAsync(id);
                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));
                var companies = _companyRepository.ListAsync(
                      new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false, C => C.Id == getCompany.CompanyId },
                      q => q.OrderBy(u => u.Id),
                      null).Result;
                var departments = _departmentRepository.ListAsync(
                      new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null).Result;
                var functions = _functionRepository.ListAsync(
                      new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null).Result;

                ViewBag.CompanyId = new SelectList(companies, "Id", "Name", auditResource.CompanyId);
                ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", auditResource.DepartmentId);
                ViewBag.FunctionId = new SelectList(functions, "Id", "Name", auditResource.FunctionId);
                return View(auditResource);
            }
            return RedirectToAction("Login", "Account");
        }

        private string GetDepartmentName(string departmentId)
        {
            if (int.TryParse(departmentId, out int id))
            {
                var department = _db.Departments.FirstOrDefault(d => d.Id == id);
                return department?.Name;
            }

            return string.Empty;
        }

        private List<string> GetFunctionNames(string functionIds)
        {
            if (!_memoryCache.TryGetValue(functionIds, out List<string> functionNames))
            {
                functionNames = new List<string>();

                if (!string.IsNullOrEmpty(functionIds))
                {
                    var ids = functionIds.Split(',').Select(int.Parse).ToList();
                    functionNames = _db.Functions.Where(f => ids.Contains(f.Id)).Select(f => f.Name).ToList();
                }

                _memoryCache.Set(functionIds, functionNames, TimeSpan.FromMinutes(10)); 
            }

            return functionNames;
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
                new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false, D => D.DepartmentId == departmentId },
                q => q.OrderBy(u => u.Id),
                null).Result;

            var functionList = functions.Select(f => new { Id = f.Id, Name = f.Name }).ToList();

            return Json(functionList);
        }

    }
}
