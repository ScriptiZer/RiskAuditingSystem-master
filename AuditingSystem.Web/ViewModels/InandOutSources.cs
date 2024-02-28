using AuditingSystem.Entities.Setup;

namespace AuditingSystem.Web.ViewModels
{
    public class InandOutSources<T>
    {
        public List<T>? Insources { get; set; }
        public List<T>? Outsources { get; set; }
    }
}
