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
    public class TaskController : Controller
    {
        private readonly IBaseRepository<Industry, int> _industryRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly IBaseRepository<Activity, int> _activityRepository;
        private readonly IBaseRepository<Objective, int> _objectiveRepository;
        private readonly IBaseRepository<Tasks, int> _tasksRepository;

        private readonly AuditingSystemDbContext db;
        public TaskController(
            IBaseRepository<Tasks, int> tasksRepository,
            IBaseRepository<Objective, int> objectiveRepository,
            AuditingSystemDbContext db, 
            IBaseRepository<Industry, int> industryRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<Function, int> functionRepository,
            IBaseRepository<Activity, int> activityRepository )
        {
            _tasksRepository = tasksRepository;
            _industryRepository = industryRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _functionRepository = functionRepository;
            _activityRepository = activityRepository;
            _objectiveRepository = objectiveRepository;
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var tasks = await db.Tasks.Include(t=>t.Objective).ThenInclude(a => a.Activity).ThenInclude(f => f.Function)
                .ThenInclude(d => d.Department).ThenInclude(c => c.Company).Include(d => d.Practices)
                .Where(f =>
                f.Objective.Code != null &&
                f.Objective.Activity.Code != null &&
                f.Objective.Activity.Function.Code != null &&
                f.Objective.Activity.Function.Department.Code != null &&
                f.Objective.Activity.Function.Department.Company.Code != null &&
                f.IsDeleted == false)
                .OrderBy(a => 
                a.Objective.Activity.Function.Department.Company.Code)
                .ThenBy(a => a.Objective.Activity.Function.Department.Code)
                .ThenBy(a => a.Objective.Activity.Function.Code)
                .ThenBy(a => a.Objective.Activity.Code)
                .ThenBy(o=>o.Objective.Code)
                .ThenBy(a => a.Code)
                .ToListAsync();
            //.OrderBy(i => i.Industry).ThenBy(c => c.Company).ThenBy(d => d.Department).ThenBy(f => f.Function).ThenBy(a => a.Activity).ThenBy(o=>o.Objective).ThenBy(t => t.Code);

            //string apiurl = "https://onyx3.azurewebsites.net/tasks/GetAlltasks";
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

            //        // Create APITask entities and add them to the repository
            //        foreach (var item in json["items"])
            //        {
            //            int id = (int)item["id"];
            //            string name = (string)item["taskName"];
            //            int? objectiveId = item["objectiveId"]?.ToObject<int?>() ?? null;
            //            string code = (string)item["code"];

            //            // Check if the APITask with the given id already exists
            //            var existingApiTask = await _tasksRepository.FindByAsync(id);
            //            if (existingApiTask == null)
            //            {
            //                // Check if objectiveId is not null
            //                if (objectiveId != null)
            //                {
            //                    // Check if there is a record in the Objective table with the same id
            //                    var existingObjective = await _objectiveRepository.FindByAsync(objectiveId.Value);

            //                    // Create a new APITask and add it to the repository
            //                    var newApiTask = new Tasks
            //                    {
            //                        Id = id,
            //                        Name = name,
            //                        Code = code,
            //                        ObjectiveId = existingObjective != null ? existingObjective.Id : null,
            //                        Source = "API",
            //                        CreatedBy = "Admin",
            //                        CreationDate = DateTime.Now,
            //                        UpdatedBy = "Admin",
            //                        UpdatedDate = DateTime.Now,
            //                        IsDeleted = false
            //                    };
            //                    await _tasksRepository.CreateAsync(newApiTask);
            //                }
            //                else
            //                {
            //                    // Create a new APITask without an ObjectiveId and add it to the repository
            //                    var newApiTask = new Tasks
            //                    {
            //                        Id = id,
            //                        Name = name,
            //                        Code = code,
            //                        ObjectiveId = objectiveId,
            //                        Source = "API",
            //                        CreatedBy = "Admin",
            //                        CreationDate = DateTime.Now,
            //                        UpdatedBy = "Admin",
            //                        UpdatedDate = DateTime.Now,
            //                        IsDeleted = false
            //                    };
            //                    await _tasksRepository.CreateAsync(newApiTask);
            //                }
            //            }
            //        }
            //    }
            //}

            return View(tasks);
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

            var objective = _objectiveRepository.ListAsync(
                  new Expression<Func<Objective, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name");
            ViewBag.CompanyId = new SelectList(company, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name");
            ViewBag.FunctionId = new SelectList(function, "Id", "Name");
            ViewBag.ActivityId = new SelectList(activity, "Id", "Name");
            ViewBag.ObjectiveId = new SelectList(objective, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var task = await _tasksRepository.FindByAsync(id);

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
            var objective = _objectiveRepository.ListAsync(
                  new Expression<Func<Objective, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", task.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", task.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", task.DepartmentId);
            ViewBag.FunctionId = new SelectList(function, "Id", "Name", task.FunctionId);
            ViewBag.ActivityId = new SelectList(activity, "Id", "Name", task.ActivityId);
            ViewBag.ObjectiveId = new SelectList(objective, "Id", "Name", task.ObjectiveId);

            return View(task);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var task = await _tasksRepository.FindByAsync(id);

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
            var objective = _objectiveRepository.ListAsync(
                  new Expression<Func<Objective, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", task.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", task.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", task.DepartmentId);
            ViewBag.FunctionId = new SelectList(function, "Id", "Name", task.FunctionId);
            ViewBag.ActivityId = new SelectList(activity, "Id", "Name", task.ActivityId);
            ViewBag.ObjectiveId = new SelectList(objective, "Id", "Name", task.ObjectiveId);

            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksByObjectiveId(int objectiveId)
        {
            var items = await _tasksRepository.ListAsync(
              new Expression<Func<Tasks, bool>>[] { c => c.IsDeleted == false,o => o.ObjectiveId == objectiveId,
              c => c.Objective.Code != null,c => c.Objective.Activity.Code != null, c => c.Objective.Activity.Function.Code != null, c => c.Objective.Activity.Function.Department.Code != null,
              c => c.Objective.Activity.Function.Department.Company.Code != null }, q => q.OrderBy(u => u.Objective.Code),
              c => c.Objective, c => c.Objective.Activity, c => c.Objective.Activity.Function, c => c.Objective.Activity.Function.Department,
              c => c.Objective.Activity.Function.Department.Company, c => c.Practices);

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
        public JsonResult GetObjectivesByActivity(int activityId)
        {

            var objectives = _objectiveRepository.ListAsync(
                new Expression<Func<Objective, bool>>[] { u => u.IsDeleted == false, f => f.ActivityId == activityId },
                q => q.OrderBy(u => u.Id),
                null).Result;

            var objectiveList = objectives.Select(d => new { Id = d.Id, Name = d.Name }).ToList();

            return Json(objectiveList);
        }
    }
}
