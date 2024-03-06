using AuditingSystem.Database;
using AuditingSystem.Entities.AuditFieldWork;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Services.ViewModels;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AuditingSystem.Web.Controllers.AuditFieldWork
{
    public class AuditProgramController : Controller
    {
        private readonly IBaseRepository<AuditProgram, int> _auditProgramRepository;
        private readonly IBaseRepository<AuditProgramList, int> _auditProgramListRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly AuditingSystemDbContext db;
        public AuditProgramController(IBaseRepository<AuditProgram, int> auditProgramRepository,
            IBaseRepository<AuditProgramList, int> auditProgramListRepository,
            IBaseRepository<User, int> userRepository, 
            IBaseRepository<Company, int> companyRepository,
            IBaseRepository<Department, int> departmentRepository,
            AuditingSystemDbContext db)
        {
            _auditProgramRepository = auditProgramRepository;
            _auditProgramListRepository = auditProgramListRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            this.db = db;
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
                c => c.AuditProgram);

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

        public async Task<IActionResult> Edit(int id)
        {
            var auditProgram = await _auditProgramRepository.FindByAsync(id);

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

            var auditProgramList = from pl in db.AuditProgramLists
                                   join p in db.AuditPrograms on pl.AuditProgramId equals p.Id
                                   join ri in db.RiskIdentifications on pl.RiskIdenticationId equals ri.Id
                                   join rc in db.Controls on pl.ControlId equals rc.Id
                                   join c in db.Companies on ri.CompanyId equals c.Id
                                   join d in db.Departments on ri.DepartmentId equals d.Id
                                   join rcat in db.RiskCategories on ri.RiskCategoryId equals rcat.Id
                                   join ct in db.ControlTypes on rc.ControlTypeId equals ct.Id
                                   join cp in db.ControlProcesses on rc.ControlProcessId equals cp.Id
                                   join cf in db.ControlFrequencies on rc.ControlFrequencyId equals cf.Id
                                   join ce in db.ControlEffectivenesses on rc.ControlEffectivenessId equals ce.Id
                                   where p.IsDeleted == false && pl.IsDeleted == false && pl.AuditProgramId == id
                                   select new AuditProgramVM {
                                       Company = c,
                                       Department = d,
                                       AuditProgramList = pl,
                                       AuditProgram = p,
                                       RiskIdentification = ri,
                                       RiskCategory = rcat,
                                       Control = rc,
                                       ControlType = ct,
                                       ControlProcess = cp,
                                       ControlFrequency = cf,
                                       ControlEffectiveness = ce
                                   };
            ViewBag.auditProgramList = auditProgramList;

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name", auditProgram.CompanyId);
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", auditProgram.DepartmentId);
            ViewBag.AuditorId = new SelectList(auditors, "Id", "Name", auditProgram.AuditorId);

            return View(auditProgram);
        }
    }
}
