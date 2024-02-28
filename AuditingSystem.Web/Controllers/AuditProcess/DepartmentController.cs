using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class DepartmentController : Controller
    {
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        public DepartmentController(
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository, 
            IBaseRepository<User, int> userRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var departments = await _departmentRepository.ListAsync(
                new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Company.Code),
                c => c.Company, f=>f.Functions);

            string apiurl = "https://onyx3.azurewebsites.net/departments/GetAlldepartments";
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

                    // Create APIDepartment entities and add them to the repository
                    foreach (var item in json["items"])
                    {
                        int id = (int)item["id"];
                        string name = (string)item["departmentName"];
                        int? companyId = item["companyId"]?.ToObject<int?>() ?? null;
                        string code = (string)item["code"];

                        // Check if the APIDepartment with the given id already exists
                        var existingApiDepartment = await _departmentRepository.FindByAsync(id);
                        if (existingApiDepartment == null)
                        {
                            // Check if companyId is not null
                            if (companyId != null)
                            {
                                // Check if there is a record in the Company table with the same id
                                var existingCompany = await _companyRepository.FindByAsync(companyId.Value);

                                // Create a new APIDepartment and add it to the repository
                                var newApiDepartment = new Department
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    Source = "API",
                                    CompanyId = existingCompany != null ? existingCompany.Id : null,
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _departmentRepository.CreateAsync(newApiDepartment);
                            }
                            else
                            {
                                // Create a new APIDepartment without a CompanyId and add it to the repository
                                var newApiDepartment = new Department
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    Source = "API",
                                    CompanyId = companyId,
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };
                                await _departmentRepository.CreateAsync(newApiDepartment);
                            }
                        }
                    }
                }
            }

            return View(departments);
        }


        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var company = _companyRepository.ListAsync(
                  new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            var head = _userRepository.ListAsync(
                  new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.CompanyId = new SelectList(company, "Id", "Name");
            ViewBag.Head = new SelectList(head, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var department = await _departmentRepository.FindByAsync(id);

            var company = _companyRepository.ListAsync(
                  new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            var head = _userRepository.ListAsync(
                  new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", department.CompanyId);
            ViewBag.Head = new SelectList(head, "Id", "Name", department.Head);

            return View(department);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var department = await _departmentRepository.FindByAsync(id);

            var company = _companyRepository.ListAsync(
                  new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.CompanyId = new SelectList(company, "Id", "Name", department.CompanyId);

            return View(department);
        }

    }
}
