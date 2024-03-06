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

        public AuditProgramsController(IBaseRepository<AuditProgram, int> auditProgramRepository)
        {
            _auditProgramRepository = auditProgramRepository;
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
    }
}
