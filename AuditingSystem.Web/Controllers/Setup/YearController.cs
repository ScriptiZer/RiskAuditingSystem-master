using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class YearController : Controller
    {
        private readonly IBaseRepository<Year, int> _yearRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Quarter, int> _quarterRepository;
        private readonly IBaseRepository<User, int> _userRepository;

        public YearController(
            IBaseRepository<Year, int> yearRepository,
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<Quarter, int> quarterRepository,
            IBaseRepository<User, int> userRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _yearRepository = yearRepository;
            _quarterRepository = quarterRepository;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetString("UserId");
            

            if (userId != null) { 
                var year = await _yearRepository.ListAsync(
                      new Expression<Func<Year, bool>>[] { u => u.IsDeleted == false },
                      q => q.OrderBy(u => u.Name),
                      /*u => u.Company, u => u.Department*/ null);

                var model = year.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.TotalRow = year.Count();
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(year.Count() / (double)pageSize);

                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            ViewBag.UserId = userId;
            ViewBag.Company = getCompany.CompanyId;

            if(userId != null) { 
                var quarter = await _quarterRepository.ListAsync(
                  new Expression<Func<Quarter, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null);

                ViewBag.Quarter = new SelectList(quarter, "Id", "Name");

                return View();
            }
            return RedirectToAction("Login", "Account");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");
            var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            ViewBag.Company = getCompany.CompanyId;

            if (userId != null)
            {
                var year = await _yearRepository.FindByAsync(id);

                var quarter = await _quarterRepository.ListAsync(
                 new Expression<Func<Quarter, bool>>[] { u => u.IsDeleted == false },
                 q => q.OrderBy(u => u.Id),
                 null);

                ViewBag.Quarter = new SelectList(quarter, "Id", "Name", year?.Quarter?.Split(','));

                return View(year);
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> View(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");
            var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            ViewBag.Company = getCompany.CompanyId;

            if (userId != null)
            {
                var year = await _yearRepository.FindByAsync(id);

                var quarter = await _quarterRepository.ListAsync(
                 new Expression<Func<Quarter, bool>>[] { u => u.IsDeleted == false },
                 q => q.OrderBy(u => u.Id),
                 null);

                ViewBag.Quarter = new SelectList(quarter, "Id", "Name", year?.Quarter?.Split(','));

                return View(year);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
