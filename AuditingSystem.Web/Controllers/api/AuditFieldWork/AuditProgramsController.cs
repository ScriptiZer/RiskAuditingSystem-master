using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditFieldWork;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditFieldWork
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditProgramsController : ControllerBase
    {
        private readonly IBaseRepository<AuditProgram, int> _auditProgramRepository;
        private readonly ILogger<AuditProgram> _logger;

        public AuditProgramsController(IBaseRepository<AuditProgram, int> auditProgramRepository, ILogger<AuditProgram> logger)
        {
            _auditProgramRepository = auditProgramRepository ?? throw new ArgumentNullException(nameof(auditProgramRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] AuditProgram auditProgram)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingAuditProgram = await _auditProgramRepository.FindByAsync(id);

                    if (existingAuditProgram == null)
                    {
                        return NotFound(new { error = $"Audit Program with ID {id} not found" });
                    }

                    existingAuditProgram.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingAuditProgram.UpdatedDate = DateTime.Now;
                    existingAuditProgram.AuditorId = auditProgram.AuditorId;
                    existingAuditProgram.ActualDate = auditProgram.ActualDate;
                    existingAuditProgram.Period = auditProgram.Period;

                    await _auditProgramRepository.UpdateAsync(existingAuditProgram);

                    return NoContent();
                }

                return BadRequest(new { error = "Invalid ModelState" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = $"Error updating audot program: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var auditProgramToDelete = await _auditProgramRepository.FindByAsync(id);
                if (auditProgramToDelete == null)
                {
                    return NotFound();
                }

                await _auditProgramRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the role with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }
    }
}
