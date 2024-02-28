using AuditingSystem.Entities.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Services.Interfaces
{
    public interface IUserContext
    {
        string UserName { get; set; }
        //List<Permission> UserPermissions { get; set; }
        List<RolesPermissions> UserPermissions { get; set; }
    }
}
