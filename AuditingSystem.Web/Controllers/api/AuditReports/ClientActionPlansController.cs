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
    public class ClientActionPlansController : ControllerBase
    {
        private readonly IBaseRepository<ClientActionPlans, int> _clientActionPlans;
        private readonly ILogger<ClientActionPlansController> _logger;
        public ClientActionPlansController(IBaseRepository<ClientActionPlans, int> clientActionPlans, ILogger<ClientActionPlansController> logger)
        {
            _clientActionPlans = clientActionPlans ?? throw new ArgumentNullException(nameof(clientActionPlans));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var clientActionPlans = await _clientActionPlans.ListAsync();
                return Ok(clientActionPlans);
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
                var clientActionPlans = await _clientActionPlans.FindByAsync(id);
                if (clientActionPlans == null)
                {
                    return NotFound();
                }
                await _clientActionPlans.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the controleffect with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClientActionPlans clientActionPlans)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    clientActionPlans.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    clientActionPlans.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    clientActionPlans.CreationDate = DateTime.Now;
                    clientActionPlans.CurrentYear = DateTime.Now.Year;
                    DateTime complationDate = Convert.ToDateTime(clientActionPlans.CompletionDate);
                    clientActionPlans.CompletionDate = complationDate.AddDays(1);
                    await _clientActionPlans.CreateAsync(clientActionPlans);
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
        public async Task<IActionResult> Edit(int id, [FromBody] ClientActionPlans clientActionPlans)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingclientActionPlans = await _clientActionPlans.FindByAsync(id);
                    if (existingclientActionPlans == null)
                    {
                        return NotFound();
                    }

                    existingclientActionPlans.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingclientActionPlans.UpdatedDate = DateTime.Now;
                    existingclientActionPlans.Name = clientActionPlans.Name;
                    existingclientActionPlans.Description = clientActionPlans.Description;
                    existingclientActionPlans.ManagementAcceptance = clientActionPlans.ManagementAcceptance;
                    existingclientActionPlans.UserId = clientActionPlans.UserId;
                    existingclientActionPlans.DraftAuditReportsId = clientActionPlans.DraftAuditReportsId;
                    existingclientActionPlans.CompletionDate = clientActionPlans.CompletionDate;

                    await _clientActionPlans.UpdateAsync(existingclientActionPlans);

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
