using AuditingSystem.Entities.Lockups;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Controllers.api.Lockups;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Lockups
{
    public class ControlFrequencyssController : Controller
    {
        private readonly IBaseRepository<ControlFrequency, int> _controlefrequncy;
        private readonly ILogger<ControlFrequencyssController> _logger;
        public ControlFrequencyssController(IBaseRepository<ControlFrequency, int> controlefrequncy)
        {
            _controlefrequncy = controlefrequncy;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var controls = await _controlefrequncy.ListAsync(
                       new Expression<Func<ControlFrequency, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id));

                return View(controls);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _controlefrequncy.FindByAsync(id));
        }

        public async Task<IActionResult> View(int id)
        {
            return View(await _controlefrequncy.FindByAsync(id));
        }
    }

}
