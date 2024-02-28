using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.Controllers.api.Setup;
using AuditingSystem.Web.ViewModels;
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

                    var function = auditResource.FunctionId.Split(',');
                    foreach(var item in function)
                    {
                        var resource = new AuditResources();
                        resource.FunctionId = item;
                        resource.CompanyId = auditResource.CompanyId;
                        resource.Name = auditResource.Name;
                        resource.UserId = auditResource.UserId;
                        resource.Description = auditResource.Description;
                        resource.DepartmentId = auditResource.DepartmentId;
                        resource.PlanStartDate =  auditResource.PlanStartDate;
                        resource.PlanEndDate = auditResource.PlanEndDate;
                        await _auditResources.CreateAsync(resource);

                        var draft = new DraftAuditPlan();
                        draft.AuditResourceId = resource.Id;
                        draft.CompanyId = resource.CompanyId;
                        draft.DepartmentId = Convert.ToInt32(resource.DepartmentId);
                        draft.FunctionId = Convert.ToInt32(resource.FunctionId);
                        await _draftRepository.CreateAsync(draft);
                    }
                   
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
        public async Task<IActionResult> Edit(int id, [FromBody] AuditResources updateAuditResource)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingauditResource = await _auditResources.FindByAsync(id);
                    if (existingauditResource == null)
                    {
                        return NotFound();
                    }

                    existingauditResource.CompanyId = updateAuditResource.CompanyId;
                    existingauditResource.UserId = updateAuditResource.UserId;
                    existingauditResource.DepartmentId = updateAuditResource.DepartmentId; 
                    existingauditResource.FunctionId = updateAuditResource.FunctionId; 
                    existingauditResource.Description = updateAuditResource.Description;

                    await _auditResources.UpdateAsync(existingauditResource);

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

        [HttpPost("AddResourcesList")]
        public async Task<IActionResult> AddResourcesList([FromBody] CombinedAuditResourcesModel? combinedData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var auditResourcesList = combinedData?.auditResourcesLists;
                    var startDateEndDateList = combinedData?.StartDateEndDateList;

                    if (auditResourcesList == null || startDateEndDateList == null)
                    {
                        return BadRequest("Invalid request data");
                    }

                    var groupedByResourceId = auditResourcesList.Max(a=>a.AuditResourceId);

                    var groupedPlanByResourceType = auditResourcesList
                        .GroupBy(a => a.ResourceType)
                        .Select(group => new
                        {
                            ResourceType = group.Key,
                            TotalPlan = group.Sum(a => a.Plan)
                        })
                        .ToList();

                    var insourceTotalPlan = groupedPlanByResourceType.FirstOrDefault(g => g.ResourceType == "Insource")?.TotalPlan;
                    var OutsourceTotalPlan = groupedPlanByResourceType.FirstOrDefault(g => g.ResourceType == "Outsource")?.TotalPlan;
                    var managerTotalPlan = groupedPlanByResourceType.FirstOrDefault(g => g.ResourceType == "Manager")?.TotalPlan;

                    var draft = db.DraftAuditPlans.SingleOrDefault(d => d.AuditResourceId == groupedByResourceId);

                    foreach (var auditResource in auditResourcesList)
                    {
                        var existingRecord = db.AuditResourcesLists.FirstOrDefault(a =>
                            a.AuditResourceId == auditResource.AuditResourceId &&
                            a.UserId == auditResource.UserId &&
                            a.ResourceType == auditResource.ResourceType);

                        if (existingRecord != null)
                        {
                            existingRecord.Plan = auditResource.Plan;
                            existingRecord.Actual = auditResource.Actual;
                        }
                        else
                        {
                            db.AuditResourcesLists.Add(auditResource);
                        }

                        if(draft != null)
                        {
                            if (auditResource.ResourceType == "Insource" && insourceTotalPlan.HasValue)
                            {
                                draft.Insource = insourceTotalPlan.Value;
                            }
                            if (auditResource.ResourceType == "Outsource" && OutsourceTotalPlan.HasValue)
                            {
                                draft.Outsource = OutsourceTotalPlan.Value;
                            }
                            if (auditResource.ResourceType == "Manager" && managerTotalPlan.HasValue)
                            {
                                draft.Manager = managerTotalPlan.Value;
                            }
                        }
                    }

                    foreach (var startDateEndDate in startDateEndDateList)
                    {
                        var existingRecord = db.AuditResourcesListStartEndDates.FirstOrDefault(a =>
                            a.AuditResourceId == startDateEndDate.AuditResourceId &&
                            a.YearId == startDateEndDate.YearId &&
                            a.QuarterId == startDateEndDate.QuarterId);

                        var auditResource = await _auditResources.FindByAsync(Convert.ToInt32(startDateEndDate.AuditResourceId));

                        if (existingRecord != null)
                        {
                            existingRecord.ActualStartDate = startDateEndDate.ActualStartDate;
                            existingRecord.ActualEndDate = startDateEndDate.ActualEndDate;
                            existingRecord.AssignedToStartActualId = startDateEndDate.AssignedToStartActualId;
                            existingRecord.AssignedToEndActualId = startDateEndDate.AssignedToEndActualId;

                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            db.AuditResourcesListStartEndDates.Add(startDateEndDate);
                        }

                        var existingRecord_Draft = db.DraftAuditPlanLists.FirstOrDefault(a =>
                           a.Year == startDateEndDate.YearId &&
                           a.DraftAuditPlanId == draft.Id &&
                           a.Quarter == startDateEndDate.QuarterId);

                        if (existingRecord_Draft != null)
                        {
                            /*
                             1 → Plan → Gray
                             2 → Actual → Green
                             3 → Exceeded → Red
                            */
                            if (auditResource.PlanStartDate != null)
                            {
                                int year = auditResource.PlanStartDate.Value.Year;
                                DateTime startQ1 = new DateTime(year, 1, 1);
                                DateTime endQ1 = new DateTime(year, 3, 31);
                                DateTime startQ2 = new DateTime(year, 4, 1);
                                DateTime endQ2 = new DateTime(year, 6, 30);
                                DateTime startQ3 = new DateTime(year, 7, 1);
                                DateTime endQ3 = new DateTime(year, 9, 30);
                                DateTime startQ4 = new DateTime(year, 10, 1);
                                DateTime endQ4 = new DateTime(year, 12, 31);
                                /*
                                 &&
                                    (existingRecord_Draft.Year == startDateEndDate.YearId) &&
                                    (existingRecord_Draft.Quarter == startDateEndDate.QuarterId) && 
                                    (draft.Id == existingRecord_Draft.DraftAuditPlanId
                                 */
                                if (auditResource.PlanStartDate.Value >= startQ1 && auditResource.PlanStartDate.Value <= endQ1 && existingRecord_Draft.Quarter == "Q1" && draft.Id == existingRecord_Draft.DraftAuditPlanId)
                                {
                                    existingRecord_Draft.Plan = 1; // Gray
                                }
                                else if ((auditResource.PlanStartDate.Value >= startQ2 && auditResource.PlanStartDate.Value <= endQ2 && existingRecord_Draft.Quarter == "Q2" && draft.Id == existingRecord_Draft.DraftAuditPlanId))
                                {
                                    existingRecord_Draft.Plan = 1; // Gray
                                }
                                else if ((auditResource.PlanStartDate.Value >= startQ3 && auditResource.PlanStartDate.Value <= endQ3 && existingRecord_Draft.Quarter == "Q3" && draft.Id == existingRecord_Draft.DraftAuditPlanId))
                                {
                                    existingRecord_Draft.Plan = 1; // Gray
                                }
                                else if (auditResource.PlanStartDate.Value >= startQ4 && auditResource.PlanStartDate.Value <= endQ4 && existingRecord_Draft.Quarter == "Q4" && draft.Id == existingRecord_Draft.DraftAuditPlanId)
                                {
                                    existingRecord_Draft.Plan = 1; // Gray
                                }
                                else
                                {
                                    existingRecord_Draft.Plan = 0; // no plan
                                }
                            }
                            else
                            {
                                existingRecord_Draft.Plan = 0; // no color
                            }

                            if (auditResource.PlanEndDate < startDateEndDate.ActualEndDate)
                            {
                                existingRecord_Draft.Actual = 3; // Exceeded Actual
                            }
                            else if ((auditResource.PlanEndDate >= startDateEndDate.ActualEndDate))
                            {
                                existingRecord_Draft.Actual = 2; // Actual
                            }
                            else
                            {
                                existingRecord_Draft.Actual = 0; // no actual
                            }
                        }
                        else
                        {
                            var item = new DraftAuditPlanList();
                            item.DraftAuditPlanId = draft.Id;
                            item.Year = startDateEndDate.YearId;
                            item.Quarter = startDateEndDate.QuarterId;
                            if (auditResource.PlanStartDate != null)
                            {
                                int year = auditResource.PlanStartDate.Value.Year;
                                DateTime startQ1 = new DateTime(year, 1, 1);
                                DateTime endQ1 = new DateTime(year, 3, 31);
                                DateTime startQ2 = new DateTime(year, 4, 1);
                                DateTime endQ2 = new DateTime(year, 6, 30);
                                DateTime startQ3 = new DateTime(year, 7, 1);
                                DateTime endQ3 = new DateTime(year, 9, 30);
                                DateTime startQ4 = new DateTime(year, 10, 1);
                                DateTime endQ4 = new DateTime(year, 12, 31);

                                if (auditResource.PlanStartDate.Value >= startQ1 && auditResource.PlanStartDate.Value <= endQ1)
                                {
                                    item.Plan = 1; // Gray
                                }
                                else if (auditResource.PlanStartDate.Value >= startQ2 && auditResource.PlanStartDate.Value <= endQ2)
                                {
                                    item.Plan = 1; // Gray
                                }
                                else if (auditResource.PlanStartDate.Value >= startQ3 && auditResource.PlanStartDate.Value <= endQ3)
                                {
                                    item.Plan = 1; // Gray
                                }
                                else if (auditResource.PlanStartDate.Value >= startQ4 && auditResource.PlanStartDate.Value <= endQ4)
                                {
                                    item.Plan = 1; // Gray
                                }
                                else
                                {
                                    item.Plan = 0; // no color
                                }


                                if (auditResource.PlanEndDate < startDateEndDate.ActualEndDate)
                                {
                                    item.Actual = 3; // Exceeded Actual
                                }
                                else if ((auditResource.PlanEndDate >= startDateEndDate.ActualEndDate))
                                {
                                    item.Actual = 2; // Actual
                                }
                                else
                                {
                                    item.Actual = 0; // no actual
                                }


                                //if (auditResource.PlanStartDate != null)
                                //{
                                //    item.Plan = 1; // Gray
                                //}
                                //else
                                //{
                                //    item.Plan = 0; // no plan
                                //}

                                //if (auditResource.PlanEndDate < startDateEndDate.ActualEndDate)
                                //{
                                //    item.Actual = 3; // Exceeded Actual
                                //}
                                //else if ((auditResource.PlanEndDate >= startDateEndDate.ActualEndDate))
                                //{
                                //    item.Actual = 2; // Actual
                                //}
                                //else
                                //{
                                //    item.Actual = 0; // no actual
                                //}
                                await _draftAuditPlanListRepository.CreateAsync(item);
                            }

                        }
                    }

                    await db.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding/updating a list of audit resources.");
                return StatusCode(500, new { error = "An error occurred while processing your request to add/update a list of audit resources." });
            }
        }



        [HttpPost("SaveDesciption")]
        public async Task<IActionResult> SaveDesciption(int id,string description)
        {
            var existingauditResource = await _auditResources.FindByAsync(id);
            existingauditResource.Description = description;
            await _auditResources.UpdateAsync(existingauditResource);

            return NoContent();
        }


    }
}
