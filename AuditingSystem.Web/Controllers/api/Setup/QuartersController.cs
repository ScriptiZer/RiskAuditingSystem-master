using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuartersController : ControllerBase
    {
        private readonly IBaseRepository<Quarter, int> _quarterRepository;
        private readonly ILogger<QuartersController> _logger;

        public QuartersController(IBaseRepository<Quarter, int> quarterRepository, ILogger<QuartersController> logger)
        {
            _quarterRepository = quarterRepository ?? throw new ArgumentNullException(nameof(quarterRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var quarter = await _quarterRepository.ListAsync();
                return Ok(quarter);
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
                var quarterToDelete = await _quarterRepository.FindByAsync(id);
                if (quarterToDelete == null)
                {
                    return NotFound();
                }

                await _quarterRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the role with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Quarter quarter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    quarter.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    quarter.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    quarter.CreationDate = DateTime.Now;
                    quarter.CurrentYear = DateTime.Now.Year;
                    await _quarterRepository.CreateAsync(quarter);
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
        public async Task<IActionResult> Edit(int id, [FromBody] Quarter updatedQuarter)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingQuarter = await _quarterRepository.FindByAsync(id);
                    if (existingQuarter == null)
                    {
                        return NotFound();
                    }

                    existingQuarter.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingQuarter.UpdatedDate = DateTime.Now;
                    existingQuarter.Name = updatedQuarter.Name;
                    existingQuarter.Description = updatedQuarter.Description;
                    existingQuarter.Month = updatedQuarter.Month;

                    await _quarterRepository.UpdateAsync(existingQuarter);

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
    }
}
