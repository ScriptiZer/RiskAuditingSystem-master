using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class RolesPermissionsController : Controller
    {
        private readonly IBaseRepository<Role, int> _roleRepository;
        private readonly IBaseRepository<Permission, int> _permissionRepository;
        private readonly IBaseRepository<RolesPermissions, int> _rolesPermissionRepository;

        public RolesPermissionsController(IBaseRepository<Role, int> roleRepository, IBaseRepository<Permission, int> permissionRepository,
            IBaseRepository<RolesPermissions, int> rolesPermissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _rolesPermissionRepository = rolesPermissionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var rolesPermission = await _rolesPermissionRepository.ListAsync(
                       new Expression<Func<RolesPermissions, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id),
                       r=>r.Role,p=>p.Permission);

                return View(rolesPermission);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            var role = _roleRepository.ListAsync(
              new Expression<Func<Role, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var permission = _permissionRepository.ListAsync(
              new Expression<Func<Permission, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;

            ViewBag.RoleId = new SelectList(role, "Id", "Name");
            ViewBag.PermissionId = new SelectList(permission, "Id", "Name");
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var rolepermission = await _rolesPermissionRepository.FindByAsync(id);
            var role = _roleRepository.ListAsync(
              new Expression<Func<Role, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var permission = _permissionRepository.ListAsync(
              new Expression<Func<Permission, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;

            ViewBag.RoleId = new SelectList(role, "Id", "Name", rolepermission.RoleId);
            ViewBag.PermissionId = new SelectList(permission, "Id", "Name", rolepermission.PermissionId);
            return View(rolepermission);
        }

        public async Task<IActionResult> View(int id)
        {
            var rolepermission = await _rolesPermissionRepository.FindByAsync(id);
            var role = _roleRepository.ListAsync(
              new Expression<Func<Role, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var permission = _permissionRepository.ListAsync(
              new Expression<Func<Permission, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;

            ViewBag.RoleId = new SelectList(role, "Id", "Name", rolepermission.RoleId);
            ViewBag.PermissionId = new SelectList(permission, "Id", "Name", rolepermission.PermissionId);
            return View(rolepermission);
        }
    }
}
