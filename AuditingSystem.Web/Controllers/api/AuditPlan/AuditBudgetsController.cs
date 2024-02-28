using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Controllers.api.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditPlan
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditBudgetsController : ControllerBase
    {
        private readonly IBaseRepository<AuditBudget, int> _auditBudgetRepository;
        private readonly AuditingSystemDbContext db;
        private readonly ILogger<AuditBudgetsController> _logger;

        public AuditBudgetsController(IBaseRepository<AuditBudget, int> auditBudget, ILogger<AuditBudgetsController> logger,
            AuditingSystemDbContext db)
        {
            _auditBudgetRepository = auditBudget ?? throw new ArgumentNullException(nameof(auditBudget));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var auditResources = await _auditBudgetRepository.ListAsync();
                return Ok(auditResources);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving roles.");
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var auditBudgetToDelete = await _auditBudgetRepository.FindByAsync(id);
                if (auditBudgetToDelete == null)
                {
                    return NotFound();
                }

                await _auditBudgetRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the role with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AuditBudget auditBudget)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _auditBudgetRepository.CreateAsync(auditBudget);
                    return NoContent();
                }

                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new role.");
                return StatusCode(500, new { error = "An error occurred while processing your request to add a new role." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] AuditBudget updateAuditBudget)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingauditBudget = await _auditBudgetRepository.FindByAsync(id);
                    if (existingauditBudget == null)
                    {
                        return NotFound();
                    }

                    existingauditBudget.TotalEstmated = updateAuditBudget.TotalEstmated;
                    existingauditBudget.TotalActual = updateAuditBudget.TotalActual;
                    existingauditBudget.Variance = updateAuditBudget.Variance; 
                    existingauditBudget.BudgetType = updateAuditBudget.BudgetType; 

                    await _auditBudgetRepository.UpdateAsync(existingauditBudget);

                    return NoContent();
                }

                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while editing the role with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to edit role with ID: {id}." });
            }
        }

        [HttpPost("AddBudgetList")]
        public async Task<IActionResult> AddBudgetList([FromBody] List<AuditBudgetList> auditBudgetList)
        
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var auditBudget in auditBudgetList)
                    {
                        var existingRecord = db.AuditBudgetLists.FirstOrDefault(a =>
                           // a.Estimated == auditBudget.Estimated &&
                           // a.Actual == auditBudget.Actual &&
                            a.Year == auditBudget.Year &&
                            a.Quarter == auditBudget.Quarter &&
                            a.Month == auditBudget.Month &&
                            a.BudgetId == auditBudget.BudgetId &&
                            a.CompanyId == auditBudget.CompanyId);

                        if (existingRecord != null)
                        {
                            existingRecord.Estimated = auditBudget.Estimated;
                            existingRecord.Actual = auditBudget.Actual;
                        }
                        else
                        {
                            db.AuditBudgetLists.Add(auditBudget);
                        }
                    }

                    
                    db.SaveChanges();

                    return Ok();
                }

                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding/updating a list of audit resources.");
                return StatusCode(500, new { error = "An error occurred while processing your request to add/update a list of audit resources." });
            }
        }


        [HttpPost("SaveDesciption")]
        public async Task<IActionResult> SaveDesciption(int id, string description)
        {
            var existingauditResource = await _auditBudgetRepository.FindByAsync(id);
            existingauditResource.Description = description;
            await _auditBudgetRepository.UpdateAsync(existingauditResource);

            return NoContent();
        }

    }
}
