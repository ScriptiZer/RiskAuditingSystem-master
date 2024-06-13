using AuditingSystem.Database;
using AuditingSystem.Entities.FollowUp;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuditingSystem.Web.Controllers.FollowUp
{
    public class SegregationOfDutiesController : Controller
    {
        private readonly IBaseRepository<FirstDuty,int> _firstDutyRepository;
        private readonly IBaseRepository<SecondDuty,int> _secondDutyRepository;
        private readonly IBaseRepository<FirstSecondDuties,int> _firstSecondDutiesRepository;
        private readonly AuditingSystemDbContext db;

        public SegregationOfDutiesController(IBaseRepository<FirstSecondDuties, int> firstSecondDutiesRepository,
            IBaseRepository<FirstDuty, int> firstDutyRepository,
            IBaseRepository<SecondDuty, int> secondDutyRepository,
            AuditingSystemDbContext db)
        {
            _firstSecondDutiesRepository = firstSecondDutiesRepository;
            _firstDutyRepository = firstDutyRepository;
            _secondDutyRepository = secondDutyRepository;
            this.db = db;
        }

        public async Task<IActionResult> Index(int sODTypeId, int page = 1, int pageSize = 20)
        {
            var firstSecondDuties = await _firstSecondDutiesRepository.ListAsync(
                new Expression<Func<FirstSecondDuties, bool>>[] { u => u.IsDeleted == false,u=>u.SODType.IsDeleted == false, u=>u.FirstDuty.IsDeleted == false, u=>u.SecondDuty.IsDeleted == false, u=>u.SODTypeId == sODTypeId },
                q => q.OrderBy(u => u.Id),
                u=>u.SODType, u=>u.FirstDuty, u=>u.SecondDuty);

            var model = firstSecondDuties.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.sODTypeId = sODTypeId;
            ViewBag.TotalRow = firstSecondDuties.Count();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(firstSecondDuties.Count() / (double)pageSize);

            var firstDuty = await _firstDutyRepository.ListAsync(
                new Expression<Func<FirstDuty, bool>>[] { u => u.IsDeleted == false && u.SODTypeId == sODTypeId },
                q => q.OrderBy(u => u.Id), null);
            var secondDuty = await _secondDutyRepository.ListAsync(
               new Expression<Func<SecondDuty, bool>>[] { u => u.IsDeleted == false && u.SODTypeId == sODTypeId },
               q => q.OrderBy(u => u.Id), null);

            ViewBag.FirstDutyId = new SelectList(firstDuty, "Id","Name");
            ViewBag.SecondDutyId = new SelectList(secondDuty, "Id","Name");

            return View(model);
        }

        [HttpGet]
        public JsonResult FilterData(int sODTypeId,int? firstDutyId, int? secondDutyId)
        {
            var filteredData = db.FirstSecondDuties.Where(s=>s.SODTypeId == sODTypeId).AsQueryable();

            if (firstDutyId.HasValue)
            {
                filteredData = filteredData.Where(item => item.FirstDutyId == firstDutyId);
            }

            if (secondDutyId.HasValue)
            {
                filteredData = filteredData.Where(item => item.SecondDutyId == secondDutyId);
            }

            var jsonData = filteredData.Select(item => new {
                id = item.Id,
                firstDutyCode = item.FirstDuty.Code,
                firstDutyName = item.FirstDuty.Name,
                secondDutyCode = item.SecondDuty.Code,
                secondDutyName = item.SecondDuty.Name,
                result = item.Result,
                risk = (item.Risk != null ? item.Risk : ""),
                mitigation = (item.Mitigation != null ? item.Mitigation : "")
            }).ToList();

            return Json(jsonData);
        }


    }
}
