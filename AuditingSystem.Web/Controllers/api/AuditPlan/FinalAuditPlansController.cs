using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditPlan
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalAuditPlansController : ControllerBase
    {
        private readonly IBaseRepository<FinalAuditPlan, int> _finalAuditPlanRepository;
        private readonly AuditingSystemDbContext _db;
        private readonly ILogger<DraftAuditPlansController> _logger;
        private readonly IBaseRepository<FinalAuditPlanList, int> _finalAuditPlanListRepository;

        public FinalAuditPlansController(IBaseRepository<FinalAuditPlan, int> finalAuditPlanRepository,
            AuditingSystemDbContext db, 
            ILogger<DraftAuditPlansController> logger,
            IBaseRepository<FinalAuditPlanList, int> finalAuditPlanListRepository)
        {
            _finalAuditPlanRepository = finalAuditPlanRepository;
            _db = db;
            _logger = logger;
            _finalAuditPlanListRepository = finalAuditPlanListRepository;
        }

        [HttpPost("AddFinalListData")]
        public async Task<IActionResult> AddFinalListData(List<FinalAuditPlanList>? finalAuditPlanList)
        {
            try
            {
                foreach (var item in finalAuditPlanList)
                {
                    var existingRecord = _db.FinalAuditPlanLists.FirstOrDefault(a =>
                        a.Plan == item.Plan &&
                        a.Actual == item.Actual &&
                        a.Year == item.Year &&
                        a.FinalAuditPlanId == item.FinalAuditPlanId &&
                        a.Quarter == item.Quarter);

                    if (existingRecord != null)
                    {
                        existingRecord.Plan = item.Plan;
                        existingRecord.Actual = item.Actual;
                    }
                    else
                    {
                        await _finalAuditPlanListRepository.CreateAsync(item);
                    }
                }

                _db.SaveChanges();

                // Assuming success, return a success response
                return Ok(new { success = true, message = "Data saved successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                _logger.LogError(ex, "An error occurred while saving data");
                return StatusCode(500, new { success = false, message = "An error occurred while saving data" });
            }
        }

        [HttpPost("SaveDesciption")]
        public async Task<IActionResult> SaveDesciption(int id, string description)
        {
            var existingauditResource = await _finalAuditPlanRepository.FindByAsync(id);
            existingauditResource.Description = description;
            await _finalAuditPlanRepository.UpdateAsync(existingauditResource);

            return Ok();
        }
    }
}
