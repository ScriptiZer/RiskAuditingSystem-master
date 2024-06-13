using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditFieldWork;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.FollowUp;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.FollowUp
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegregationOfDutiesAPIController : ControllerBase
    {
        private readonly IBaseRepository<FirstSecondDuties, int> _firstSecondDutiesReposiotory;

        public SegregationOfDutiesAPIController(IBaseRepository<FirstSecondDuties, int> firstSecondDutiesReposiotory)
        {
            _firstSecondDutiesReposiotory = firstSecondDutiesReposiotory;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] FirstSecondDuties formData)
        {
            try
            {
                var data = await _firstSecondDutiesReposiotory.FindByAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                data.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                data.UpdatedDate = DateTime.Now;
                data.Mitigation = formData.Mitigation;
                data.Risk = formData.Risk;
                await _firstSecondDutiesReposiotory.UpdateAsync(data); // تحديث الكائن data بدلاً من formData
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = $"Error updating Segregation of Duties: {ex.Message}" });
            }
        }
    }
}
