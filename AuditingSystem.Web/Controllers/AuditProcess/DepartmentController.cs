using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class DepartmentController : Controller
    {
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Industry, int> _industryRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly AuditingSystemDbContext _db;

        public DepartmentController(
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<User, int> userRepository,
            AuditingSystemDbContext db,
            IBaseRepository<Industry, int> industryRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _db = db;
            _industryRepository = industryRepository;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");


            var departments = await _db.Departments
                .Include(c => c.Company)
                .Include(f => f.Functions)
                .OrderBy(d => d.Company.Code)
                .ThenBy(d => d.Code)
                .Where(d=>d.IsDeleted == false)
                .ToListAsync();

            ViewBag.CompanyId = new SelectList(_db.Companies.Where(c=>c.IsDeleted == false), "Id","Name");

            //string apiUrl = "https://onyx3.azurewebsites.net/departments/GetAlldepartments";
            //using (var client = new HttpClient())
            //{
            //    var response = await client.GetAsync(apiUrl);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var responseBody = await response.Content.ReadAsStringAsync();
            //        var json = JObject.Parse(responseBody);
            //        foreach (var item in json["items"])
            //        {
            //            var id = (int)item["id"];
            //            var name = (string)item["departmentName"];
            //            var companyId = item["companyId"]?.ToObject<int?>();
            //            var code = (string)item["code"];

            //            var existingApiDepartment = await _departmentRepository.FindByAsync(id);
            //            if (existingApiDepartment == null)
            //            {
            //                var newApiDepartment = new Department
            //                {
            //                    Id = id,
            //                    Name = name,
            //                    Code = code,
            //                    Source = "API",
            //                    CompanyId = companyId,
            //                    CreatedBy = "Admin",
            //                    CreationDate = DateTime.Now,
            //                    UpdatedBy = "Admin",
            //                    UpdatedDate = DateTime.Now,
            //                    IsDeleted = false
            //                };
            //                await _departmentRepository.CreateAsync(newApiDepartment);
            //            }
            //        }
            //    }
            //}

            return View(departments);
        }

        [HttpGet]
        public IActionResult FilterDeptTablebyCompanyId(int companyId)
        {
            if(companyId != 0)
            {
                var departments = _db.Departments
                .Include(d => d.Company)
                .Include(d => d.Functions)
                .Where(d => d.CompanyId == companyId)
                .ToList();

                var result = departments.Select(d => new
                {
                    id = d.Id,
                    companyCode = d.Company.Code,
                    code = d.Code,
                    name = d.Name,
                    companyId = d.CompanyId,
                    functions = d.Functions.Select(f => new
                    {
                        id = f.Id,
                        code = f.Code,
                        name = f.Name
                    }).ToList()
                });

                return Json(result);
            }
            else
            {
                var departments = _db.Departments
                    .Include(d => d.Company)
                    .Include(d => d.Functions)
                    .ToList();

                var result = departments.Select(d => new
                {
                    id = d.Id,
                    companyCode = d.Company.Code,
                    code = d.Code,
                    name = d.Name,
                    companyId = d.CompanyId,
                    functions = d.Functions.Select(f => new
                    {
                        id = f.Id,
                        code = f.Code,
                        name = f.Name
                    }).ToList()
                });

                return Json(result);
            }
        }
        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var industry = await _industryRepository.ListAsync(new Expression<Func<Industry, bool>>[] { u => u.IsDeleted == false }, q => q.OrderBy(u => u.Id), null);
            var company = await _companyRepository.ListAsync(new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false }, q => q.OrderBy(u => u.Id), null);
            var head = await _userRepository.ListAsync(new Expression<Func<User, bool>>[] { u => u.IsDeleted == false }, q => q.OrderBy(u => u.Id), null);

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name");
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
            var industry = await _industryRepository.ListAsync(new Expression<Func<Industry, bool>>[] { u => u.IsDeleted == false }, q => q.OrderBy(u => u.Id), null);
            var company = await _companyRepository.ListAsync(new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false }, q => q.OrderBy(u => u.Id), null);
            var head = await _userRepository.ListAsync(new Expression<Func<User, bool>>[] { u => u.IsDeleted == false }, q => q.OrderBy(u => u.Id), null);

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", department.IndustryId);
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
            var industry = await _industryRepository.ListAsync(new Expression<Func<Industry, bool>>[] { u => u.IsDeleted == false }, q => q.OrderBy(u => u.Id), null);
            var company = await _companyRepository.ListAsync(new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false }, q => q.OrderBy(u => u.Id), null);
            var head = await _userRepository.ListAsync(new Expression<Func<User, bool>>[] { u => u.IsDeleted == false }, q => q.OrderBy(u => u.Id), null);

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", department.IndustryId);
            ViewBag.CompanyId = new SelectList(company, "Id", "Name", department.CompanyId);
            ViewBag.Head = new SelectList(head, "Id", "Name", department.Head);

            return View(department);
        }

        [HttpGet]
        public async Task<JsonResult> GetCompanyByIndustry(int industryId)
        {
            var companies = await _companyRepository.ListAsync(
                new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false && u.IndustryId == industryId },
                q => q.OrderBy(u => u.Id),
                null);

            var companyList = companies.Select(d => new { Id = d.Id, Name = d.Name }).ToList();
            return Json(companyList);
        }
    }
}