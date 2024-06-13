using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers
{
    public class ObservationController : Controller
    {
        private readonly IBaseRepository<Observation, int> _observation;
        public ObservationController(IBaseRepository<Observation, int> observation)
        {
            _observation = observation;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var observation = await _observation.ListAsync(
                       new Expression<Func<Observation, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id));

                return View(observation);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _observation.FindByAsync(id));
        }

        public async Task<IActionResult> View(int id)
        {
            return View(await _observation.FindByAsync(id));
        }
    }
}
