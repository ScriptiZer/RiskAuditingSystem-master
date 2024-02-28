﻿using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.api.AuditPlan
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftAuditPlansController : ControllerBase
    {
        private readonly IBaseRepository<DraftAuditPlan, int> _draftAuditPlanRepository;
        private readonly AuditingSystemDbContext _db;
        private readonly ILogger<DraftAuditPlansController> _logger;
        private readonly IBaseRepository<DraftAuditPlanList, int> _draftAuditPlanListRepository;
        private readonly IBaseRepository<FinalAuditPlan, int> _finalRepository;
        private readonly IBaseRepository<FinalAuditPlanList, int> _finalAuditPlanListRepository;

        public DraftAuditPlansController(
            IBaseRepository<DraftAuditPlan, int> draftAuditPlanRepository,
            AuditingSystemDbContext db,
            ILogger<DraftAuditPlansController> logger,
            IBaseRepository<DraftAuditPlanList, int> draftAuditPlanListRepository,
            IBaseRepository<FinalAuditPlan, int> finalRepository,
            IBaseRepository<FinalAuditPlanList, int> finalAuditPlanListRepository)
        {
            _draftAuditPlanRepository = draftAuditPlanRepository;
            _db = db;
            _logger = logger;
            _draftAuditPlanListRepository = draftAuditPlanListRepository;
            _finalRepository = finalRepository;
            _finalAuditPlanListRepository = finalAuditPlanListRepository;
        }

        [HttpPost("AddDraftDataAndList")]
        public async Task<IActionResult> AddDraftDataAndList(CombinedDraftAuditPlan? formData)
        {
            try
            {
                var draftAuditPlan = await _draftAuditPlanRepository.FindByAsync(Convert.ToInt32(formData.Id));

                if (draftAuditPlan != null)
                {
                    draftAuditPlan.IsOneYear = formData.IsOneYear;
                    draftAuditPlan.IsTwoYear = formData.IsTwoYear;
                    draftAuditPlan.IsThreeYear = formData.IsThreeYear;
                    draftAuditPlan.IA = formData.IA;
                    draftAuditPlan.GovAudit = formData.GovAudit;
                    draftAuditPlan.ELC = formData.ELC;
                    draftAuditPlan.RpetitiveFindings = formData.RpetitiveFindings;                   
                    await _draftAuditPlanRepository.UpdateAsync(draftAuditPlan);
                }

                // Assuming success, return a success response
                return Ok(new { success = true, message = "Data saved successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                _logger.LogError(ex, "An error occurred while saving data");
                return StatusCode(500, new { success = false, message = "An error occurred while saving data" });
            }
        }

        [HttpPost("SaveDesciption")]
        public async Task<IActionResult> SaveDesciption(int id, string description)
        {
            var existingauditResource = await _draftAuditPlanRepository.FindByAsync(id);
            existingauditResource.Description = description;
            await _draftAuditPlanRepository.UpdateAsync(existingauditResource);

            return Ok();
        }
    }
}
