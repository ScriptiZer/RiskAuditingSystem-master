using AuditingSystem.Entities.Setup;

namespace AuditingSystem.Web.ViewModels
{
    public class RolePermissionsViewModel
    {
        public Role Role { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
