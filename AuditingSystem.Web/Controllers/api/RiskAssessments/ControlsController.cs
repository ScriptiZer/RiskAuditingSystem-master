using AuditingSystem.Database;
using AuditingSystem.Entities.AuditFieldWork;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.api.RiskAssessment
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlsController : ControllerBase
    {
        private readonly IBaseRepository<Control, int> _controlRepository;
        private readonly IBaseRepository<RiskAssessmentList, int> _riskAssessmentListRepository;
        private readonly IBaseRepository<RiskIdentification, int> _riskIdentificationRepository;
        private readonly IBaseRepository<AuditProgram, int> _auditProgramRepository;
        private readonly IBaseRepository<AuditProgramList, int> _auditProgramListRepository;
        private readonly IBaseRepository<AuditResources, int> _auditResourcesRepository;
        private readonly IBaseRepository<DraftAuditPlan, int> _auditDraftRepository;
        private readonly IBaseRepository<FinalAuditPlan, int> _auditFinalRepository;

        public ControlsController(
            IBaseRepository<Control, int> controlRepository,
            IBaseRepository<RiskAssessmentList, int> riskAssessmentListRepository,
            IBaseRepository<RiskIdentification, int> riskIdentificationRepository,
            IBaseRepository<AuditProgram, int> auditProgramRepository,
            IBaseRepository<AuditProgramList, int> auditProgramListRepository,
            IBaseRepository<AuditResources, int> auditResourcesRepository,
            IBaseRepository<DraftAuditPlan, int> auditDraftRepository,
            IBaseRepository<FinalAuditPlan, int> auditFinalRepository)
        {
            _controlRepository = controlRepository ?? throw new ArgumentNullException(nameof(controlRepository));
            _riskAssessmentListRepository = riskAssessmentListRepository ?? throw new ArgumentNullException(nameof(riskAssessmentListRepository));
            _riskIdentificationRepository = riskIdentificationRepository ?? throw new ArgumentNullException(nameof(riskIdentificationRepository));
            _auditProgramRepository = auditProgramRepository ?? throw new ArgumentNullException(nameof(auditProgramRepository));
            _auditProgramListRepository = auditProgramListRepository ?? throw new ArgumentNullException(nameof(auditProgramListRepository));
            _auditResourcesRepository = auditResourcesRepository ?? throw new ArgumentNullException(nameof(auditResourcesRepository));
            _auditDraftRepository = auditDraftRepository;
            _auditFinalRepository = auditFinalRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var controls = await _controlRepository.ListAsync();
                return Ok(controls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving controls: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _controlRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error deleting control: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Control control)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var controlRepository = await _controlRepository.FindByAsync(c => c.RiskIdentificationId == control.RiskIdentificationId);

                    if (controlRepository == null)
                    {
                        control.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                        control.CreatedBy = HttpContext.Session.GetInt32("UserId");
                        control.CreationDate = DateTime.Now;
                        control.CurrentYear = DateTime.Now.Year;
                        await _controlRepository.CreateAsync(control);

                        var controlEntity = await _controlRepository.FindByAsync(control.Id);
                        var inherentRisk = await _riskIdentificationRepository.FindByAsync((int)control.RiskIdentificationId);

                        var controlRate = controlEntity.ControlEffectivenessRating;
                        var inherentRate = inherentRisk?.InherentRiskRating ?? 0;

                        var riskAssessment = new RiskAssessmentList()
                        {
                            CreatedByCompany = HttpContext.Session.GetInt32("CompanyId"),
                            CreatedBy = HttpContext.Session.GetInt32("UserId"),
                            CreationDate = DateTime.Now,
                            CurrentYear = DateTime.Now.Year,
                            ControlId = controlEntity.Id,
                            RiskIdentificationId = controlEntity.RiskIdentificationId,
                            Description = "test",
                            Name = "test"
                        };

                        var resource = await _auditResourcesRepository.FindByAsync(x=>x.CompanyId == inherentRisk.CompanyId && Convert.ToInt32(x.DepartmentId) == inherentRisk.DepartmentId && x.FunctionId == x.FunctionId);
                        if(resource == null)
                        {
                            var auditResource = new AuditResources();
                            auditResource.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                            auditResource.CreatedBy = HttpContext.Session.GetInt32("UserId");
                            auditResource.CreationDate = DateTime.Now;
                            auditResource.CurrentYear = DateTime.Now.Year;
                            auditResource.CompanyId = inherentRisk.CompanyId;
                            auditResource.DepartmentId = inherentRisk.DepartmentId.ToString();
                            auditResource.FunctionId = inherentRisk.FunctionId.ToString();
                            auditResource.IsDeleted = false;
                            await _auditResourcesRepository.CreateAsync(auditResource);
                        }

                        var draft = await _auditDraftRepository.FindByAsync(x => x.CompanyId == inherentRisk.CompanyId && Convert.ToInt32(x.DepartmentId) == inherentRisk.DepartmentId && x.FunctionId == x.FunctionId);
                        if(draft == null)
                        {
                            var auditDraft = new DraftAuditPlan();
                            auditDraft.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                            auditDraft.CreatedBy = HttpContext.Session.GetInt32("UserId");
                            auditDraft.CreationDate = DateTime.Now;
                            auditDraft.CurrentYear = DateTime.Now.Year;
                            auditDraft.CompanyId = inherentRisk.CompanyId;
                            auditDraft.DepartmentId = inherentRisk.DepartmentId;
                            auditDraft.FunctionId = inherentRisk.FunctionId;
                            auditDraft.IsDeleted = false;
                            await _auditDraftRepository.CreateAsync(auditDraft);
                        }

                        var final = await _auditFinalRepository.FindByAsync(x => x.CompanyId == inherentRisk.CompanyId && Convert.ToInt32(x.DepartmentId) == inherentRisk.DepartmentId && x.FunctionId == x.FunctionId);
                        if(final == null)
                        {
                            var auditFinal = new FinalAuditPlan();
                            auditFinal.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                            auditFinal.CreatedBy = HttpContext.Session.GetInt32("UserId");
                            auditFinal.CreationDate = DateTime.Now;
                            auditFinal.CurrentYear = DateTime.Now.Year;
                            auditFinal.CompanyId = inherentRisk.CompanyId;
                            auditFinal.DepartmentId = inherentRisk.DepartmentId;
                            auditFinal.FunctionId = inherentRisk.FunctionId;
                            auditFinal.IsDeleted = false;
                            await _auditFinalRepository.CreateAsync(auditFinal);
                        }


                        var rateNo = inherentRisk.InherentRiskRating - (control.ControlEffectivenessRating * 50 / 100);
                        riskAssessment.ResidualRiskRatingNumber = rateNo;

                        if (rateNo <= 2)
                        {
                            riskAssessment.ResidualRiskRating = "No major concern";
                        }
                        else if (rateNo <= 4)
                        {
                            riskAssessment.ResidualRiskRating = "Periodic Monitoring";
                        }
                        else if (rateNo < 6)
                        {
                            riskAssessment.ResidualRiskRating = "Continuous Review";
                        }
                        else if (rateNo >= 6)
                        {
                            riskAssessment.ResidualRiskRating = "Active Management";
                        }
                        else
                        {
                            riskAssessment.ResidualRiskRating = "-";
                        }

                        await _riskAssessmentListRepository.CreateAsync(riskAssessment);

                        var programChick = await _auditProgramRepository.FindByAsync
                                            (i => i.CompanyId == inherentRisk.CompanyId && i.DepartmentId == inherentRisk.DepartmentId);

                        if (programChick == null)
                        {
                            var auditProgram = new AuditProgram()
                            {
                                CompanyId = inherentRisk.CompanyId,
                                DepartmentId = inherentRisk.DepartmentId
                            };
                            await _auditProgramRepository.CreateAsync(auditProgram);
                        }

                        var programData = await _auditProgramRepository.FindByAsync
                            (i => i.CompanyId == inherentRisk.CompanyId && i.DepartmentId == inherentRisk.DepartmentId);

                        var auditProgramList = new AuditProgramList()
                        {
                            AuditProgramId = programData.Id,
                            RiskIdenticationId = Convert.ToInt32(controlEntity.RiskIdentificationId),
                            ControlId = controlEntity.Id
                        };
                        await _auditProgramListRepository.CreateAsync(auditProgramList);

                        return NoContent();
                    }
                    else
                    {
                        return Conflict(new { error = "DuplicateData", message = "Risk Control already exists." });
                    }

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal Server Error: {ex.Message}" });
            }

            return BadRequest();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Control control)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var controlRepository = await _controlRepository.FindByAsync(c => c.RiskIdentificationId == control.RiskIdentificationId);


                    //if (controlRepository == null)
                    //{
                        var existingControl = await _controlRepository.FindByAsync(id);
                        if (existingControl == null)
                            return NotFound();
                        var identification = await _riskIdentificationRepository.FindByAsync(Convert.ToInt32(control.RiskIdentificationId));
                        existingControl.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                        existingControl.UpdatedDate = DateTime.Now;
                        existingControl.Code = control.Code;
                        existingControl.Name = control.Name;
                        existingControl.Description = control.Description;
                        existingControl.ControlTypeId = control.ControlTypeId;
                        existingControl.ControlProcessId = control.ControlProcessId;
                        existingControl.ControlFrequencyId = control.ControlFrequencyId;
                        existingControl.ControlEffectivenessId = control.ControlEffectivenessId;
                        existingControl.ControlEffectivenessRating = control.ControlEffectivenessRating;
                        existingControl.RiskIdentificationId = control.RiskIdentificationId;

                        await _controlRepository.UpdateAsync(existingControl);


                        var programChick = await _auditProgramRepository.FindByAsync
                                            (i => i.CompanyId == identification.CompanyId && i.DepartmentId == identification.DepartmentId);

                        if (programChick == null)
                        {
                            var auditProgram = new AuditProgram()
                            {
                                CompanyId = identification.CompanyId,
                                DepartmentId = identification.DepartmentId
                            };
                            await _auditProgramRepository.CreateAsync(auditProgram);
                        }

                        var programListData = await _auditProgramListRepository.FindByAsync
                            (c => c.ControlId == existingControl.Id &&
                            c.RiskIdenticationId == existingControl.RiskIdentificationId &&
                            c.AuditProgramId == programChick.Id);

                        if (programListData == null)
                        {
                            var auditProgramList = new AuditProgramList()
                            {
                                AuditProgramId = programChick.Id,
                                RiskIdenticationId = Convert.ToInt32(existingControl.RiskIdentificationId),
                                ControlId = existingControl.Id
                            };
                            await _auditProgramListRepository.CreateAsync(auditProgramList);
                        }

                        return NoContent();
                    //}
                    //else
                    //{
                    //    return Conflict(new { error = "DuplicateData", message = "Risk Control already exists." });
                    //}
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error updating control: {ex.Message}" });
            }

            return BadRequest();
        }
    }
}
