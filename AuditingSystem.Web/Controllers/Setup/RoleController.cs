using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class RoleController : Controller
    {
        private readonly IBaseRepository<Role, int> _roleRepository;
        private readonly IBaseRepository<Permission, int> _permissionRepository;
        private readonly AuditingSystemDbContext db;
        private readonly IBaseRepository<RolesPermissions, int> _rolesPermissionRepository;
       
        public RoleController(IBaseRepository<Role, int> roleRepository,
            IBaseRepository<Permission, int> permissionRepository, AuditingSystemDbContext db,
            IBaseRepository<RolesPermissions, int> rolesPermissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            this.db = db;
            this._rolesPermissionRepository = rolesPermissionRepository;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if(userId != null) { 
                var role = await _roleRepository.ListAsync(
                       new Expression<Func<Role, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id));


                var model = role.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.TotalRow = role.Count();
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(role.Count() / (double)pageSize);
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            var permissions = await _permissionRepository.ListAsync(
                       new Expression<Func<Permission, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id));

            ViewBag.permissions = permissions;

            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var rolespermissions = await _rolesPermissionRepository.ListAsync(
                    new Expression<Func<RolesPermissions, bool>>[] { u => u.IsDeleted == false, r=>r.RoleId ==id },
                    q => q.OrderBy(u => u.Id),
                    r=>r.Permission);

            if (rolespermissions == null || rolespermissions.Count() == 0)
            {
                var existingPermissions = await _rolesPermissionRepository.ListAsync(
                    new Expression<Func<RolesPermissions, bool>>[] { u => u.IsDeleted == false, r => r.RoleId == id },
                    q => q.OrderBy(u => u.Id),
                    r => r.Permission);

                db.RolesPermissions.RemoveRange(existingPermissions);

                List<RolesPermissions> rolesPermissions = new List<RolesPermissions>();
                var permissions = await _permissionRepository.ListAsync(
                    new Expression<Func<Permission, bool>>[] { u => u.IsDeleted == false },
                    q => q.OrderBy(u => u.Id));

                foreach (var item in permissions)
                {
                    RolesPermissions rolePermission = new RolesPermissions
                    {
                        RoleId = id,
                        PermissionId = item.Id,
                        View = item.View,
                        Add = item.Add,
                        Edit = item.Edit,
                        Delete = item.Delete,
                        ExportToPDF = item.ExportToPDF,
                        ExportToWord = item.ExportToWord,
                        ExportToExcel = item.ExportToExcel
                    };

                    rolesPermissions.Add(rolePermission);
                }

                db.RolesPermissions.AddRange(rolesPermissions);
                await db.SaveChangesAsync();
                ViewBag.permissions = rolesPermissions;
                return View(await _roleRepository.FindByAsync(id));
            }
            ViewBag.permissions = rolespermissions;

            return View(await _roleRepository.FindByAsync(id));
        }

        public async Task<IActionResult> View(int id)
        {
            var rolespermissions = await _rolesPermissionRepository.ListAsync(
                    new Expression<Func<RolesPermissions, bool>>[] { u => u.IsDeleted == false, r => r.RoleId == id },
                    q => q.OrderBy(u => u.Id),
                    r => r.Permission);

            if (rolespermissions == null || rolespermissions.Count() == 0)
            {
                var existingPermissions = await _rolesPermissionRepository.ListAsync(
                    new Expression<Func<RolesPermissions, bool>>[] { u => u.IsDeleted == false, r => r.RoleId == id },
                    q => q.OrderBy(u => u.Id),
                    r => r.Permission);

                db.RolesPermissions.RemoveRange(existingPermissions);

                List<RolesPermissions> rolesPermissions = new List<RolesPermissions>();
                var permissions = await _permissionRepository.ListAsync(
                    new Expression<Func<Permission, bool>>[] { u => u.IsDeleted == false },
                    q => q.OrderBy(u => u.Id));

                foreach (var item in permissions)
                {
                    RolesPermissions rolePermission = new RolesPermissions
                    {
                        RoleId = id,
                        PermissionId = item.Id,
                        View = item.View,
                        Add = item.Add,
                        Edit = item.Edit,
                        Delete = item.Delete,
                        ExportToPDF = item.ExportToPDF,
                        ExportToWord = item.ExportToWord,
                        ExportToExcel = item.ExportToExcel
                    };

                    rolesPermissions.Add(rolePermission);
                }

                db.RolesPermissions.AddRange(rolesPermissions);
                await db.SaveChangesAsync();
                ViewBag.permissions = rolesPermissions;
                return View(await _roleRepository.FindByAsync(id));
            }
            ViewBag.permissions = rolespermissions;

            return View(await _roleRepository.FindByAsync(id));
        }
    }
}
