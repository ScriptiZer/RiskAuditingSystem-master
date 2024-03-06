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
    public class FunctionController : Controller
    {
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly AuditingSystemDbContext db;
        public FunctionController(
            IBaseRepository<Function, int> functionRepository,
            IBaseRepository<Department, int> departmentRepository,
            AuditingSystemDbContext db)
        {
            _functionRepository = functionRepository;
            _departmentRepository = departmentRepository;
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            //var functions = await _functionRepository.ListAsync(
            //      new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
            //      q => q.OrderBy(u => u.Department.Code),
            //      c => c.Department, c=>c.Department.Company, a=>a.Activities);
            var functions = db.Functions.Include(i => i.Department).Include(d => d.Activities)
                .ThenInclude(a => a.Objectives).ThenInclude(o => o.Tasks).ThenInclude(p => p.Practices);

            string apiurl = "https://onyx3.azurewebsites.net/functions/GetAllfunctions";
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

                    // Create APIFunction entities and add them to the repository
                    foreach (var item in json["items"])
                    {
                        int id = (int)item["id"];
                        string name = (string)item["functionName"];
                        int? departmentId = item["departmentId"]?.ToObject<int?>() ?? null;
                        string code = (string)item["code"];

                        // Check if the APIFunction with the given id already exists
                        var existingApiFunction = await _functionRepository.FindByAsync(id);
                        if (existingApiFunction == null)
                        {
                            // Check if departmentId is not null
                            if (departmentId != null)
                            {
                                // Check if there is a record in the Department table with the same id
                                var existingDepartment = await _departmentRepository.FindByAsync(departmentId.Value);

                                // Create a new APIFunction and add it to the repository
                                var newApiFunction = new Function
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    Source = "API",
                                    DepartmentId = existingDepartment != null ? existingDepartment.Id : null,
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _functionRepository.CreateAsync(newApiFunction);
                            }
                            else
                            {
                                // Create a new APIFunction without a DepartmentId and add it to the repository
                                var newApiFunction = new Function
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    Source = "API",
                                    DepartmentId = departmentId,
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _functionRepository.CreateAsync(newApiFunction);
                            }
                        }
                    }
                }
            }
            return View(functions);
        }


        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var department = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.DepartmentId = new SelectList(department, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var function = await _functionRepository.FindByAsync(id);

            var department = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", function.DepartmentId);

            return View(function);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var function = await _functionRepository.FindByAsync(id);

            var department = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", function.DepartmentId);

            return View(function);
        }

        [HttpGet]
        public async Task<IActionResult> GetFunctionsByDepartmentId(int departmentId)
        {

            var items = await _functionRepository.ListAsync(
              new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false, f => f.DepartmentId == departmentId },
              q => q.OrderBy(u => u.Department.Code),
              c => c.Department, c => c.Department.Company, a => a.Activities);

            return View(items);
        }

    }
}
