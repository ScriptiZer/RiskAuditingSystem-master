using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditFieldTests
{
    [Route("api/[controller]")]
    [ApiController]
    public class FindingsController : ControllerBase
    {
        private readonly IBaseRepository<Finding, int> _finding;
        private readonly ILogger<FindingsController> _logger;
        public FindingsController(IBaseRepository<Finding, int> finding, ILogger<FindingsController> logger)
        {
            _finding = finding ?? throw new ArgumentNullException(nameof(Finding));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var finding = await _finding.ListAsync();
                return Ok(finding);
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
                var finding = await _finding.FindByAsync(id);
                if (finding == null)
                {
                    return NotFound();
                }
                await _finding.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the controleffect with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Finding finding)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    finding.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    finding.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    finding.CreationDate = DateTime.Now;
                    finding.CurrentYear = DateTime.Now.Year;
                    await _finding.CreateAsync(finding);
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
        public async Task<IActionResult> Edit(int id, [FromBody] Finding finding)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingfinding = await _finding.FindByAsync(id);
                    if (existingfinding == null)
                    {
                        return NotFound();
                    }

                    existingfinding.UpdatedDate = DateTime.Now;
                    finding.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingfinding.Code = finding.Code;
                    existingfinding.Name = finding.Name;
                    existingfinding.Description = finding.Description;
                    await _finding.UpdateAsync(existingfinding);

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
