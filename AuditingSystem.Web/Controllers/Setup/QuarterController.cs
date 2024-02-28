using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class QuarterController : Controller
    {
        private readonly IBaseRepository<Quarter, int> _quarterRepository;
        public QuarterController(IBaseRepository<Quarter, int> quarterRepository)
        {
            _quarterRepository = quarterRepository;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var quarter = await _quarterRepository.ListAsync(
                       new Expression<Func<Quarter, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id));


                var model = quarter.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.TotalRow = quarter.Count();
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(quarter.Count() / (double)pageSize);
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var monthsList = Enum.GetValues(typeof(Monthes)).Cast<Monthes>()
                    .Select(m => new SelectListItem { Value = m.ToString(), Text = m.ToString() });

                ViewBag.MonthsList = new SelectList(monthsList, "Value", "Text");

                return View();
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var monthsList = Enum.GetValues(typeof(Monthes)).Cast<Monthes>()
                    .Select(m => new SelectListItem { Value = m.ToString(), Text = m.ToString() });

                ViewBag.MonthsList = new SelectList(monthsList, "Value", "Text");

                var quarter = await _quarterRepository.FindByAsync(id);

                ViewBag.SelectedMonths = quarter.Month?.Select(m => m.ToString()).ToArray();

                return View(quarter);
            }

            return RedirectToAction("Login", "Account");
        }


    }
}
