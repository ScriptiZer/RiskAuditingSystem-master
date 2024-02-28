using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingSystem.Services.Implements
{
    public class UserContext : IUserContext
    {
        public string UserName { get; set; }
        public List<RolesPermissions> UserPermissions { get; set; }
    }
}
