using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.AuditReports;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.AuditReports
{
    public class ClientActionPlanController : Controller
    {
        private readonly IBaseRepository<ClientActionPlans, int> _clientActionPlans;
        private readonly IBaseRepository<User, int> _UserId;
        private readonly IBaseRepository<DraftAuditReports, int> _draftAuditReports;
        public ClientActionPlanController(IBaseRepository<ClientActionPlans, int> clientActionPlans,
            IBaseRepository<User, int> UserId,
            IBaseRepository<DraftAuditReports, int> draftAuditReports)
        {
            _clientActionPlans = clientActionPlans;
            _UserId = UserId;
            _draftAuditReports = draftAuditReports;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var clientActionPlan = await _clientActionPlans.ListAsync(
                       new Expression<Func<ClientActionPlans, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id),
                       u=>u.User, d=>d.DraftAuditReports);

                return View(clientActionPlan);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            var responsibleEntity = _UserId.ListAsync(
              new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var draftAuditReports = _draftAuditReports.ListAsync(
              new Expression<Func<DraftAuditReports, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;

            ViewBag.UserId = new SelectList(responsibleEntity, "Id", "Name");
            ViewBag.DraftAuditReportsId = new SelectList(draftAuditReports, "Id", "Name");

            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var clientActionPlans = await _clientActionPlans.FindByAsync(id);
            var responsibleEntity = _UserId.ListAsync(
              new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var draftAuditReports = _draftAuditReports.ListAsync(
              new Expression<Func<DraftAuditReports, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;

            ViewBag.UserId = new SelectList(responsibleEntity, "Id", "Name", clientActionPlans.UserId);
            ViewBag.DraftAuditReportsId = new SelectList(draftAuditReports, "Id", "Name", clientActionPlans.DraftAuditReportsId);
            return View(clientActionPlans);
        }

        public async Task<IActionResult> View(int id)
        {
            var clientActionPlans = await _clientActionPlans.FindByAsync(id);
            var responsibleEntity = _UserId.ListAsync(
              new Expression<Func<User, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;
            var draftAuditReports = _draftAuditReports.ListAsync(
              new Expression<Func<DraftAuditReports, bool>>[] { u => u.IsDeleted == false },
              q => q.OrderBy(u => u.Id),
              null).Result;

            ViewBag.UserId = new SelectList(responsibleEntity, "Id", "Name", clientActionPlans.UserId);
            ViewBag.DraftAuditReportsId = new SelectList(draftAuditReports, "Id", "Name", clientActionPlans.DraftAuditReportsId);
            return View(clientActionPlans);
        }
    }
}
