using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class ObjectiveController : Controller
    {
        private readonly IBaseRepository<Industry, int> _industryRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly IBaseRepository<Activity, int> _activityRepository;
        private readonly IBaseRepository<Objective, int> _objectiveRepository;
        private readonly AuditingSystemDbContext db;
        public ObjectiveController(
            IBaseRepository<Activity, int> activityRepository,
            IBaseRepository<Objective, int> objectiveRepository, 
            IBaseRepository<Industry, int> industryRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<Function,int> functionRepository,
            AuditingSystemDbContext db)
        {
            _activityRepository = activityRepository;
            _objectiveRepository = objectiveRepository;
            this.db = db;
            _industryRepository = industryRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _functionRepository = functionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var objectives = await db.Objectives.Include(a=>a.Activity).ThenInclude(f => f.Function)
                .ThenInclude(d => d.Department).ThenInclude(c => c.Company).Include(d => d.Tasks)
                .Where(f =>f.Activity.Code != null &&
                f.Activity.Function.Code != null &&
                f.Activity.Function.Department.Code != null &&
                f.Activity.Function.Department.Company.Code != null &&
                f.IsDeleted == false)
                .OrderBy(a => a.Activity.Function.Department.Company.Code).ThenBy(a => a.Activity.Function.Department.Code)
                .ThenBy(a => a.Activity.Function.Code).ThenBy(a => a.Activity.Code).ThenBy(a=>a.Code)
                .ToListAsync();

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments.Where(c => c.IsDeleted == false), "Id", "Name");
            ViewBag.FunctionId = new SelectList(db.Functions.Where(c => c.IsDeleted == false), "Id", "Name");
            ViewBag.ActivityId = new SelectList(db.Activities.Where(c => c.IsDeleted == false), "Id", "Name");
            //.OrderBy(i => i.Industry).ThenBy(c => c.Company).ThenBy(d => d.Department).ThenBy(f => f.Function).ThenBy(a=>a.Activity).ThenBy(o => o.Code);

            //string apiurl = "https://onyx3.azurewebsites.net/objectives/Getallobjectives";
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

            //        // Create APIObjective entities and add them to the repository
            //        foreach (var item in json["items"])
            //        {
            //            int id = (int)item["id"];
            //            string name = (string)item["objectiveName"];
            //            int? activityId = item["activityId"]?.ToObject<int?>() ?? null;
            //            string code = (string)item["code"];

            //            // Check if the APIObjective with the given id already exists
            //            var existingApiObjective = await _objectiveRepository.FindByAsync(id);
            //            if (existingApiObjective == null)
            //            {
            //                // Check if activityId is not null
            //                if (activityId != null)
            //                {
            //                    // Check if there is a record in the Activity table with the same id
            //                    var existingActivity = await _activityRepository.FindByAsync(activityId.Value);

            //                    // Create a new APIObjective and add it to the repository
            //                    var newApiObjective = new Objective
            //                    {
            //                        Id = id,
            //                        Name = name,
            //                        Code = code,
            //                        ActivityId = existingActivity != null ? existingActivity.Id : null,
            //                        Source = "API",
            //                        CreatedBy = "Admin",
            //                        CreationDate = DateTime.Now,
            //                        UpdatedBy = "Admin",
            //                        UpdatedDate = DateTime.Now,
            //                        IsDeleted = false
            //                    };
            //                    await _objectiveRepository.CreateAsync(newApiObjective);
            //                }
            //                else
            //                {
            //                    // Create a new APIObjective without an ActivityId and add it to the repository
            //                    var newApiObjective = new Objective
            //                    {
            //                        Id = id,
            //                        Name = name,
            //                        Code = code,
            //                        ActivityId = activityId,
            //                        Source = "API",
            //                        CreatedBy = "Admin",
            //                        CreationDate = DateTime.Now,
            //                        UpdatedBy = "Admin",
            //                        UpdatedDate = DateTime.Now,
            //                        IsDeleted = false
            //                    };
            //                    await _objectiveRepository.CreateAsync(newApiObjective);
            //                }
            //            }
            //        }
            //    }
            //}
            return View(objectives);
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
            var activity = _activityRepository.ListAsync(
                  new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name");
            ViewBag.CompanyId = new SelectList(company, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name");
            ViewBag.FunctionId = new SelectList(function, "Id", "Name");
            ViewBag.ActivityId = new SelectList(activity, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var objective = await _objectiveRepository.FindByAsync(id);

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
            var activity = _activityRepository.ListAsync(
                  new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", objective.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", objective.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", objective.DepartmentId);
            ViewBag.FunctionId = new SelectList(function, "Id", "Name", objective.FunctionId);
            ViewBag.ActivityId = new SelectList(activity, "Id", "Name", objective.ActivityId);

            return View(objective);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var objective = await _objectiveRepository.FindByAsync(id);

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
            var activity = _activityRepository.ListAsync(
                  new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", objective.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", objective.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", objective.DepartmentId);
            ViewBag.FunctionId = new SelectList(function, "Id", "Name", objective.FunctionId);
            ViewBag.ActivityId = new SelectList(activity, "Id", "Name", objective.ActivityId);

            return View(objective);
        }

        [HttpGet]
        public async Task<IActionResult> GetObjectivesByActivityId(int activityId)
        {
            var items = await _objectiveRepository.ListAsync(
              new Expression<Func<Objective, bool>>[] { u => u.IsDeleted == false, a => a.Activity.Name != null, a => a.ActivityId == activityId },
              q => q.OrderBy(u => u.Activity.Code),
              c => c.Activity, c => c.Activity.Function, c => c.Activity.Function.Department, c => c.Activity.Function.Department.Company, c => c.Tasks);

            return Json(items);
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
        public JsonResult GetActivitiesByFunction(int functionId)
        {

            var activities = _activityRepository.ListAsync(
                new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false, f => f.FunctionId == functionId },
                q => q.OrderBy(u => u.Id),
                null).Result;

            var activityList = activities.Select(d => new { Id = d.Id, Name = d.Name }).ToList();

            return Json(activityList);
        }

        [HttpGet]
        public IActionResult FilterObjTable(int companyId, int departmentId, int functionId, int activityId)
        {
            IQueryable<Objective> query = db.Objectives
                .Include(a => a.Activity).ThenInclude(f => f.Function)
                .ThenInclude(d => d.Department).ThenInclude(c => c.Company)
                .Include(d => d.Tasks)
                .Where(f => f.Activity.Code != null &&
                            f.Activity.Function.Code != null &&
                            f.Activity.Function.Department.Code != null &&
                            f.Activity.Function.Department.Company.Code != null &&
                            f.IsDeleted == false);

            if (companyId != 0)
            {
                query = query.Where(f => f.Activity.Function.Department.Company.Id == companyId);
            }

            if (departmentId != 0)
            {
                query = query.Where(f => f.Activity.Function.DepartmentId == departmentId &&
                f.Activity.Function.Department.Company.Id == companyId);
            }

            if (functionId != 0)
            {
                query = query.Where(f => f.Activity.FunctionId == functionId &&
                f.Activity.Function.DepartmentId == departmentId &&
                f.Activity.Function.Department.Company.Id == companyId);
            }

            if (activityId != 0)
            {
                query = query.Where(f => f.Activity.Id == activityId &&
                f.Activity.FunctionId == functionId &&
                f.Activity.Function.DepartmentId == departmentId &&
                f.Activity.Function.Department.Company.Id == companyId);
            }

            var objectives = query
                .OrderBy(a => a.Activity.Function.Department.Company.Code)
                .ThenBy(a => a.Activity.Function.Department.Code)
                .ThenBy(a => a.Activity.Function.Code)
                .ThenBy(a => a.Activity.Code)
                .ThenBy(a => a.Code)
                .ToList();

            var result = objectives.Select(f => new
            {
                id = f.Id,
                companyCode = f.Function.Department.Company.Code,
                companyId = f.Function.Department.Company.Id,
                departmentId = f.Function.Department.Id,
                departmentCode = f.Function.Department.Code,
                functionId = f.Function.Id,
                functionCode = f.Function.Code,
                activityId = f.Activity.Id,
                activityCode = f.Activity.Code,
                code = f.Code,
                name = f.Name,
                tasks = f.Tasks.Select(a => new
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
