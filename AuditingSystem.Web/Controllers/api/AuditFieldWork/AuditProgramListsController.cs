using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditFieldWork;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Controllers.api.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;

namespace AuditingSystem.Web.Controllers.api.AuditFieldWork
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditProgramListsController : ControllerBase
    {
        private readonly IBaseRepository<AuditProgramList, int> _auditProgramListRepository;
        private readonly IBaseRepository<Finding, int> _findingRepository;
        private readonly ILogger<AuditProgramList> _logger;

        public AuditProgramListsController(IBaseRepository<AuditProgramList, int> auditProgramListRepository,
            ILogger<AuditProgramList> logger,
            IBaseRepository<Finding, int> findingRepository)
        {
            _auditProgramListRepository = auditProgramListRepository ?? throw new ArgumentNullException(nameof(auditProgramListRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _findingRepository = findingRepository ?? throw new ArgumentNullException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] AuditProgramListFormData formData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingAuditProgramList = await _auditProgramListRepository.FindByAsync(id);

                    if (existingAuditProgramList == null)
                    {
                        return NotFound(new { error = $"Audit Program with ID {id} not found" });
                    }

                    existingAuditProgramList.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingAuditProgramList.UpdatedDate = DateTime.Now;
                    existingAuditProgramList.Id = id;
                    existingAuditProgramList.Description = formData.Description;
                    existingAuditProgramList.TestResult = formData.TestResult;

                    var finding = await _findingRepository.FindByAsync(f =>f.Code == existingAuditProgramList.Id);
                    
                    if(finding != null)
                    {
                        if (finding.Code == existingAuditProgramList.Id)
                        {
                            finding.Name = RemoveHtmlTags(existingAuditProgramList.TestResult);
                            finding.Description = existingAuditProgramList.TestResult;
                            await _findingRepository.UpdateAsync(finding);
                        }
                    }
                    else
                    {
                        var newFinding = new Finding();
                        newFinding.Code = existingAuditProgramList.Id;
                        newFinding.Name = RemoveHtmlTags(existingAuditProgramList.TestResult);
                        newFinding.Description = existingAuditProgramList.TestResult;
                        await _findingRepository.CreateAsync(newFinding);
                    }

                    if (formData.ReferenceFiles != null)
                    { 
                        var fileNames = await SaveFiles(formData.ReferenceFiles);
                        existingAuditProgramList.Reference = string.Join(", ", fileNames);

                    }


                    await _auditProgramListRepository.UpdateAsync(existingAuditProgramList);

                    return NoContent();
                }

                return BadRequest(new { error = "Invalid ModelState" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = $"Error updating audit program: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var auditProgramListToDelete = await _auditProgramListRepository.FindByAsync(id);
                if (auditProgramListToDelete == null)
                {
                    return NotFound();
                }

                await _auditProgramListRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the role with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        private async Task<List<string>> SaveFiles(List<IFormFile> files)
        {
            var fileNames = new List<string>();

            if(files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = file.FileName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine("wwwroot/attachments", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        fileNames.Add(fileName);
                    }
                }
            }

            return fileNames;
        }

        private string RemoveHtmlTags(string input)
        {
            // Replace &nbsp; with a space
            string cleanedText = Regex.Replace(input, "&nbsp;", " ");

            // Remove other HTML tags
            cleanedText = Regex.Replace(cleanedText, "<.*?>", "");

            return cleanedText;
        }
    }
}
