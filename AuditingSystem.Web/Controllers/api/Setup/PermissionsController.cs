using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IBaseRepository<Permission, int> _permissionRepository;
        private readonly ILogger<PermissionsController> _logger;
        public PermissionsController(IBaseRepository<Permission, int> permissionRepository, ILogger<PermissionsController> logger)
        {
            _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var permission = await _permissionRepository.ListAsync();
                return Ok(permission);
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
                var permission = await _permissionRepository.FindByAsync(id);
                if (permission == null)
                {
                    return NotFound();
                }
                await _permissionRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the controleffect with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Permission permission)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    permission.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    permission.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    permission.CreationDate = DateTime.Now;
                    permission.CurrentYear = DateTime.Now.Year;
                    await _permissionRepository.CreateAsync(permission);
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
        public async Task<IActionResult> Edit(int id, [FromBody] Permission permission)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingPermission = await _permissionRepository.FindByAsync(id);
                    if (existingPermission == null)
                    {
                        return NotFound();
                    }

                    existingPermission.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingPermission.UpdatedDate = DateTime.Now;
                    existingPermission.Name = permission.Name;
                    existingPermission.Description = permission.Description;
                    await _permissionRepository.UpdateAsync(existingPermission);

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
