using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarsController : ControllerBase
    {
        private readonly IBaseRepository<Calendar, int> _calendarRepository;
        private readonly ILogger<CalendarsController> _logger;

        public CalendarsController(IBaseRepository<Calendar, int> calendarRepository, ILogger<CalendarsController> logger)
        {
            _calendarRepository = calendarRepository ?? throw new ArgumentNullException(nameof(calendarRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var calendar = await _calendarRepository.ListAsync();
                return Ok(calendar);
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
                var calendarToDelete = await _calendarRepository.FindByAsync(id);
                if (calendarToDelete == null)
                {
                    return NotFound();
                }

                await _calendarRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the role with ID: {id}.");
                return StatusCode(500, new { error = $"An error occurred while processing your request to delete role with ID: {id}." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Calendar calendar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _calendarRepository.CreateAsync(calendar);
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
        public async Task<IActionResult> Edit(int id, [FromBody] Calendar updatedCalendar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingCalendar = await _calendarRepository.FindByAsync(id);
                    if (existingCalendar == null)
                    {
                        return NotFound();
                    }

                    existingCalendar.Name = updatedCalendar.Name;
                    existingCalendar.Description = updatedCalendar.Description;
                    existingCalendar.CompanyId = updatedCalendar.CompanyId;
                    existingCalendar.DepartmentId = updatedCalendar.DepartmentId;
                    existingCalendar.FunctionId = updatedCalendar.FunctionId;
                    existingCalendar.UserId = updatedCalendar.UserId;
                    existingCalendar.YearId = updatedCalendar.YearId;
                    existingCalendar.FromDate = updatedCalendar.FromDate;
                    existingCalendar.ToDate = updatedCalendar.ToDate;
                    existingCalendar.DaysNumber = updatedCalendar.DaysNumber;

                    existingCalendar.DaysNumberInYear = updatedCalendar.DaysNumberInYear;
                    existingCalendar.Weekends = updatedCalendar.Weekends;
                    existingCalendar.HolidaysNumber = updatedCalendar.HolidaysNumber;
                    existingCalendar.NoofInternationalHlidays = updatedCalendar.NoofInternationalHlidays;
                    existingCalendar.NoofLeavesDays = updatedCalendar.NoofLeavesDays;
                    existingCalendar.SpecialWorkingHours = updatedCalendar.SpecialWorkingHours;
                    existingCalendar.EstimatedSickLeaves = updatedCalendar.EstimatedSickLeaves;
                    existingCalendar.BalancefromPreviousYear = updatedCalendar.BalancefromPreviousYear;
                    existingCalendar.WorkingDays = updatedCalendar.WorkingDays;

                    await _calendarRepository.UpdateAsync(existingCalendar);

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
    }
}
