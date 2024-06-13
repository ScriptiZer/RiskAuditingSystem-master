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
        private readonly IBaseRepository<Industry, int> _industryRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly AuditingSystemDbContext db;
        public FunctionController(
            IBaseRepository<Function, int> functionRepository,
            IBaseRepository<Department, int> departmentRepository,
            AuditingSystemDbContext db,
            IBaseRepository<Industry,int> industryRepository,
            IBaseRepository<Company,int> companyRepository)
        {
            _functionRepository = functionRepository;
            _departmentRepository = departmentRepository;
            this.db = db;
            _industryRepository = industryRepository;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var functions = await db.Functions.Include(d=>d.Department).ThenInclude(c => c.Company).Include(a=>a.Activities).
                OrderBy(c=>c.Company.Code).ThenBy(d=>d.Department.Code).ThenBy(f=>f.Code).Where(f=>f.IsDeleted == false).ToListAsync();

            ViewBag.CompanyId = new SelectList(db.Companies.Where(c => c.IsDeleted == false), "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments.Where(c => c.IsDeleted == false), "Id", "Name");

            //string apiurl = "https://onyx3.azurewebsites.net/functions/GetAllfunctions";
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

            //        // Create APIFunction entities and add them to the repository
            //        foreach (var item in json["items"])
            //        {
            //            int id = (int)item["id"];
            //            string name = (string)item["functionName"];
            //            int? departmentId = item["departmentId"]?.ToObject<int?>() ?? null;
            //            string code = (string)item["code"];

            //            // Check if the APIFunction with the given id already exists
            //            var existingApiFunction = await _functionRepository.FindByAsync(id);
            //            if (existingApiFunction == null)
            //            {
            //                // Check if departmentId is not null
            //                if (departmentId != null)
            //                {
            //                    // Check if there is a record in the Department table with the same id
            //                    var existingDepartment = await _departmentRepository.FindByAsync(departmentId.Value);

            //                    // Create a new APIFunction and add it to the repository
            //                    var newApiFunction = new Function
            //                    {
            //                        Id = id,
            //                        Name = name,
            //                        Code = code,
            //                        Source = "API",
            //                        DepartmentId = existingDepartment != null ? existingDepartment.Id : null,
            //                        CreatedBy = "Admin",
            //                        CreationDate = DateTime.Now,
            //                        UpdatedBy = "Admin",
            //                        UpdatedDate = DateTime.Now,
            //                        IsDeleted = false
            //                    };
            //                    await _functionRepository.CreateAsync(newApiFunction);
            //                }
            //                else
            //                {
            //                    // Create a new APIFunction without a DepartmentId and add it to the repository
            //                    var newApiFunction = new Function
            //                    {
            //                        Id = id,
            //                        Name = name,
            //                        Code = code,
            //                        Source = "API",
            //                        DepartmentId = departmentId,
            //                        CreatedBy = "Admin",
            //                        CreationDate = DateTime.Now,
            //                        UpdatedBy = "Admin",
            //                        UpdatedDate = DateTime.Now,
            //                        IsDeleted = false
            //                    };
            //                    await _functionRepository.CreateAsync(newApiFunction);
            //                }
            //            }
            //        }
            //    }
            //}
            return View(functions);
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

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name");
            ViewBag.CompanyId = new SelectList(company, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var function = await _functionRepository.FindByAsync(id);

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

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", function.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", function.CompanyId);
            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", function.DepartmentId);

            return View(function);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var function = await _functionRepository.FindByAsync(id);

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

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", function.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", function.CompanyId);
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
        public IActionResult FilterFuncTable(int companyId, int departmentId)
        {
            IQueryable<Function> query = db.Functions.Include(d => d.Department).ThenInclude(c => c.Company).Include(a => a.Activities);

            if (companyId != 0)
            {
                query = query.Where(f => f.Department.Company.Id == companyId);
            }

            if (departmentId != 0)
            {
                query = query.Where(f => f.DepartmentId == departmentId && f.Department.Company.Id == companyId);
            }

            var functions = query.OrderBy(c => c.Company.Code)
                                .ThenBy(d => d.Department.Code)
                                .ThenBy(f => f.Code)
                                .ToList();

            var result = functions.Select(f => new
            {
                id = f.Id,
                companyCode = f.Department.Company.Code,
                companyId = f.Department.Company.Id,
                departmentId = f.Department.Id,
                departmentCode = f.Department.Code,
                code = f.Code,
                name = f.Name,
                activities = f.Activities.Select(a => new
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
