using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class CalendarController : Controller
    {
        private readonly IBaseRepository<Calendar, int> _calendarRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly IBaseRepository<Year, int> _yearRepository;
        private readonly AuditingSystemDbContext db;

        public CalendarController(IBaseRepository<Calendar, int> calendarRepository, 
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<Function, int> functionRepository,
            IBaseRepository<User, int> userRepository,
            IBaseRepository<Year, int> yearRepository,
            AuditingSystemDbContext db)
        {
            _calendarRepository = calendarRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _functionRepository = functionRepository;
            _userRepository = userRepository;
            _yearRepository = yearRepository;
            this.db = db;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var getCompany = db.Users.Find(userId);
                //var getCompany = await _userRepository.FindByAsync(userId);
 

                var calendar = await _calendarRepository.ListAsync(
                       new Expression<Func<Calendar, bool>>[] { u => u.IsDeleted == false, u=>u.CompanyId ==  getCompany.CompanyId},
                       q => q.OrderBy(u => u.Year.Name),
                       y => y.Year, u => u.User, c => c.Company, d => d.Department, f => f.Function);


                var model = calendar.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.TotalRow = calendar.Count();
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(calendar.Count() / (double)pageSize);
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));
                ViewBag.UserId = userId;
                ViewBag.Company = getCompany.CompanyId;
                var companies = await _companyRepository.ListAsync(
                  new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false, c => c.Id == getCompany.CompanyId },
                  q => q.OrderBy(u => u.Id),
                  null);
                //var departments =await _departmentRepository.ListAsync(
                //      new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                //      q => q.OrderBy(u => u.Id),
                //      null);
                //var function =await _functionRepository.ListAsync(
                //      new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
                //      q => q.OrderBy(u => u.Id),
                //      null);
                //var user =await _userRepository.ListAsync(
                // new Expression<Func<User, bool>>[] { u => u.IsDeleted == false, u=>u.Role.Name == "Auditor" , c => c.CompanyId == getCompany.CompanyId},
                // q => q.OrderBy(u => u.Id),
                // null);
                var year =await _yearRepository.ListAsync(
                 new Expression<Func<Year, bool>>[] { u => u.IsDeleted == false},
                 q => q.OrderBy(u => u.Name),
                 null);


                ViewBag.CompanyId = new SelectList(companies, "Id", "Name");
                //ViewBag.DepartmentId = new SelectList(departments, "Id", "Name");
                //ViewBag.FunctionId = new SelectList(function, "Id", "Name");
                //ViewBag.UserId = new SelectList(user, "Id", "Name");
                ViewBag.YearId = new SelectList(year, "Id", "Name");

                return View();
            }

            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = userId;
            if (userId != null)
            {
                var calendar = await _calendarRepository.FindByAsync(id);
                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));
                ViewBag.Company = getCompany.CompanyId;
                ViewBag.UserId = userId;
                var companies = await _companyRepository.ListAsync(
                  new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false, c => c.Id == getCompany.CompanyId },
                  q => q.OrderBy(u => u.Id),
                  null);
                var departments = await _departmentRepository.ListAsync(
                      new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null);
                var function = await _functionRepository.ListAsync(
                      new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null);
                var user = await _userRepository.ListAsync(
                 new Expression<Func<User, bool>>[] { u => u.IsDeleted == false, u => u.Role.Name == "Auditor", c => c.CompanyId == getCompany.CompanyId },
                 q => q.OrderBy(u => u.Id),
                 null);
                var year = await _yearRepository.ListAsync(
                 new Expression<Func<Year, bool>>[] { u => u.IsDeleted == false },
                 q => q.OrderBy(u => u.Name),
                 null);


                ViewBag.CompanyId = new SelectList(companies, "Id", "Name", calendar.CompanyId);
                ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", calendar.DepartmentId);
                ViewBag.FunctionId = new SelectList(function, "Id", "Name", calendar.FunctionId);
                ViewBag.UserId = new SelectList(user, "Id", "Name", calendar.UserId);
                ViewBag.YearId = new SelectList(year, "Id", "Name", calendar.YearId);

                return View(calendar);
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = userId;
            if (userId != null)
            {
                var calendar = await _calendarRepository.FindByAsync(id);
                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));
                ViewBag.Company = getCompany.CompanyId;
                ViewBag.UserId = userId;
                var companies = await _companyRepository.ListAsync(
                  new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false, c => c.Id == getCompany.CompanyId },
                  q => q.OrderBy(u => u.Id),
                  null);
                var departments = await _departmentRepository.ListAsync(
                      new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null);
                var function = await _functionRepository.ListAsync(
                      new Expression<Func<Function, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Id),
                      null);
                var user = await _userRepository.ListAsync(
                 new Expression<Func<User, bool>>[] { u => u.IsDeleted == false, u => u.Role.Name == "Auditor", c => c.CompanyId == getCompany.CompanyId },
                 q => q.OrderBy(u => u.Id),
                 null);
                var year = await _yearRepository.ListAsync(
                 new Expression<Func<Year, bool>>[] { u => u.IsDeleted == false },
                 q => q.OrderBy(u => u.Name),
                 null);


                ViewBag.CompanyId = new SelectList(companies, "Id", "Name", calendar.CompanyId);
                ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", calendar.DepartmentId);
                ViewBag.FunctionId = new SelectList(function, "Id", "Name", calendar.FunctionId);
                ViewBag.UserId = new SelectList(user, "Id", "Name", calendar.UserId);
                ViewBag.YearId = new SelectList(year, "Id", "Name", calendar.YearId);

                return View(calendar);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
