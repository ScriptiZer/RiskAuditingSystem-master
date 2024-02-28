using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class UserController : Controller
    {
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Role, int> _rolesRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        public UserController(
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<Role, int> roleRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<User, int> userRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _rolesRepository = roleRepository;
            _userRepository = userRepository;
        }


        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));
                var users = await _userRepository.ListAsync(
                    new Expression<Func<User, bool>>[] { u => u.IsDeleted == false},
                    q => q.OrderBy(u => u.Id),
                    u => u.Company, u => u.Department, u => u.Role);

                // Paginate the result
                var model = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.TotalRow = users.Count();
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(users.Count() / (double)pageSize);

                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }


        public async Task<IActionResult> Add()
        {
            var companies = _companyRepository.ListAsync(
                  new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var departments = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var roles = _rolesRepository.ListAsync(
                  new Expression<Func<Role, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name");
            ViewBag.RoleId = new SelectList(roles, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userRepository.FindByAsync(id);

            var companies = _companyRepository.ListAsync(
                  new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var departments = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var roles = _rolesRepository.ListAsync(
                  new Expression<Func<Role, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", user.DepartmentId);
            ViewBag.RoleId = new SelectList(roles, "Id", "Name", user.RoleId);

            return View(user);
        }

        public async Task<IActionResult> View(int id)
        {
            var user = await _userRepository.FindByAsync(id);

            var companies = _companyRepository.ListAsync(
                  new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var departments = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var roles = _rolesRepository.ListAsync(
                  new Expression<Func<Role, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", user.DepartmentId);
            ViewBag.RoleId = new SelectList(roles, "Id", "Name", user.RoleId);

            return View(user);
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

    }
}
