using AuditingSystem.Database;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.RiskAssessments
{
    public class ControlController : Controller
    {
        private readonly IBaseRepository<Control, int> _controlRepository;
        private readonly IBaseRepository<ControlType, int> _controlTypeRepository;
        private readonly IBaseRepository<ControlProcess, int> _controlProcessRepository;
        private readonly IBaseRepository<ControlFrequency, int> _controlFrequencyRepository;
        private readonly IBaseRepository<ControlEffectiveness, int> _controlEffectivenessRepository;
        private readonly IBaseRepository<RiskIdentification, int> _riskIdentificationRepository;
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly AuditingSystemDbContext db;

        public ControlController(
            IBaseRepository<Control, int> controlRepository,
            IBaseRepository<ControlEffectiveness, int> controlEffectivenessRepository,
            IBaseRepository<ControlFrequency, int> controlFrequencyRepository,
            IBaseRepository<ControlProcess, int> controlProcessRepository,
            IBaseRepository<ControlType, int> controlTypeRepository,
            IBaseRepository<RiskIdentification, int> riskIdentificationRepository,
            IBaseRepository<User, int> userRepository,
            AuditingSystemDbContext db)
        {
            _controlRepository = controlRepository;
            _controlEffectivenessRepository = controlEffectivenessRepository;
            _controlFrequencyRepository = controlFrequencyRepository;
            _controlProcessRepository = controlProcessRepository;
            _controlTypeRepository = controlTypeRepository;
            _riskIdentificationRepository = riskIdentificationRepository;
            _userRepository = userRepository;
            this.db = db;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var currentCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

            var controlRepository = await _controlRepository.ListAsync(
                new Expression<Func<Control, bool>>[] { r => r.IsDeleted == false && r.RiskIdentification.IsDeleted == false && r.RiskIdentificationId == r.RiskIdentification.Id &&
                r.ControlType.IsDeleted == false && r.ControlProcess.IsDeleted == false && r.ControlFrequency.IsDeleted == false &&
                r.ControlEffectiveness.IsDeleted == false},
               r => r.OrderBy(o => o.Code).ThenBy(o => o.Name),
                c => c.ControlType, c => c.ControlProcess, c => c.ControlFrequency, c => c.ControlEffectiveness, c => c.RiskIdentification);

            if (currentCompany.CompanyId != 1)
            {
                controlRepository = await _controlRepository.ListAsync(
                    new Expression<Func<Control, bool>>[] { r => r.IsDeleted == false && r.RiskIdentification.IsDeleted == false && r.RiskIdentificationId == r.RiskIdentification.Id &&
                r.ControlType.IsDeleted == false && r.ControlProcess.IsDeleted == false && r.ControlFrequency.IsDeleted == false &&
                r.ControlEffectiveness.IsDeleted == false  && (r.CreatedByCompany == currentCompany.CompanyId || r.CreatedByCompany == 1)},
                   r => r.OrderBy(o => o.Code).ThenBy(o => o.Name),
                    c => c.ControlType, c => c.ControlProcess, c => c.ControlFrequency, c => c.ControlEffectiveness, c => c.RiskIdentification);
            }
            var controlType = await _controlTypeRepository.ListAsync(
                new Expression<Func<ControlType, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlProcess = await _controlProcessRepository.ListAsync(
                new Expression<Func<ControlProcess, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlFrequency = await _controlFrequencyRepository.ListAsync(
                new Expression<Func<ControlFrequency, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlEffectivenesss = await _controlEffectivenessRepository.ListAsync(
                new Expression<Func<ControlEffectiveness, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var riskIdentification = await _riskIdentificationRepository.ListAsync(
                new Expression<Func<RiskIdentification, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);


            ViewBag.ControlTypeId = new SelectList(controlType, "Id", "Name");
            ViewBag.ControlProcessId = new SelectList(controlProcess, "Id", "Name");
            ViewBag.ControlFrequencyId = new SelectList(controlFrequency, "Id", "Name");
            ViewBag.ControlEffectivenessId = new SelectList(controlEffectivenesss.Select(r => new { Id = r.Id, Name = $"{r.Rate} - {r.Name}" }), "Id", "Name");

            var model = controlRepository.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.TotalRow = controlRepository.Count();
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(controlRepository.Count() / (double)pageSize);


            return View(model);
        }

        public async Task<IActionResult> Add(int? riskIdentificationId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var controlType = await _controlTypeRepository.ListAsync(
                new Expression<Func<ControlType, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlProcess = await _controlProcessRepository.ListAsync(
                new Expression<Func<ControlProcess, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlFrequency = await _controlFrequencyRepository.ListAsync(
                new Expression<Func<ControlFrequency, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlEffectivenesss = await _controlEffectivenessRepository.ListAsync(
                new Expression<Func<ControlEffectiveness, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var riskIdentification = await _riskIdentificationRepository.ListAsync(
                new Expression<Func<RiskIdentification, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            // Filter Risk Identification based on the provided ID
            if (riskIdentificationId.HasValue)
            {
                riskIdentification = riskIdentification.Where(r => r.Id == riskIdentificationId.Value);
            }

            ViewBag.ControlTypeId = new SelectList(controlType, "Id", "Name");
            ViewBag.ControlProcessId = new SelectList(controlProcess, "Id", "Name");
            ViewBag.ControlFrequencyId = new SelectList(controlFrequency, "Id", "Name");
            ViewBag.ControlEffectivenessId = new SelectList(controlEffectivenesss.Select(r => new { Id = r.Id, Name = $"{r.Rate} - {r.Name}" }), "Id", "Name");
            var riskIdentificationList = riskIdentification.Select(r => new
            {
                Id = r.Id,
                DisplayText = RemoveHtmlTags(r.Name), // إزالة العناصر HTML
            }).ToList();

            ViewBag.RiskIdentificationId = new SelectList(
                riskIdentificationList,
                "Id",
                "DisplayText"
            );

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int? riskIdentificationId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (riskIdentificationId.HasValue)
            {
                //  await _riskIdentificationRepository.UpdateAsync(indentification);

                var controls = await _controlRepository.FindByAsync(x => x.RiskIdentificationId == riskIdentificationId);

                if (controls != null)
                {
                    var controlTypes = await _controlTypeRepository.ListAsync(
                       new Expression<Func<ControlType, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Name),
                       null);

                    var controlProcesses = await _controlProcessRepository.ListAsync(
                        new Expression<Func<ControlProcess, bool>>[] { u => u.IsDeleted == false },
                        q => q.OrderBy(u => u.Name),
                        null);

                    var controlFrequencies = await _controlFrequencyRepository.ListAsync(
                        new Expression<Func<ControlFrequency, bool>>[] { u => u.IsDeleted == false },
                        q => q.OrderBy(u => u.Name),
                        null);

                    var controlEffectivenesss = await _controlEffectivenessRepository.ListAsync(
                      new Expression<Func<ControlEffectiveness, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Rate),
                       null);

                    var riskIdentifications = await _riskIdentificationRepository.ListAsync(
                        new Expression<Func<RiskIdentification, bool>>[] { u => u.IsDeleted == false },
                        q => q.OrderBy(u => u.Name),
                        null);

                    ViewBag.ControlTypeId = new SelectList(controlTypes, "Id", "Name", controls.ControlTypeId);
                    ViewBag.ControlProcessId = new SelectList(controlProcesses, "Id", "Name", controls.ControlProcessId);
                    ViewBag.ControlFrequencyId = new SelectList(controlFrequencies, "Id", "Name", controls.ControlFrequencyId);
                    ViewBag.ControlEffectivenessId = new SelectList(controlEffectivenesss.Select(r => new { Id = r.Id, Name = $"{r.Rate} - {r.Name}" }), "Id", "Name", controls.ControlEffectivenessId);
                    var riskIdentificationLista = riskIdentifications.Select(r => new
                    {
                        Id = r.Id,
                        DisplayText = RemoveHtmlTags(r.Name),
                    }).ToList();

                    ViewBag.RiskIdentificationId = new SelectList(
                        riskIdentificationLista,
                        "Id",
                        "DisplayText",
                        controls.RiskIdentificationId
                    );
                    return View(controls);

                }
            }

            var control = await _controlRepository.FindByAsync(id);

            var controlType = await _controlTypeRepository.ListAsync(
                new Expression<Func<ControlType, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlProcess = await _controlProcessRepository.ListAsync(
                new Expression<Func<ControlProcess, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlFrequency = await _controlFrequencyRepository.ListAsync(
                new Expression<Func<ControlFrequency, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlEffectiveness = await _controlEffectivenessRepository.ListAsync(
                new Expression<Func<ControlEffectiveness, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var riskIdentification = await _riskIdentificationRepository.ListAsync(
                new Expression<Func<RiskIdentification, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            ViewBag.ControlTypeId = new SelectList(controlType, "Id", "Name", control.ControlTypeId);
            ViewBag.ControlProcessId = new SelectList(controlProcess, "Id", "Name", control.ControlProcessId);
            ViewBag.ControlFrequencyId = new SelectList(controlFrequency, "Id", "Name", control.ControlFrequencyId);
            ViewBag.ControlEffectivenessId = new SelectList(controlEffectiveness.Select(r => new { Id = r.Id, Name = $"{r.Rate} - {r.Name}" }), "Id", "Name", control.ControlEffectivenessId);
            //ViewBag.RiskIdentificationId = new SelectList(riskIdentification, "Id", "Name", control.RiskIdentificationId);
            var riskIdentificationList = riskIdentification.Select(r => new
            {
                Id = r.Id,
                DisplayText = RemoveHtmlTags(r.Name),
            }).ToList();

            ViewBag.RiskIdentificationId = new SelectList(
                riskIdentificationList,
                "Id",
                "DisplayText",
                control.RiskIdentificationId
            );
            return View(control);
        }


        public async Task<IActionResult> View(int id, int? riskIdentificationId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (riskIdentificationId.HasValue)
            {
                var controls = await _controlRepository.FindByAsync(x => x.RiskIdentificationId == riskIdentificationId);

                if (controls != null)
                {
                    var controlTypes = await _controlTypeRepository.ListAsync(
               new Expression<Func<ControlType, bool>>[] { u => u.IsDeleted == false },
               q => q.OrderBy(u => u.Name),
               null);

                    var controlProcesses = await _controlProcessRepository.ListAsync(
                        new Expression<Func<ControlProcess, bool>>[] { u => u.IsDeleted == false },
                        q => q.OrderBy(u => u.Name),
                        null);

                    var controlFrequencies = await _controlFrequencyRepository.ListAsync(
                        new Expression<Func<ControlFrequency, bool>>[] { u => u.IsDeleted == false },
                        q => q.OrderBy(u => u.Name),
                        null);

                    var controlEffectivenesss = await _controlEffectivenessRepository.ListAsync(
                      new Expression<Func<ControlEffectiveness, bool>>[] { u => u.IsDeleted == false },
                       q => q.OrderBy(u => u.Rate),
                       null);

                    var riskIdentifications = await _riskIdentificationRepository.ListAsync(
                        new Expression<Func<RiskIdentification, bool>>[] { u => u.IsDeleted == false },
                        q => q.OrderBy(u => u.Name),
                        null);

                    ViewBag.ControlTypeId = new SelectList(controlTypes, "Id", "Name", controls.ControlTypeId);
                    ViewBag.ControlProcessId = new SelectList(controlProcesses, "Id", "Name", controls.ControlProcessId);
                    ViewBag.ControlFrequencyId = new SelectList(controlFrequencies, "Id", "Name", controls.ControlFrequencyId);
                    ViewBag.ControlEffectivenessId = new SelectList(controlEffectivenesss.Select(r => new { Id = r.Id, Name = $"{r.Rate} - {r.Name}" }), "Id", "Name", controls.ControlEffectivenessId);
                    var riskIdentificationLists = riskIdentifications.Select(r => new
                    {
                        Id = r.Id,
                        DisplayText = RemoveHtmlTags(r.Name),
                    }).ToList();

                    ViewBag.RiskIdentificationId = new SelectList(
                        riskIdentificationLists,
                        "Id",
                        "DisplayText"
                    );
                    return View(controls);

                }
            }

            var control = await _controlRepository.FindByAsync(id);

            var controlType = await _controlTypeRepository.ListAsync(
                new Expression<Func<ControlType, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlProcess = await _controlProcessRepository.ListAsync(
                new Expression<Func<ControlProcess, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlFrequency = await _controlFrequencyRepository.ListAsync(
                new Expression<Func<ControlFrequency, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            var controlEffectiveness = await _controlEffectivenessRepository.ListAsync(
                new Expression<Func<ControlEffectiveness, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Rate),
                null);

            var riskIdentification = await _riskIdentificationRepository.ListAsync(
                new Expression<Func<RiskIdentification, bool>>[] { u => u.IsDeleted == false },
                q => q.OrderBy(u => u.Id),
                null);

            ViewBag.ControlTypeId = new SelectList(controlType, "Id", "Name", control.ControlTypeId);
            ViewBag.ControlProcessId = new SelectList(controlProcess, "Id", "Name", control.ControlProcessId);
            ViewBag.ControlFrequencyId = new SelectList(controlFrequency, "Id", "Name", control.ControlFrequencyId);
            ViewBag.ControlEffectivenessId = new SelectList(controlEffectiveness.Select(r => new { Id = r.Id, Name = $"{r.Rate} - {r.Name}" }), "Id", "Name", control.ControlEffectivenessId);
            ViewBag.RiskIdentificationId = new SelectList(riskIdentification, "Id", "Name", control.RiskIdentificationId);

            return View(control);
        }

        private string RemoveHtmlTags(string input)
        {
            // Replace &nbsp; with a space
            string cleanedText = Regex.Replace(input, "&nbsp;", " ");

            // Remove other HTML tags
            cleanedText = Regex.Replace(cleanedText, "<.*?>", "");

            return cleanedText;
        }

        [HttpGet]
        public IActionResult FilterRiskControlTable(int controlTypeId, int controlProcessId, int controlFrequencyId, int controlEffectivenessId)
        {
            var query = from control in db.Controls
                        join identification in db.RiskIdentifications on control.RiskIdentificationId equals identification.Id
                        join controlType in db.ControlTypes on control.ControlTypeId equals controlType.Id
                        join controlProcess in db.ControlProcesses on control.ControlProcessId equals controlProcess.Id
                        join controlFrequency in db.ControlFrequencies on control.ControlFrequencyId equals controlFrequency.Id
                        join controlEffectiveness in db.ControlEffectivenesses on control.ControlEffectivenessId equals controlEffectiveness.Id
                        where
                        control.IsDeleted == false && identification.IsDeleted == false &&
                        controlType.IsDeleted == false && controlProcess.IsDeleted == false &&
                        controlFrequency.IsDeleted == false && controlEffectiveness.IsDeleted == false
                        orderby control.Code
                        select new RiskControlVM
                        {
                            Control = control,
                            ControlType = controlType,
                            ControlProcess = controlProcess,
                            ControlFrequency = controlFrequency,
                            ControlEffectiveness = controlEffectiveness,
                            RiskIdentification = identification
                        };

            if (controlTypeId != 0)
            {
                query = query.Where(f => f.Control.ControlTypeId == controlTypeId);
            }

            if (controlProcessId != 0)
            {
                query = query.Where(f => f.Control.ControlProcessId == controlProcessId);
            }

            if (controlFrequencyId != 0)
            {
                query = query.Where(f => f.Control.ControlFrequencyId == controlFrequencyId);
            }

            if (controlEffectivenessId != 0)
            {
                query = query.Where(f => f.Control.ControlEffectivenessId == controlEffectivenessId);
            }

            var riskControl = query.OrderBy(c => c.Control.Code)
                                .ToList();

            int totalRow = riskControl.Count();
            double totalControl = 0.0;
            foreach (var control in query)
            {
                totalControl += Convert.ToDouble(control.Control.ControlEffectivenessRating);
            }
            var result = new
            {
                Data = riskControl.Select(a => new
                {
                    id = a.Control.Id,
                    controlCode = a.Control.Code,
                    controlName = a.Control.Name,
                    controlDescription = a.Control.Description,
                    controlTypeName = a.ControlType.Name,
                    controlTypeBGColor = a.ControlType.BGColor,
                    controlTypeFontColor = a.ControlType.FontColor,
                    controlProcessName = a.ControlProcess.Name,
                    controlProcessBGColor = a.ControlProcess.BGColor,
                    controlProcessFontColor = a.ControlProcess.FontColor,
                    controlFrequencyName = a.ControlFrequency.Name,
                    controlFrequencyBGColor = a.ControlFrequency.BGColor,
                    controlFrequencyFontColor = a.ControlFrequency.FontColor,
                    controlEffectivenessName = a.ControlEffectiveness.Name,
                    controlEffectivenessBGColor = a.ControlEffectiveness.BGColor,
                    controlEffectivenessFontColor = a.ControlEffectiveness.FontColor,
                    riskIdentificationName = a.RiskIdentification.Name,
                    controlEffectivenessRating = a.ControlEffectiveness.Rate
                }).ToList(),
                totalControlRow = totalRow,
                totalRiskControl = (totalControl > 1) ? (totalControl / totalRow) : totalRow
            };


            return Json(result);
        }

    }
}
