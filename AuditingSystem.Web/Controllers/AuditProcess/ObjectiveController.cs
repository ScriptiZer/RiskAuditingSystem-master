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
        private readonly IBaseRepository<Activity, int> _activityRepository;
        private readonly IBaseRepository<Objective, int> _objectiveRepository;
        public ObjectiveController(
            IBaseRepository<Activity, int> activityRepository,
            IBaseRepository<Objective, int> objectiveRepository)
        {
            _activityRepository = activityRepository;
            _objectiveRepository = objectiveRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var objectives = await _objectiveRepository.ListAsync(
                  new Expression<Func<Objective, bool>>[] { u => u.IsDeleted == false, a=>a.Activity.Name !=null },
                  q => q.OrderBy(u => u.Activity.Code),
                  c => c.Activity, c=>c.Activity.Function, c => c.Activity.Function.Department, c => c.Activity.Function.Department.Company, c=>c.Tasks);

            string apiurl = "https://onyx3.azurewebsites.net/objectives/Getallobjectives";
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

                    // Create APIObjective entities and add them to the repository
                    foreach (var item in json["items"])
                    {
                        int id = (int)item["id"];
                        string name = (string)item["objectiveName"];
                        int? activityId = item["activityId"]?.ToObject<int?>() ?? null;
                        string code = (string)item["code"];

                        // Check if the APIObjective with the given id already exists
                        var existingApiObjective = await _objectiveRepository.FindByAsync(id);
                        if (existingApiObjective == null)
                        {
                            // Check if activityId is not null
                            if (activityId != null)
                            {
                                // Check if there is a record in the Activity table with the same id
                                var existingActivity = await _activityRepository.FindByAsync(activityId.Value);

                                // Create a new APIObjective and add it to the repository
                                var newApiObjective = new Objective
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    ActivityId = existingActivity != null ? existingActivity.Id : null,
                                    Source = "API",
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _objectiveRepository.CreateAsync(newApiObjective);
                            }
                            else
                            {
                                // Create a new APIObjective without an ActivityId and add it to the repository
                                var newApiObjective = new Objective
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    ActivityId = activityId,
                                    Source = "API",
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _objectiveRepository.CreateAsync(newApiObjective);
                            }
                        }
                    }
                }
            }
            return View(objectives);
        }

        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var activity = _activityRepository.ListAsync(
                  new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.ActivityId = new SelectList(activity, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var objective = await _objectiveRepository.FindByAsync(id);

            var activity = _activityRepository.ListAsync(
                  new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.ActivityId = new SelectList(activity, "Id", "Name", objective.ActivityId);

            return View(objective);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var objective = await _objectiveRepository.FindByAsync(id);

            var activity = _activityRepository.ListAsync(
                  new Expression<Func<Activity, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

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
    }
}
