using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.api.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBaseRepository<User,int> _userRepository;
        private readonly IBaseRepository<AuditBudget,int> _auditBudgetRepository;
        private readonly IBaseRepository<Department, int> _departmentRepository;
        private readonly ILogger<UsersController> _logger;
        private readonly AuditingSystemDbContext db;

        public UsersController(IBaseRepository<User, int> userRepository, ILogger<UsersController> logger,
            IBaseRepository<Department, int> departmentRepository,
            AuditingSystemDbContext db,
            IBaseRepository<AuditBudget, int> auditBudgetRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _departmentRepository = departmentRepository;
            this.db = db;
            _auditBudgetRepository = auditBudgetRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userRepository.ListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userToDelete = await _userRepository.FindByAsync(id);
                if (userToDelete == null)
                {
                    return NotFound();
                }

                await _userRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the user with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete user with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    user.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    user.CreationDate = DateTime.Now;
                    user.CurrentYear = DateTime.Now.Year;
                    await _userRepository.CreateAsync(user);

                    var auditBudget = new AuditBudget();
                    auditBudget.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    auditBudget.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    auditBudget.CreationDate = DateTime.Now;
                    auditBudget.CurrentYear = DateTime.Now.Year;
                    auditBudget.ResourceId = user.Id;
                    auditBudget.CompanyId = Convert.ToInt32(user.CompanyId);
                    auditBudget.IsDeleted = false;
                    await _auditBudgetRepository.CreateAsync(auditBudget);

                    return NoContent();
                }

                return BadRequest(ModelState); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user.");
                return StatusCode(500, new { error = "An error occurred while processing your request to add a new user." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] User updatedUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = await _userRepository.FindByAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingUser.UpdatedDate = DateTime.Now;
                    existingUser.Title = updatedUser.Title;
                    existingUser.Email = updatedUser.Email;
                    existingUser.Name = updatedUser.Name;
                    existingUser.ResourceType = updatedUser.ResourceType;
                    existingUser.CompanyId = updatedUser.CompanyId;
                    existingUser.DepartmentId = updatedUser.DepartmentId;
                    existingUser.RoleId = updatedUser.RoleId;
                    existingUser.ContactNo = updatedUser.ContactNo;
                    existingUser.Description = updatedUser.Description;
                    existingUser.Password = updatedUser.Password;
                    existingUser.ConfirmPassword = updatedUser.ConfirmPassword;

                    await _userRepository.UpdateAsync(existingUser);
                    return NoContent();
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while editing the user with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to edit user with ID: {id}." });
            }
        }

        [HttpGet("GetDepartmentsByCompany")]
        public IActionResult GetDepartmentsByCompany(int companyId)
        {
            try
            {
                var departments = _departmentRepository.ListAsync(
                    new Expression<Func<Department, bool>>[] { u => u.IsDeleted == false, c => c.CompanyId == companyId },
                    q => q.OrderBy(u => u.Id),
                    null).Result;

                var departmentList = departments.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList();

                return Ok(departmentList);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
