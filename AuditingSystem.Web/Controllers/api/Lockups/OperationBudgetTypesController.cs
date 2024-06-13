using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Controllers.Lockups;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.Lockups
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationBudgetTypesController : ControllerBase
    {
        private readonly IBaseRepository<OperationBudgetType, int> _operationBudgetTypeRepository;
        private readonly IBaseRepository<AuditBudget, int> _auditBudgetRepository;
        public OperationBudgetTypesController(IBaseRepository<OperationBudgetType, int> operationBudgetTypeRepository,
            IBaseRepository<AuditBudget, int> auditBudgetRepository)
        {
            _operationBudgetTypeRepository = operationBudgetTypeRepository ?? throw new ArgumentNullException(nameof(operationBudgetTypeRepository));
            _auditBudgetRepository = auditBudgetRepository ?? throw new ArgumentNullException(nameof(auditBudgetRepository));
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var operationBudgetTypeRepository = await _operationBudgetTypeRepository.ListAsync();
                return Ok(operationBudgetTypeRepository);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var operationBudgetTypeRepository = await _operationBudgetTypeRepository.FindByAsync(id);
                if (operationBudgetTypeRepository == null)
                {
                    return NotFound();
                }
                await _operationBudgetTypeRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OperationBudgetType operationBudgetType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    operationBudgetType.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    operationBudgetType.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    operationBudgetType.CreationDate = DateTime.Now;
                    operationBudgetType.CurrentYear = DateTime.Now.Year;
                    await _operationBudgetTypeRepository.CreateAsync(operationBudgetType);
                    //var auditBudget = new AuditBudget();
                    //auditBudget.ResourceType = operationBudgetType.Name;
                    //auditBudget.IsDeleted = operationBudgetType.IsDeleted;
                    //auditBudget.Description = operationBudgetType.Description;

                    //await _auditBudgetRepository.CreateAsync(auditBudget);
                    return NoContent();
                }
                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request to add a new role." });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] OperationBudgetType operationBudgetType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingoperationBudgetType = await _operationBudgetTypeRepository.FindByAsync(id);
                    if (existingoperationBudgetType == null)
                    {
                        return NotFound();
                    }

                    existingoperationBudgetType.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingoperationBudgetType.UpdatedDate = DateTime.Now;
                    existingoperationBudgetType.Name = operationBudgetType.Name;
                    existingoperationBudgetType.Description = operationBudgetType.Description;
                    await _operationBudgetTypeRepository.UpdateAsync(existingoperationBudgetType);

                    return NoContent();
                }

                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while processing your request to edit role with ID: {id}." });
            }
        }
    }
}
