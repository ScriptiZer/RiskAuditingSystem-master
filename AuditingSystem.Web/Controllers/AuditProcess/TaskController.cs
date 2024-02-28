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
        private readonly IBaseRepository<Tasks, int> _tasksRepository;
        private readonly IBaseRepository<Objective, int> _objectiveRepository;
        public TaskController(
            IBaseRepository<Tasks, int> tasksRepository,
            IBaseRepository<Objective, int> objectiveRepository)
        {
            _tasksRepository = tasksRepository;
            _objectiveRepository = objectiveRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var tasks = await _tasksRepository.ListAsync(
                  new Expression<Func<Tasks, bool>>[] { c => c.IsDeleted == false,
                      c => c.Objective.Code != null,c => c.Objective.Activity.Code != null, c => c.Objective.Activity.Function.Code != null, c => c.Objective.Activity.Function.Department.Code != null,
                  c => c.Objective.Activity.Function.Department.Company.Code != null },
                  q => q.OrderBy(u => u.Objective.Code),
                  c => c.Objective,c => c.Objective.Activity, c => c.Objective.Activity.Function, c => c.Objective.Activity.Function.Department, 
                  c => c.Objective.Activity.Function.Department.Company, c => c.Practices);

            string apiurl = "https://onyx3.azurewebsites.net/tasks/GetAlltasks";
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

                    // Create APITask entities and add them to the repository
                    foreach (var item in json["items"])
                    {
                        int id = (int)item["id"];
                        string name = (string)item["taskName"];
                        int? objectiveId = item["objectiveId"]?.ToObject<int?>() ?? null;
                        string code = (string)item["code"];

                        // Check if the APITask with the given id already exists
                        var existingApiTask = await _tasksRepository.FindByAsync(id);
                        if (existingApiTask == null)
                        {
                            // Check if objectiveId is not null
                            if (objectiveId != null)
                            {
                                // Check if there is a record in the Objective table with the same id
                                var existingObjective = await _objectiveRepository.FindByAsync(objectiveId.Value);

                                // Create a new APITask and add it to the repository
                                var newApiTask = new Tasks
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    ObjectiveId = existingObjective != null ? existingObjective.Id : null,
                                    Source = "API",
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _tasksRepository.CreateAsync(newApiTask);
                            }
                            else
                            {
                                // Create a new APITask without an ObjectiveId and add it to the repository
                                var newApiTask = new Tasks
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    ObjectiveId = objectiveId,
                                    Source = "API",
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _tasksRepository.CreateAsync(newApiTask);
                            }
                        }
                    }
                }
            }

            return View(tasks);
        }

        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var objective = _objectiveRepository.ListAsync(
                  new Expression<Func<Objective, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.ObjectiveId = new SelectList(objective, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var task = await _tasksRepository.FindByAsync(id);

            var objective = _objectiveRepository.ListAsync(
                  new Expression<Func<Objective, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.ObjectiveId = new SelectList(objective, "Id", "Name", task.ObjectiveId);

            return View(task);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var task = await _tasksRepository.FindByAsync(id);

            var objective = _objectiveRepository.ListAsync(
                  new Expression<Func<Objective, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

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
    }
}
