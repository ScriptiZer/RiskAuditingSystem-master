using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Controllers.api.Lockups;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditFieldTests
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        private readonly IBaseRepository<Observation, int> _observation;
        private readonly ILogger<ObservationsController> _logger;
        public ObservationsController(IBaseRepository<Observation, int> observation, ILogger<ObservationsController> logger)
        {
            _observation = observation ?? throw new ArgumentNullException(nameof(Observation));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var observation = await _observation.ListAsync();
                return Ok(observation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving controleffect.");
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var observation = await _observation.FindByAsync(id);
                if (observation == null)
                {
                    return NotFound();
                }
                await _observation.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the controleffect with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Observation observation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    observation.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    observation.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    observation.CreationDate = DateTime.Now;
                    observation.CurrentYear = DateTime.Now.Year;
                    await _observation.CreateAsync(observation);
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
        public async Task<IActionResult> Edit(int id, [FromBody] Observation observation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingobservation = await _observation.FindByAsync(id);
                    if (existingobservation == null)
                    {
                        return NotFound();
                    }

                    existingobservation.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingobservation.UpdatedDate = DateTime.Now;
                    existingobservation.Name = observation.Name;
                    existingobservation.Description = observation.Description;
                    await _observation.UpdateAsync(existingobservation);

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
