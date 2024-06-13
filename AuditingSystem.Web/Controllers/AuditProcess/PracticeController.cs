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
    public class PracticeController : Controller
    {
        private readonly IBaseRepository<Tasks, int> _tasksRepository;
        private readonly IBaseRepository<Practice, int> _practiceRepository;
        public PracticeController(
            IBaseRepository<Tasks, int> tasksRepository,
            IBaseRepository<Practice, int> practiceRepository)
        {
            _tasksRepository = tasksRepository;
            _practiceRepository = practiceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var practices = await _practiceRepository.ListAsync(
                new Expression<Func<Practice, bool>>[] { u => u.IsDeleted == false },
                /*c => c.Task.Code != null, c => c.Task.Objective.Code != null, c => c.Task.Objective.Activity.Code != null, c => c.Task.Objective.Activity.Function.Code != null,
                    c => c.Task.Objective.Activity.Function.Department.Code != null,
                  c => c.Task.Objective.Activity.Function.Department.Company.Code != null*/
                q => q.OrderBy(u => u.Code),
                c => c.Task, c => c.Task.Objective, c => c.Task.Objective.Activity, c => c.Task.Objective.Activity.Function, c => c.Task.Objective.Activity.Function.Department,
                  c => c.Task.Objective.Activity.Function.Department.Company, c => c.Task.Practices);


            //string apiurl = "https://onyx3.azurewebsites.net/practices/GetAllpractices";
            //using (HttpClient client = new HttpClient())
            //{
            //    string requestBody = "{}";
            //    StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            //    HttpResponseMessage response = await client.PostAsync(apiurl, content);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        string responseBody = await response.Content.ReadAsStringAsync();

            //        JObject json = JObject.Parse(responseBody);

            //        // Create APIPractice entities and add them to the repository
            //        foreach (var item in json["items"])
            //        {
            //            int id = (int)item["id"];
            //            string name = (string)item["practiceName"];
            //            int? taskId = item["taskId"]?.ToObject<int?>() ?? null;
            //            string code = (string)item["code"];

            //            // Check if the APIPractice with the given id already exists
            //            var existingApiPractice = await _practiceRepository.FindByAsync(id);

            //            if (existingApiPractice == null)
            //            {
            //                // Check if taskId is not null
            //                if (taskId != null)
            //                {
            //                    // Check if there is a record in the Task table with the same taskId
            //                    var existingTask = await _tasksRepository.FindByAsync(taskId.Value);

            //                    if (existingTask == null)
            //                    {
            //                        // Create a new APIPractice and add it to the repository
            //                        var newApiPractice = new Practice
            //                        {
            //                            Id = id,
            //                            Name = name,
            //                            Code = code,
            //                            TaskId = null,
            //                            Source = "API",
            //                            CreatedBy = "Admin",
            //                            CreationDate = DateTime.Now,
            //                            UpdatedBy = "Admin",
            //                            UpdatedDate = DateTime.Now,
            //                            IsDeleted = false
            //                        };
            //                        await _practiceRepository.CreateAsync(newApiPractice);
            //                    }
            //                    else
            //                    {
            //                        // Log or handle the case where the associated Task doesn't exist
            //                    }
            //                }
            //                else
            //                {
            //                    // Create a new APIPractice without a TaskId and add it to the repository
            //                    var newApiPractice = new Practice
            //                    {
            //                        Id = id,
            //                        Name = name,
            //                        Code = code,
            //                        TaskId = taskId,
            //                        Source = "API",
            //                        CreatedBy = "Admin",
            //                        CreationDate = DateTime.Now,
            //                        UpdatedBy = "Admin",
            //                        UpdatedDate = DateTime.Now,
            //                        IsDeleted = false
            //                    };
            //                    await _practiceRepository.CreateAsync(newApiPractice);
            //                }
            //            }
            //        }
            //    }
            //}
            return View(practices);
        }


        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var task = _tasksRepository.ListAsync(
                  new Expression<Func<Tasks, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.TaskId = new SelectList(task, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var practice = await _practiceRepository.FindByAsync(id);

            var task = _tasksRepository.ListAsync(
                  new Expression<Func<Tasks, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.TaskId = new SelectList(task, "Id", "Name", practice.TaskId);

            return View(practice);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var practice = await _practiceRepository.FindByAsync(id);

            var task = _tasksRepository.ListAsync(
                  new Expression<Func<Tasks, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.TaskId = new SelectList(task, "Id", "Name", practice.TaskId);

            return View(practice);
        }

        [HttpGet]
        public async Task<IActionResult> GetPracticesByTaskId(int taskId)
        {
            var items = await _practiceRepository.ListAsync(
                new Expression<Func<Practice, bool>>[] { u => u.IsDeleted == false, o => o.TaskId == taskId },
                q => q.OrderBy(u => u.Code),
                c => c.Task, c => c.Task.Objective, c => c.Task.Objective.Activity, c => c.Task.Objective.Activity.Function, c => c.Task.Objective.Activity.Function.Department,
                c => c.Task.Objective.Activity.Function.Department.Company, c => c.Task.Practices);

            return View(items);
        }
    }
}
