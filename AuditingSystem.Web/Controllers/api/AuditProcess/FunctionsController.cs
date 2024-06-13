using AuditingSystem.Database;
using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionsController : ControllerBase
    {
        private readonly IBaseRepository<Function, int> _functionRepository;
        private readonly IBaseRepository<AuditResources, int> _auditResourcesRepository;
        private readonly IBaseRepository<DraftAuditPlan, int> _draftAuditPlanRepository;
        private readonly AuditingSystemDbContext db;

        public FunctionsController(IBaseRepository<Function, int> functionRepository,
            AuditingSystemDbContext db,
            IBaseRepository<AuditResources, int> auditResourcesRepository)
        {
            _functionRepository = functionRepository ?? throw new ArgumentNullException(nameof(functionRepository));
            this.db = db;
            _auditResourcesRepository = auditResourcesRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var functions = await _functionRepository.ListAsync();
                return Ok(functions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = $"Error retrieving functions: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _functionRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = $"Error deleting function: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Function function)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRecord = db.Functions
                    .Where(x => x.Source == "System")
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                    function.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    function.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    function.CreationDate = DateTime.Now;
                    function.CurrentYear = DateTime.Now.Year;
                    int increment = 5000;
                    if (existingRecord != null)
                    {
                        function.Id = existingRecord.Id + 1;
                        function.Source = existingRecord.Source;
                        await _functionRepository.CreateAsync(function);
                        //var auditResource = new AuditResources();
                        //auditResource.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                        //auditResource.CreatedBy = HttpContext.Session.GetInt32("UserId");
                        //auditResource.CreationDate = DateTime.Now;
                        //auditResource.CurrentYear = DateTime.Now.Year;
                        //auditResource.CompanyId = function.CompanyId;
                        //auditResource.DepartmentId = function.DepartmentId.ToString();
                        //auditResource.FunctionId = function.Id.ToString();
                        //auditResource.IsDeleted = false;
                        //await _auditResourcesRepository.CreateAsync(auditResource);
                        return NoContent();
                    }
                    else
                    {
                        function.Id = increment;
                        function.Source = "System";
                        await _functionRepository.CreateAsync(function);
                        var auditResource = new AuditResources();
                        auditResource.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                        auditResource.CreatedBy = HttpContext.Session.GetInt32("UserId");
                        auditResource.CreationDate = DateTime.Now;
                        auditResource.CurrentYear = DateTime.Now.Year;
                        auditResource.CompanyId = function.CompanyId;
                        auditResource.DepartmentId = function.DepartmentId.ToString();
                        auditResource.FunctionId = function.Id.ToString();
                        auditResource.IsDeleted = false;
                        await _auditResourcesRepository.CreateAsync(auditResource);

                        var auditDraft = new DraftAuditPlan();
                        auditDraft.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                        auditDraft.CreatedBy = HttpContext.Session.GetInt32("UserId");
                        auditDraft.CreationDate = DateTime.Now;
                        auditDraft.CurrentYear = DateTime.Now.Year;
                        auditDraft.CompanyId = function.CompanyId;
                        auditDraft.DepartmentId = function.DepartmentId;
                        auditDraft.FunctionId = function.Id;
                        auditDraft.IsDeleted = false;
                        await _draftAuditPlanRepository.CreateAsync(auditDraft);
                        return NoContent();
                    }
                }

                return BadRequest(new { error = "Invalid ModelState" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = $"Error adding function: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Function updatedFunction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingFunction = await _functionRepository.FindByAsync(id);

                    if (existingFunction == null)
                    {
                        return NotFound(new { error = $"Function with ID {id} not found" });
                    }

                    existingFunction.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingFunction.UpdatedDate = DateTime.Now;
                    existingFunction.Code = updatedFunction.Code;
                    existingFunction.Name = updatedFunction.Name;
                    existingFunction.Description = updatedFunction.Description;
                    existingFunction.IndustryId = updatedFunction.IndustryId;
                    existingFunction.CompanyId = updatedFunction.CompanyId;
                    existingFunction.DepartmentId = updatedFunction.DepartmentId;

                    await _functionRepository.UpdateAsync(existingFunction);

                    return NoContent();
                }

                return BadRequest(new { error = "Invalid ModelState" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = $"Error updating function: {ex.Message}" });
            }
        }
    }
}
