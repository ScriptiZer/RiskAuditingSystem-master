using AuditingSystem.Entities.AuditFieldWork;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.AuditFieldWork
{
    public class AuditProgramController : Controller
    {
        private readonly IBaseRepository<AuditProgram, int> _auditProgramRepository;
        private readonly IBaseRepository<AuditProgramList, int> _auditProgramListRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        public AuditProgramController(IBaseRepository<AuditProgram, int> auditProgramRepository,
            IBaseRepository<AuditProgramList, int> auditProgramListRepository,
            IBaseRepository<User, int> userRepository, 
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository)
        {
            _auditProgramRepository = auditProgramRepository;
            _auditProgramListRepository = auditProgramListRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            var list = await _auditProgramRepository.ListAsync(
            new Expression<Func<AuditProgram, bool>>[] { u => u.IsDeleted == false, a => a.CompanyId == getCompany.CompanyId },
            q => q.OrderBy(u => u.Id),
            c => c.Company, d=>d.Department, u=>u.User);
            return View(list);
        }

        public async Task<IActionResult> Add()
        {
            var list = await _auditProgramListRepository.ListAsync(
                new Expression<Func<AuditProgramList, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                c => c.AuditProgram, d => d.RiskCategory);

            var companies = _companyRepository.ListAsync(
              new Expression<Func<Company, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var departments = _departmentRepository.ListAsync(
                  new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;
            var auditors = _userRepository.ListAsync(
                  new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
                  q => q.OrderBy(u => u.Id),
                  null).Result;

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name");
            ViewBag.AuditorId = new SelectList(auditors, "Id", "Name");
            return View();
        }
    }
}
