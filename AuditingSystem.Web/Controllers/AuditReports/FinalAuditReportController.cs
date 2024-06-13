using AuditingSystem.Database;
using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.AuditReports;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.AuditReports
{
    public class FinalAuditReportController : Controller
    {
        private readonly IBaseRepository<DraftAuditReports, int> _draftAuditReportsRepository;
        private readonly IBaseRepository<ClientActionPlans, int> _clientActionPlansRepository;
        private readonly IBaseRepository<Industry, int> _industryRepository;
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly IBaseRepository<Finding, int> _findingRepository;
        private readonly IBaseRepository<Observation, int> _observationRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly AuditingSystemDbContext db;

        public FinalAuditReportController(IBaseRepository<DraftAuditReports, int> draftAuditReportsRepository,
            IBaseRepository<ClientActionPlans, int> clientActionPlansRepository, 
            IBaseRepository<Industry, int> industryRepository, 
            IBaseRepository<Company, int> companyRepository, 
            IBaseRepository<Department, int> departmentRepository,
            IBaseRepository<Function, int> functionRepository, 
            IBaseRepository<Finding, int> findingRepository, 
            IBaseRepository<Observation, int> observationRepository,
            IBaseRepository<User, int> userRepository,
            AuditingSystemDbContext db)
        {
            _draftAuditReportsRepository = draftAuditReportsRepository;
            _clientActionPlansRepository = clientActionPlansRepository;
            _industryRepository = industryRepository;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _functionRepository = functionRepository;
            _findingRepository = findingRepository;
            _observationRepository = observationRepository;
            _userRepository = userRepository;
            this.db = db;
        }

        public IActionResult Index()
        {
            var list = from dur in db.DraftAuditReports
                       join car in db.ClientActionPlans on dur.Id equals car.DraftAuditReportsId
                       //join i in db.Industries on dur.IndustryId equals i.Id
                       join c in db.Companies on dur.CompanyId equals c.Id
                       join d in db.Departments on dur.DepartmentId equals d.Id
                       join f in db.Functions on dur.FunctionId equals f.Id
                       join fi in db.Findings on dur.FindingId equals fi.Id
                       join o in db.Observations on dur.ObservationId equals o.Id
                       join u in db.Users on car.UserId equals u.Id
                       join rc in db.RiskCategories on dur.RiskCategoryId equals rc.Id
                       where dur.IsDeleted == false && car.IsDeleted == false/* && i.IsDeleted == false*/ && c.IsDeleted == false && d.IsDeleted == false &&
                       f.IsDeleted == false && fi.IsDeleted == false && o.IsDeleted == false && u.IsDeleted == false && rc.IsDeleted == false
                       select new FinalAuditReportVM { DraftAuditReports = dur, ClientActionPlans = car, /*Industry = i,*/
                       Company = c, Department = d, Function = f, Finding = fi, Observation = o, User = u, RiskCategory = rc};
            return View(list);
        }
    }
}
