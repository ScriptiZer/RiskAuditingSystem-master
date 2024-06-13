using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditReports;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Controllers.api.Lockups;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditReports
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftAuditReportsController : ControllerBase
    {
        private readonly IBaseRepository<DraftAuditReports, int> _draftAuditReports;
        private readonly ILogger<DraftAuditReportsController> _logger;
        public DraftAuditReportsController(IBaseRepository<DraftAuditReports, int> draftAuditReports, ILogger<DraftAuditReportsController> logger)
        {
            _draftAuditReports = draftAuditReports ?? throw new ArgumentNullException(nameof(draftAuditReports));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var draftreport = await _draftAuditReports.ListAsync();
                return Ok(draftreport);
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
                var draftreport = await _draftAuditReports.FindByAsync(id);
                if (draftreport == null)
                {
                    return NotFound();
                }
                await _draftAuditReports.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the controleffect with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DraftAuditReports draftAuditReports)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    draftAuditReports.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    draftAuditReports.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    draftAuditReports.CreationDate = DateTime.Now;
                    draftAuditReports.CurrentYear = DateTime.Now.Year;
                    await _draftAuditReports.CreateAsync(draftAuditReports);
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
        public async Task<IActionResult> Edit(int id, [FromBody] DraftAuditReports draftReport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingdraftAuditReports = await _draftAuditReports.FindByAsync(id);
                    if (existingdraftAuditReports == null)
                    {
                        return NotFound();
                    }

                    existingdraftAuditReports.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingdraftAuditReports.UpdatedDate = DateTime.Now;
                    existingdraftAuditReports.Name = draftReport.Name;
                   // existingdraftAuditReports.IndustryId = draftReport.IndustryId;
                    existingdraftAuditReports.CompanyId = draftReport.CompanyId;
                    existingdraftAuditReports.DepartmentId = draftReport.DepartmentId;
                    existingdraftAuditReports.FunctionId = draftReport.FunctionId;
                    existingdraftAuditReports.FindingId = draftReport.FindingId;
                    existingdraftAuditReports.ObservationId = draftReport.ObservationId;
                    existingdraftAuditReports.Significance = draftReport.Significance;
                    existingdraftAuditReports.RiskImpact = draftReport.RiskImpact;
                    existingdraftAuditReports.RiskCategoryId = draftReport.RiskCategoryId;
                    existingdraftAuditReports.Recomendation = draftReport.Recomendation;


                    await _draftAuditReports.UpdateAsync(existingdraftAuditReports);

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
