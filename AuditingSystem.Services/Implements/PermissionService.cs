using AuditingSystem.Database;
using AuditingSystem.Entities.Setup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Services.Implements
{
    public class PermissionService
    {
        private readonly AuditingSystemDbContext _db;

        public PermissionService(AuditingSystemDbContext db)
        {
            _db = db;
        }

        //public List<Permission> GetPermissionsForUserRole(string roleName)
        //{
        //    var rolePermissions = _db.Roles
        //        .Include(r => r.RolesPermissions)
        //        .ThenInclude(rp => rp.Permission)
        //        .FirstOrDefault(r => r.Name == roleName)?
        //        .RolesPermissions
        //        .Select(rp => rp.Permission)
        //        .ToList();

        //    return rolePermissions ?? new List<Permission>();
        //}

        public List<RolesPermissions> GetPermissionsForUserRole(string roleName)
        {
            var rolePermissions = _db.RolesPermissions
                .Include(r => r.Role)
                .Include(p=>p.Permission)
                .Where(r=>r.Role.Name == roleName)
                .ToList();

            return rolePermissions ?? new List<RolesPermissions>();
        }
    }

}
