using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class CompanyController : Controller
    {
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Industry, int> _industryRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly IBaseRepository<Year, int> _yearRepository;
        private readonly HttpClient _httpClient;
        private readonly AuditingSystemDbContext db;

        public CompanyController(
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Industry, int> industryRepository,
            HttpClient httpClient,
            IBaseRepository<User, int> userRepository,
            IBaseRepository<Year, int> yearRepository,
            AuditingSystemDbContext db)
        {
            _companyRepository = companyRepository;
            _industryRepository = industryRepository;
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _userRepository = userRepository;
            _yearRepository = yearRepository;
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            //var companies = await _companyRepository.ListAsync(
            //    new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
            //    q => q.OrderBy(u => u.Industry.Code),
            //    c => c.Industry);

            var companies = db.Companies.Include(i => i.Industry).Include(d => d.Departments).ThenInclude(f=>f.Functions)
                .ThenInclude(a=>a.Activities).ThenInclude(o=>o.Objectives).ThenInclude(t=>t.Tasks).ThenInclude(p=>p.Practices);

            //var model = companies.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //ViewBag.TotalRow = companies.Count();
            //ViewBag.CurrentPage = page;
            //ViewBag.PageSize = pageSize;
            //ViewBag.TotalPages = (int)Math.Ceiling(companies.Count() / (double)pageSize);

            try
            {
                string apiurl = "https://onyx3.azurewebsites.net/companies/GetAllcompanies";
                using (HttpClient client = new HttpClient())
                {
                    string requestBody = "{}";
                    StringContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiurl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(responseBody);

                        foreach (var item in json["items"])
                        {
                            int id = (int)item["id"];
                            string name = (string)item["companyName"];
                            int? industryId = item["industryId"]?.ToObject<int?>() ?? null;
                            string code = (string)item["code"];
                            string address = (string)item["address"];
                            string contactNo = (string)item["contactNumber"];
                            string email = (string)item["email"];

                            var existingApiCompany = await _companyRepository.FindByAsync(id);

                            if (existingApiCompany == null)
                            {
                                var industryEntity = industryId.HasValue ? await _industryRepository.FindByAsync(industryId.Value) : null;

                                var newApiCompany = new Company
                                {
                                    Id = id,
                                    Name = name,
                                    Code = code,
                                    Source = "API",
                                    IndustryId = industryEntity?.Id,
                                    Address = address,
                                    ContactNo = contactNo,
                                    Email = email,
                                    CreatedBy = "Admin",
                                    CreationDate = DateTime.Now,
                                    UpdatedBy = "Admin",
                                    UpdatedDate = DateTime.Now,
                                    IsDeleted = false
                                };

                                await _companyRepository.CreateAsync(newApiCompany);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                // Example: Logger.LogError(ex, "Error in Index action");
            }

            return View(companies);
        }

        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var industry = await _industryRepository.ListAsync(
                new Expression<Func<Industry, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var outsources = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var insources = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var manager = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var year = await _yearRepository.ListAsync(
                new Expression<Func<Year, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Name),
                null);

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name");
            ViewBag.Outsources = new SelectList(outsources, "Id", "Name");
            ViewBag.Insources = new SelectList(insources, "Id", "Name");
            ViewBag.Manager = new SelectList(manager, "Id", "Name");
            ViewBag.Year = new SelectList(year, "Id", "Name");

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var company = await _companyRepository.FindByAsync(id);

            var industry = await _industryRepository.ListAsync(
                new Expression<Func<Industry, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var users = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var outsources = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var insources = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var manager = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var year = await _yearRepository.ListAsync(
                new Expression<Func<Year, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Name),
                null);

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", company?.IndustryId);

            ViewBag.Outsources = new SelectList(outsources, "Id", "Name", company?.Outsources?.Split(','));
            ViewBag.Insources = new SelectList(insources, "Id", "Name", company?.Insources?.Split(','));
            ViewBag.Manager = new SelectList(manager, "Id", "Name", company?.Manager?.Split(','));
            ViewBag.Year = new SelectList(year, "Id", "Name", company?.PlanYear?.Split(','));

            return View(company);
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var company = await _companyRepository.FindByAsync(id);

            var industry = await _industryRepository.ListAsync(
                new Expression<Func<Industry, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var users = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var outsources = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var insources = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var manager = await _userRepository.ListAsync(
                new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var year = await _yearRepository.ListAsync(
                new Expression<Func<Year, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", company?.IndustryId);

            ViewBag.Outsources = new SelectList(outsources, "Id", "Name", company?.Outsources?.Split(','));
            ViewBag.Insources = new SelectList(insources, "Id", "Name", company?.Insources?.Split(','));
            ViewBag.Manager = new SelectList(manager, "Id", "Name", company?.Manager?.Split(','));
            ViewBag.Year = new SelectList(year, "Id", "Name", company?.PlanYear?.Split(','));

            return View(company);
        }
    }
}
