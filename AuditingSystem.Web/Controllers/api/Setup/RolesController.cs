using AuditingSystem.Database;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.api.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IBaseRepository<Role, int> _roleRepository;
        private readonly ILogger<RolesController> _logger;
        private readonly AuditingSystemDbContext db;
        private readonly IBaseRepository<RolesPermissions, int> _rolesPermissionRepository;
        private readonly IBaseRepository<Permission, int> __permissionRepository;
        public RolesController(IBaseRepository<Role, int> roleRepository, ILogger<RolesController> logger,
            AuditingSystemDbContext db,
            IBaseRepository<RolesPermissions, int> rolesPermissionRepository, IBaseRepository<Permission, int> permissionRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.db = db;
            _rolesPermissionRepository = rolesPermissionRepository;
            __permissionRepository = permissionRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roles = await _roleRepository.ListAsync();
                return Ok(roles);
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
                var roleToDelete = await _roleRepository.FindByAsync(id);
                if (roleToDelete == null)
                {
                    return NotFound();
                }

                await _roleRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the role with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RolePermissionsViewModel rolePermissionsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _roleRepository.CreateAsync(rolePermissionsViewModel.Role);

                    List<RolesPermissions> rolesPermissions = new List<RolesPermissions>();
                    foreach (var item in rolePermissionsViewModel.Permissions)
                    {
                        RolesPermissions rolePermission = new RolesPermissions
                        {
                            RoleId = rolePermissionsViewModel.Role.Id,
                            PermissionId = item.Id,
                            View = item.View,
                            Add = item.Add,
                            Edit = item.Edit,
                            Delete = item.Delete
                        };

                        rolesPermissions.Add(rolePermission);
                    }

                    db.RolesPermissions.AddRange(rolesPermissions);
                    db.SaveChanges();

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
        public async Task<IActionResult> Edit(int id, [FromBody] RolePermissionsViewModel rolePermissionsViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRole = await _roleRepository.FindByAsync(id);
                    if (existingRole == null)
                    {
                        return NotFound();
                    }

                    // Update role properties
                    existingRole.Name = rolePermissionsViewModel.Role.Name;
                    existingRole.Description = rolePermissionsViewModel.Role.Description;

                    var rolesPermissions = await db.RolesPermissions
                        .Where(u => u.IsDeleted == false && u.RoleId == id)
                        .ToListAsync();

                    // Update or add new rolesPermissions based on rolePermissionsViewModel.Permissions
                    foreach (var item in rolePermissionsViewModel.Permissions)
                    {
                        var existingRolePermission = rolesPermissions.FirstOrDefault(rp => rp.Id == item.Id);

                        if (existingRolePermission != null)
                        {
                            // Update existing rolesPermission
                            existingRolePermission.View = item.View;
                            existingRolePermission.Add = item.Add;
                            existingRolePermission.Edit = item.Edit;
                            existingRolePermission.Delete = item.Delete;
                        }
                    }

                    // Update rolesPermissions in the database
                    db.RolesPermissions.UpdateRange(rolesPermissions);
                    await db.SaveChangesAsync();

                    // Return the updated RolesPermissions data
                    var updatedRolesPermissions = await db.RolesPermissions
                        .Where(u => u.IsDeleted == false && u.RoleId == id)
                        .ToListAsync();

                    return Ok(updatedRolesPermissions);
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
