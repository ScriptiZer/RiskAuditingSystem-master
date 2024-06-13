using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Lockups
{
    public class ControlEffectivenesscController : Controller
    {
        private readonly IBaseRepository<ControlEffectiveness, int> _controleeffevt;
        public ControlEffectivenesscController(IBaseRepository<ControlEffectiveness, int> controleeffevt)
        {
            _controleeffevt = controleeffevt;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var controls = await _controleeffevt.ListAsync(
                       new Expression<Func<ControlEffectiveness, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Rate));

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
            return View(await _controleeffevt.FindByAsync(id));
        }

        public async Task<IActionResult> View(int id)
        {
            return View(await _controleeffevt.FindByAsync(id));
        }
    }
}

