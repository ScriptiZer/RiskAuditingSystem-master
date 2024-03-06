using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class ActivityController : Controller
    {
        private readonly IBaseRepository<Activity, int> _activityRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly AuditingSystemDbContext db;
        public ActivityController(
            IBaseRepository<Activity, int> activityRepository,
            IBaseRepository<Function, int> functionRepository, AuditingSystemDbContext db)
        {
            _activityRepository = activityRepository;
            _functionRepository = functionRepository;
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            //var activities = await _activityRepository.ListAsync(
            //    new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false, u=>u.Code !=null,
            //    c => c.Function!=null, c=>c.Function.Department.Company!=null, c => c.Function.Department!=null, c => c.Objectives!=null},
            //    q => q.OrderBy(u => u.Function.Code),
            //    c=>c.Function, c=> c.Function.Department, c=>c.Function.Department.Company, c=>c.Objectives);

            //      var activities = await _activityRepository.ListAsync(
            //new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false, c => c.Function != null, c => c.Function.Department.Code != null, c => c.Function.Department.Company.Code != null },
            //q => q.OrderBy(u => u.Function.Code), c => c.Function, c => c.Function.Department, c => c.Function.Department.Company, c => c.Objectives);
            var activities = db.Activities.Include(i => i.Function).Include(d => d.Objectives).ThenInclude(o => o.Tasks).ThenInclude(p => p.Practices);
            string apiurl = "https://onyx3.azurewebsites.net/activities/GetAllactivities";
            using (HttpClient client = new HttpClient())
            {
                string requestBody = "{}";
                StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiurl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse JSON response using JObject
                    JObject json = JObject.Parse(responseBody);

                    // Create APIIndustry entities and add them to the repository
                    foreach (var item in json["items"])
                    {
                        int id = (int)item["id"];
                        string name = (string)item["activityName"];
                        int? functionId = item["functionId"]?.ToObject<int?>() ?? null;
                        string code = (string)item["code"];

                        // Check if the APIIndustry with the given id already exists
                        var existingApiActivity = await _activityRepository.FindByAsync(id);

                        if (existingApiActivity == null)
                        {
                            // Check if functionId is not null
                            if (functionId != null)
                            {
                                // Check if there is a record in the Function table with the same id
                                var existingFunction = await _functionRepository.FindByAsync(functionId.Value);

                                // Create a new APIIndustry and add it to the repository
                                var newApiactivity = new Activity
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    FunctionId = existingFunction != null ? existingFunction.Id : null,
                                    Source = "API",
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _activityRepository.CreateAsync(newApiactivity);
                            }
                            else
                            {
                                // Create a new APIIndustry without a FunctionId and add it to the repository
                                var newApiactivity = new Activity
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    FunctionId = null,
                                    Source = "API",
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _activityRepository.CreateAsync(newApiactivity);
                            }
                        }
                        //else
                        //{
                        //    // Update existing APIActivity and set the LastApiUpdateDate
                        //    existingApiActivity.Name = name;
                        //    existingApiActivity.Code = code;
                        //    existingApiActivity.FunctionId = existingFunction != null ? existingFunction.Id : null;
                        //    existingApiActivity.UpdatedBy = "Admin";
                        //    existingApiActivity.UpdatedDate = DateTime.Now;

                        //    await _activityRepository.UpdateAsync(existingApiActivity);
                        //}
                    }

                }
            }
            return View(activities);
        }


        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var function = _functionRepository.ListAsync(
                  new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.FunctionId = new SelectList(function, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var activity = await _activityRepository.FindByAsync(id);

            var function = _functionRepository.ListAsync(
                  new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.FunctionId = new SelectList(function, "Id", "Name", activity.FunctionId);

            return View(activity);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var activity = await _activityRepository.FindByAsync(id);

            var function = _functionRepository.ListAsync(
                  new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

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
    }
}
