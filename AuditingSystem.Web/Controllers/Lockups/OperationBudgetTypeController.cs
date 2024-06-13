using AuditingSystem.Entities.Lockups;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.Lockups
{
    public class OperationBudgetTypeController : Controller
    {
        private readonly IBaseRepository<OperationBudgetType, int> _operationBudgetTypeRepository;
        private readonly ILogger<OperationBudgetTypeController> _logger;
        public OperationBudgetTypeController(IBaseRepository<OperationBudgetType, int> operationBudgetTypeRepository)
        {
            _operationBudgetTypeRepository = operationBudgetTypeRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                var operationBudgetType = await _operationBudgetTypeRepository.ListAsync(
                       new Expression<Func<OperationBudgetType, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Id));

                return View(operationBudgetType);
            }
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _operationBudgetTypeRepository.FindByAsync(id));
        }

        public async Task<IActionResult> View(int id)
        {
            return View(await _operationBudgetTypeRepository.FindByAsync(id));
        }
    }
}
