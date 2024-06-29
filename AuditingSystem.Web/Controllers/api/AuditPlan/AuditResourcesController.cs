using AuditingSystem.Database;
using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Controllers.api.Setup;
using AuditingSystem.Web.ViewModels;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace AuditingSystem.Web.Controllers.api.AuditPlan
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditResourcesController : ControllerBase
    {
        private readonly IBaseRepository<AuditResources, int> _auditResources;
        private readonly IBaseRepository<DraftAuditPlan, int> _draftRepository;
        private readonly AuditingSystemDbContext db;
        private readonly ILogger<AuditResourcesController> _logger;
        private readonly IBaseRepository<DraftAuditPlanList, int> _draftAuditPlanListRepository;
        private readonly IBaseRepository<FinalAuditPlanList, int> _finalAuditPlanListRepository;

        public AuditResourcesController(IBaseRepository<AuditResources, int> auditResources, ILogger<AuditResourcesController> logger,
            AuditingSystemDbContext db, IBaseRepository<DraftAuditPlan, int> draftRepository, IBaseRepository<DraftAuditPlanList, int> draftAuditPlanListRepository)
        {
            _auditResources = auditResources ?? throw new ArgumentNullException(nameof(auditResources));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.db = db;
            _draftRepository = draftRepository ?? throw new ArgumentNullException(nameof(draftRepository));
            _draftAuditPlanListRepository = draftAuditPlanListRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var auditResources = await _auditResources.ListAsync();
                return Ok(auditResources);
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
                var auditResourceToDelete = await _auditResources.FindByAsync(id);
                if (auditResourceToDelete == null)
                {
                    return NotFound();
                }

                await _auditResources.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the role with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AuditResources auditResource)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var functions = auditResource.FunctionId.Split(',');

                    var existingResource = await _auditResources.FindByAsync(r =>
                        r.CompanyId == auditResource.CompanyId &&
                        r.DepartmentId == auditResource.DepartmentId &&
                        functions.Contains(r.FunctionId));

                    if (existingResource == null)
                    {
                        foreach (var function in functions)
                        {
                            var resource = new AuditResources
                            {
                                CreatedByCompany = HttpContext.Session.GetInt32("CompanyId"),
                                CreatedBy = HttpContext.Session.GetInt32("UserId"),
                                CreationDate = DateTime.Now,
                                CurrentYear = DateTime.Now.Year,
                                FunctionId = function,
                                CompanyId = auditResource.CompanyId,
                                Name = auditResource.Name,
                                UserId = auditResource.UserId,
                                Description = auditResource.Description,
                                DepartmentId = auditResource.DepartmentId,
                                PlanStartDate = auditResource.PlanStartDate,
                                PlanEndDate = auditResource.PlanEndDate
                            };
                            await _auditResources.CreateAsync(resource);

                            var draft = new DraftAuditPlan
                            {
                                CreatedByCompany = HttpContext.Session.GetInt32("CompanyId"),
                                CreatedBy = HttpContext.Session.GetInt32("UserId"),
                                CreationDate = DateTime.Now,
                                CurrentYear = DateTime.Now.Year,
                                AuditResourceId = resource.Id,
                                CompanyId = resource.CompanyId,
                                DepartmentId = Convert.ToInt32(resource.DepartmentId),
                                FunctionId = Convert.ToInt32(resource.FunctionId)
                            };
                            await _draftRepository.CreateAsync(draft);
                        }

                        return NoContent();
                    }
                    else
                    {
                        return Conflict(new { error = "DuplicateData", message = "Audit resource already exists." });
                    }
                }

                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new audit resource.");
                return StatusCode(500, new { error = "An error occurred while processing your request to add a new audit resource.\nError Message: " + ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] AuditResources updateAuditResource)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingResource = await _auditResources.FindByAsync(r => r.CompanyId == updateAuditResource.CompanyId
                        && r.DepartmentId == updateAuditResource.DepartmentId
                        && r.FunctionId == updateAuditResource.FunctionId);

                    if (existingResource == null)
                    {
                        var existingauditResource = await _auditResources.FindByAsync(id);
                        if (existingauditResource == null)
                        {
                            return NotFound();
                        }

                        existingauditResource.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                        existingauditResource.UpdatedDate = DateTime.Now;
                        existingauditResource.CompanyId = updateAuditResource.CompanyId;
                        existingauditResource.UserId = updateAuditResource.UserId;
                        existingauditResource.DepartmentId = updateAuditResource.DepartmentId;
                        existingauditResource.FunctionId = updateAuditResource.FunctionId;
                        existingauditResource.Description = updateAuditResource.Description;

                        await _auditResources.UpdateAsync(existingauditResource);

                        return NoContent();
                    }
                    else
                    {
                        return Conflict(new { error = "DuplicateData", message = "Risk identification already exists." });
                    }
                }

                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while editing the role with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to edit role with ID: {id}.\nError Message: " + ex.Message });
            }
        }

        [HttpPost("AddResourcesList")]
        public async Task<IActionResult> AddResourcesList([FromBody] CombinedAuditResourcesModel? combinedData)
        {
            try
            {
                var auditR = db.AuditResources.Find(combinedData.ResourceId);
                auditR.AssignedToUserId = combinedData.AssignedToUserId;
                db.SaveChanges();

                var startDateEndDateList = combinedData?.StartDateEndDateList;

                if (startDateEndDateList == null)
                {
                    return BadRequest("Invalid request data");
                }

                var groupedByResourceId = startDateEndDateList.Max(a => a.AuditResourceId);
                var auditResources = db.AuditResources.Find(groupedByResourceId);

                var final = db.FinalAuditPlans.SingleOrDefault(d => d.CompanyId == auditResources.CompanyId &&
                d.DepartmentId == Convert.ToInt32(auditResources.DepartmentId) && d.FunctionId == Convert.ToInt32(auditResources.FunctionId));

                if (final != null)
                {
                    final.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    final.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    final.UpdatedDate = DateTime.Now;
                    final.CurrentYear = DateTime.Now.Year;
                    final.IsDeleted = false;
                    final.AuditResourcesId = combinedData.ResourceId;
                    final.CompanyId = auditResources.CompanyId;
                    final.DepartmentId = Convert.ToInt32(auditResources.DepartmentId);
                    final.FunctionId = Convert.ToInt32(auditResources.FunctionId);
                    // db.FinalAuditPlans.Add(final);
                    db.SaveChanges();
                }
                else
                {
                    FinalAuditPlan finalAuditPlan = new FinalAuditPlan();
                    finalAuditPlan.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    finalAuditPlan.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    finalAuditPlan.CreationDate = DateTime.Now;
                    finalAuditPlan.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    finalAuditPlan.UpdatedDate = DateTime.Now;
                    finalAuditPlan.CurrentYear = DateTime.Now.Year;
                    finalAuditPlan.IsDeleted = false;
                    finalAuditPlan.AuditResourcesId = combinedData.ResourceId;
                    finalAuditPlan.CompanyId = auditResources.CompanyId;
                    finalAuditPlan.DepartmentId = Convert.ToInt32(auditResources.DepartmentId);
                    finalAuditPlan.FunctionId = Convert.ToInt32(auditResources.FunctionId);
                    db.FinalAuditPlans.Add(finalAuditPlan);
                    db.SaveChanges();
                    final = db.FinalAuditPlans.SingleOrDefault(s => s.AuditResourcesId == combinedData.ResourceId);
                }

                foreach (var startDateEndDate in startDateEndDateList)
                {

                    var existingRecord = db.AuditResourcesListStartEndDates.FirstOrDefault(a =>
                            a.AuditResourceId == startDateEndDate.AuditResourceId &&
                            a.YearId == startDateEndDate.YearId &&
                            a.QuarterId == startDateEndDate.QuarterId);

                    //var auditResource = await _auditResources.FindByAsync(Convert.ToInt32(startDateEndDate.AuditResourceId));

                    if (existingRecord != null)
                    {
                        if (startDateEndDate.PlanStartDate.HasValue)
                        {
                            existingRecord.PlanStartDate = startDateEndDate.PlanStartDate.Value;
                        }

                        if (startDateEndDate.PlanEndDate.HasValue)
                        {
                            existingRecord.PlanEndDate = startDateEndDate.PlanEndDate.Value;
                        }

                        if (startDateEndDate.ActualStartDate.HasValue)
                        {
                            existingRecord.ActualStartDate = startDateEndDate.ActualStartDate.Value;
                        }

                        if (startDateEndDate.ActualEndDate.HasValue)
                        {
                            existingRecord.ActualEndDate = startDateEndDate.ActualEndDate.Value;
                        }

                        //existingRecord.AssignedToStartActualId = startDateEndDate.AssignedToStartActualId;
                        //existingRecord.AssignedToEndActualId = startDateEndDate.AssignedToEndActualId;

                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        db.AuditResourcesListStartEndDates.Add(startDateEndDate);

                        await db.SaveChangesAsync();
                    }

                    var existingRecord_final = db.FinalAuditPlanLists.SingleOrDefault(a =>
                       a.Year == startDateEndDate.YearId &&
                       a.FinalAuditPlanId == final.Id &&
                       a.Quarter == startDateEndDate.QuarterId);

                    if (existingRecord_final != null)
                    {
                        /*
                         1 → Plan → Gray
                         2 → Actual → Green
                         3 → Exceeded → Red
                        */
                        if (existingRecord.PlanStartDate.HasValue)
                        {
                            int year = existingRecord.PlanStartDate.Value.Year;
                            DateTime startQ1 = new DateTime(year, 1, 1);
                            DateTime endQ1 = new DateTime(year, 3, 31);
                            DateTime startQ2 = new DateTime(year, 4, 1);
                            DateTime endQ2 = new DateTime(year, 6, 30);
                            DateTime startQ3 = new DateTime(year, 7, 1);
                            DateTime endQ3 = new DateTime(year, 9, 30);
                            DateTime startQ4 = new DateTime(year, 10, 1);
                            DateTime endQ4 = new DateTime(year, 12, 31);
                            if (existingRecord.PlanStartDate.HasValue)
                            {
                                existingRecord_final.Plan = 1; // Gray
                            }
                        }
                        else
                        {
                            existingRecord_final.Plan = 0; // no color
                        }

                        if (existingRecord.PlanEndDate < existingRecord.ActualEndDate)
                        {
                            existingRecord_final.Actual = 3; // Exceeded Actual
                        }
                        else if ((existingRecord.PlanEndDate >= existingRecord.ActualEndDate))
                        {
                            existingRecord_final.Actual = 2; // Actual
                        }
                        else
                        {
                            existingRecord_final.Actual = 0; // no actual
                        }
                    }
                    else
                    {

                        var item = new FinalAuditPlanList();
                        item.FinalAuditPlanId = final.Id;
                        item.Year = startDateEndDate.YearId;
                        item.Quarter = startDateEndDate.QuarterId;
                        item.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                        item.CreatedBy = HttpContext.Session.GetInt32("UserId");
                        item.CreationDate = DateTime.Now;
                        item.CurrentYear = DateTime.Now.Year;
                        item.IsDeleted = false;


                        int year = existingRecord.PlanStartDate.HasValue ? existingRecord.PlanStartDate.Value.Year : DateTime.Now.Year;
                        DateTime startQ1 = new DateTime(year, 1, 1);
                        DateTime endQ1 = new DateTime(year, 3, 31);
                        DateTime startQ2 = new DateTime(year, 4, 1);
                        DateTime endQ2 = new DateTime(year, 6, 30);
                        DateTime startQ3 = new DateTime(year, 7, 1);
                        DateTime endQ3 = new DateTime(year, 9, 30);
                        DateTime startQ4 = new DateTime(year, 10, 1);
                        DateTime endQ4 = new DateTime(year, 12, 31);

                        if (existingRecord.PlanStartDate.HasValue)
                        {
                            item.Plan = 1; // Gray
                        }


                        if (existingRecord.PlanEndDate < startDateEndDate.ActualEndDate)
                        {
                            item.Actual = 3; // Exceeded Actual
                        }
                        else if ((existingRecord.PlanEndDate >= startDateEndDate.ActualEndDate))
                        {
                            item.Actual = 2; // Actual
                        }
                        else
                        {
                            item.Actual = 0; // no actual
                        }


                        db.FinalAuditPlanLists.Add(item);

                    }
                }

                await db.SaveChangesAsync();

                return Ok();
                //}

                //return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                var errorDetails = new
                {
                    error = "An error occurred while processing your request to add/update a list of audit resources.",
                    message = ex.Message,
                    stackTrace = ex.StackTrace,
                    resources = ex.Source // Include the data that was being processed
                };
                return StatusCode(500, errorDetails);
            }
        }


        [HttpPost("SaveDesciption")]
        public async Task<IActionResult> SaveDesciption(int id, string description)
        {
            var existingauditResource = await _auditResources.FindByAsync(id);
            existingauditResource.Description = description;
            await _auditResources.UpdateAsync(existingauditResource);

            return NoContent();
        }


    }
}
