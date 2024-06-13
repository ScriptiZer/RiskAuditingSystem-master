using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IBaseRepository<RolesPermissions, int> _rolePermissionsRepository;
        private readonly ILogger<RolePermissionsController> _logger;
        public RolePermissionsController(IBaseRepository<RolesPermissions, int> rolePermissionsRepository, ILogger<RolePermissionsController> logger)
        {
            _rolePermissionsRepository = rolePermissionsRepository ?? throw new ArgumentNullException(nameof(rolePermissionsRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var rolepermission = await _rolePermissionsRepository.ListAsync();
                return Ok(rolepermission);
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
                var rolepermission = await _rolePermissionsRepository.FindByAsync(id);
                if (rolepermission == null)
                {
                    return NotFound();
                }
                await _rolePermissionsRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the controleffect with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RolesPermissions rolesPermissions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    rolesPermissions.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    rolesPermissions.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    rolesPermissions.CreationDate = DateTime.Now;
                    rolesPermissions.CurrentYear = DateTime.Now.Year;
                    await _rolePermissionsRepository.CreateAsync(rolesPermissions);
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
        public async Task<IActionResult> Edit(int id, [FromBody] RolesPermissions rolesPermissions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRolePermission = await _rolePermissionsRepository.FindByAsync(id);
                    if (existingRolePermission == null)
                    {
                        return NotFound();
                    }

                    existingRolePermission.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingRolePermission.UpdatedDate = DateTime.Now;
                    existingRolePermission.RoleId = rolesPermissions.RoleId;
                    existingRolePermission.PermissionId = rolesPermissions.PermissionId;
                    existingRolePermission.Description = rolesPermissions.Description;
                    await _rolePermissionsRepository.UpdateAsync(existingRolePermission);

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
