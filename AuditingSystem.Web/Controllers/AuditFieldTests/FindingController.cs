using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.AuditFieldTests
{
    public class FindingController : Controller
    {
        private readonly IBaseRepository<Finding, int> _finding;
        public FindingController(IBaseRepository<Finding, int> finding)
        {
            _finding = finding;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var finding = await _finding.ListAsync(
                       new Expression<Func<Finding, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Code));

                return View(finding);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _finding.FindByAsync(id));
        }

        public async Task<IActionResult> View(int id)
        {
            return View(await _finding.FindByAsync(id));
        }
    }
}
