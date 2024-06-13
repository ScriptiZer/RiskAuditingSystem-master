using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    //public class test
    //{
    //    public Activity Activity { get; set; }
    //    public Function Function { get; set; }
    //    public Department Department { get; set; }
    //    public Com MyProperty { get; set; }
    //}
    public class ActivityController : Controller
    {
        private readonly IBaseRepository<Activity, int> _activityRepository;
        private readonly IBaseRepository<Industry, int> _industryRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly AuditingSystemDbContext db;
        public ActivityController(
            IBaseRepository<Activity, int> activityRepository,
            IBaseRepository<Function, int> functionRepository, AuditingSystemDbContext db,
            IBaseRepository<Industry, int> industryRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository)
        {
            _activityRepository = activityRepository;
            _functionRepository = functionRepository;
            this.db = db;
            _industryRepository = industryRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
        }

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            //var activities = db.Activities
            //    .Include(i => i.Function).
            //    OrderBy(x => x.Company.Code).ThenBy(c => c.Department.Code).ThenBy(a => a.Function.Code).ThenBy(x => x.Code);

            var activities = await db.Activities.Include(f=>f.Function)
                .ThenInclude(d=>d.Department).ThenInclude(c=>c.Company).Include(d=>d.Objectives)
                .Where(f=>f.Function.Code != null &&
                f.Function.Department.Code != null && 
                f.Function.Department.Company.Code != null &&
                f.IsDeleted == false)
                .OrderBy(a=>a.Function.Department.Company.Code).ThenBy(a => a.Function.Department.Code).ThenBy(a => a.Function.Code).ThenBy(a => a.Code)
                .ToListAsync();

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments.Where(c => c.IsDeleted == false), "Id", "Name");
            ViewBag.FunctionId = new SelectList(db.Functions.Where(c => c.IsDeleted == false), "Id", "Name");

            //var activities = await (from a in db.Activities 
            //                 join f in db.Functions on a.FunctionId equals f.Id
            //                 select new test { Activity = a, Function = f}).ToListAsync();

            //var activities = await _activityRepository.ListAsync(
            //    new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false },
            //    q => q.OrderBy(u => u.Id),
            //    u => u.Function);

            //string apiurl = "https://onyx3.azurewebsites.net/activities/GetAllactivities";
            //using (HttpClient client = new HttpClient())
            //{
            //    string requestBody = "{}";
            //    StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            //    HttpResponseMessage response = await client.PostAsync(apiurl, content);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        string responseBody = await response.Content.ReadAsStringAsync();

            //        // Parse JSON response using JObject
            //        JObject json = JObject.Parse(responseBody);

            //        // Create APIIndustry entities and add them to the repository
            //        foreach (var item in json["items"])
            //        {
            //            int id = (int)item["id"];
            //            string name = (string)item["activityName"];
            //            int? functionId = item["functionId"]?.ToObject<int?>() ?? null;
            //            string code = (string)item["code"];

            //            // Check if the APIIndustry with the given id already exists
            //            var existingApiActivity = await _activityRepository.FindByAsync(id);

            //            if (existingApiActivity == null)
            //            {
            //                // Check if functionId is not null
            //                if (functionId != null)
            //                {
            //                    // Check if there is a record in the Function table with the same id
            //                    var existingFunction = await _functionRepository.FindByAsync(functionId.Value);

            //                    // Create a new APIIndustry and add it to the repository
            //                    var newApiactivity = new Activity
            //                    {
            //                        Id = id,
            //                        Name = name,
            //                        Code = code,
            //                        FunctionId = existingFunction != null ? existingFunction.Id : null,
            //                        Source = "API",
            //                        CreatedBy = "Admin",
            //                        CreationDate = DateTime.Now,
            //                        UpdatedBy = "Admin",
            //                        UpdatedDate = DateTime.Now,
            //                        IsDeleted = false
            //                    };
            //                    await _activityRepository.CreateAsync(newApiactivity);
            //                }
            //                else
            //                {
            //                    // Create a new APIIndustry without a FunctionId and add it to the repository
            //                    var newApiactivity = new Activity
            //                    {
            //                        Id = id,
            //                        Name = name,
            //                        Code = code,
            //                        FunctionId = null,
            //                        Source = "API",
            //                        CreatedBy = "Admin",
            //                        CreationDate = DateTime.Now,
            //                        UpdatedBy = "Admin",
            //                        UpdatedDate = DateTime.Now,
            //                        IsDeleted = false
            //                    };
            //                    await _activityRepository.CreateAsync(newApiactivity);
            //                }
            //            }
            //            //else
            //            //{
            //            //    // Update existing APIActivity and set the LastApiUpdateDate
            //            //    existingApiActivity.Name = name;
            //            //    existingApiActivity.Code = code;
            //            //    existingApiActivity.FunctionId = existingFunction != null ? existingFunction.Id : null;
            //            //    existingApiActivity.UpdatedBy = "Admin";
            //            //    existingApiActivity.UpdatedDate = DateTime.Now;

            //            //    await _activityRepository.UpdateAsync(existingApiActivity);
            //            //}
            //        }

            //    }
            //}
            return View(activities);
        }


        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

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

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name");
            ViewBag.CompanyId = new SelectList(company, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name");
            ViewBag.FunctionId = new SelectList(function, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var activity = await _activityRepository.FindByAsync(id);

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

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", activity.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", activity.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", activity.DepartmentId);
            ViewBag.FunctionId = new SelectList(function, "Id", "Name", activity.FunctionId);

            return View(activity);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var activity = await _activityRepository.FindByAsync(id);

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

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", activity.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", activity.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", activity.DepartmentId);
            ViewBag.FunctionId = new SelectList(function, "Id", "Name", activity.FunctionId);


            return View(activity);
        }

        [HttpGet]
        public async Task<IActionResult> GetActivitiesByFunctionId(int functionId)
        {
            var items = await _activityRepository.ListAsync(
                new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false, c => c.Function != null, a => a.FunctionId == functionId, c => c.Function.Department.Code != null, c => c.Function.Department.Company.Code != null },
                q => q.OrderBy(u => u.Function.Code), c => c.Function, c => c.Function.Department, c => c.Function.Department.Company, c => c.Objectives);

            return View(items);
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
                new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false, f => f.DepartmentId == departmentId },
                q => q.OrderBy(u => u.Id),
                null).Result;

            var functionList = functions.Select(d => new { Id = d.Id, Name = d.Name }).ToList();

            return Json(functionList);
        }

        [HttpGet]
        public IActionResult FilterActTable(int companyId, int departmentId, int functionId)
        {
            IQueryable<Activity> query = db.Activities
                                            .Include(f => f.Function)
                                                .ThenInclude(d => d.Department)
                                                    .ThenInclude(c => c.Company)
                                            .Include(d => d.Objectives)
                                            .Where(f => f.Function.Code != null &&
                                                        f.Function.Department.Code != null &&
                                                        f.Function.Department.Company.Code != null &&
                                                        f.IsDeleted == false);

            if (companyId != 0)
            {
                query = query.Where(f => f.Function.Department.Company.Id == companyId);
            }

            if (departmentId != 0)
            {
                query = query.Where(f => f.Function.DepartmentId == departmentId && f.Function.Department.Company.Id == companyId);
            }

            if (functionId != 0)
            {
                query = query.Where(f => f.FunctionId == functionId &&
                                            f.Function.DepartmentId == departmentId &&
                                            f.Function.Department.Company.Id == companyId);
            }

            var activities = query.OrderBy(a => a.Function.Department.Company.Code)
                                  .ThenBy(a => a.Function.Department.Code)
                                  .ThenBy(a => a.Function.Code)
                                  .ThenBy(a => a.Code)
                                  .ToList();

            var result = activities.Select(f => new
            {
                id = f.Id,
                companyCode = f.Function.Department.Company.Code,
                companyId = f.Function.Department.Company.Id,
                departmentId = f.Function.Department.Id,
                departmentCode = f.Function.Department.Code,
                functionId = f.Function.Id,
                functionCode = f.Function.Code,
                code = f.Code,
                name = f.Name,
                objectives = f.Objectives.Select(a => new
                {
                    id = a.Id,
                    code = a.Code,
                    name = a.Name
                }).ToList()
            });

            return Json(result);
        }

    }
}
