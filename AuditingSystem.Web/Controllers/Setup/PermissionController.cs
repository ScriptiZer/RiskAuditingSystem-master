using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class PermissionController : Controller
    {
        private readonly IBaseRepository<Permission, int> _permissionRepository;

        public PermissionController(IBaseRepository<Permission, int> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var permission = await _permissionRepository.ListAsync(
                       new Expression<Func<Permission, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id));

                return View(permission);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _permissionRepository.FindByAsync(id));
        }

        public async Task<IActionResult> View(int id)
        {
            return View(await _permissionRepository.FindByAsync(id));
        }
    }
}
