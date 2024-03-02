using AuditingSystem.Entities.AuditFieldWork;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.AuditFieldWork
{
    public class AuditProgramListController : Controller
    {
        private readonly IBaseRepository<AuditProgram,int> _auditProgramRepository;
        private readonly IBaseRepository<AuditProgramList,int> _auditProgramListRepository;

        public AuditProgramListController(IBaseRepository<AuditProgram, int> auditProgramRepository, 
            IBaseRepository<AuditProgramList, int> auditProgramListRepository)
        {
            _auditProgramRepository = auditProgramRepository;
            _auditProgramListRepository = auditProgramListRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
