using AuditingSystem.Database;
using AuditingSystem.Entities.AuditFieldTests;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IBaseRepository<Company, int> _companyRepository;
        private readonly AuditingSystemDbContext db;
        private readonly ISplitRepository<int> _SplitCompany;
        public CompaniesController(IBaseRepository<Company, int> companyRepository,
            AuditingSystemDbContext db,
            ISplitRepository<int> SplitCompany)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            this.db = db;
            _SplitCompany = SplitCompany;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyRepository.ListAsync();
            return Ok(companies);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _companyRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error deleting company: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Company company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRecord = db.Companies
                      .Where(x => x.Source == "System")
                      .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                    company.CreatedByCompany = HttpContext.Session.GetInt32("CompanyId");
                    company.CreatedBy = HttpContext.Session.GetInt32("UserId");
                    company.CreationDate = DateTime.Now;
                    company.CurrentYear = DateTime.Now.Year;
                    int increment = 5000;
                    if (existingRecord != null)
                    {
                        company.Id = existingRecord.Id + 1;
                        company.Source = existingRecord.Source;
                        await _companyRepository.CreateAsync(company);
                        
                    }
                    else
                    {
                        company.Id = increment;
                        company.Source = "System";
                        await _companyRepository.CreateAsync(company);
                       
                    }

                    var insource = company.BudgetInsources.Split(',');
                    foreach (var item in insource)
                    {
                        var budget = new AuditBudget();
                        budget.IsDeleted = false;
                        budget.ResourceId = Convert.ToInt32(item);
                        budget.ResourceType = "Insource";
                        budget.CompanyId = company.Id;
                        db.AuditBudgets.Add(budget);
                        db.SaveChanges();
                    }

                    var outsource = company.BudgetOutsources.Split(',');
                    foreach (var item in outsource)
                    {
                        var budget = new AuditBudget();
                        budget.IsDeleted = false;
                        budget.ResourceId = Convert.ToInt32(item);
                        budget.ResourceType = "Outsource";
                        budget.CompanyId = company.Id;
                        db.AuditBudgets.Add(budget);
                        db.SaveChanges();
                    }

                    var manager = company.BudgetManager.Split(',');
                    foreach (var item in manager)
                    {
                        var budget = new AuditBudget();
                        budget.IsDeleted = false;
                        budget.ResourceId = Convert.ToInt32(item);
                        budget.ResourceType = "Manager";
                        budget.CompanyId = company.Id;
                        db.AuditBudgets.Add(budget);
                        db.SaveChanges();
                    }

                    return NoContent();

                }

                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error adding company: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Company updatedCompany)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingCompany = await _companyRepository.FindByAsync(id);

                    if (existingCompany == null)
                    {
                        return NotFound();
                    }

                    existingCompany.UpdatedBy = HttpContext.Session.GetInt32("UserId");
                    existingCompany.UpdatedDate = DateTime.Now;
                    existingCompany.Code = updatedCompany.Code;
                    existingCompany.Name = updatedCompany.Name;
                    existingCompany.Description = updatedCompany.Description;
                    existingCompany.Address = updatedCompany.Address;
                    existingCompany.ContactNo = updatedCompany.ContactNo;
                    existingCompany.Email = updatedCompany.Email;
                    existingCompany.IndustryId = updatedCompany.IndustryId;
                    existingCompany.PlanYear = updatedCompany.PlanYear;
                    existingCompany.Insources = updatedCompany.Insources;
                    existingCompany.Outsources = updatedCompany.Outsources;
                    existingCompany.Manager = updatedCompany.Manager;
                    existingCompany.BudgetPlanYear = updatedCompany.BudgetPlanYear;
                    existingCompany.BudgetInsources = updatedCompany.BudgetInsources;
                    existingCompany.BudgetOutsources = updatedCompany.BudgetOutsources;
                    existingCompany.BudgetManager = updatedCompany.BudgetManager;

                    await _companyRepository.UpdateAsync(existingCompany);

                    var insourceSplit = _SplitCompany.SplitData(existingCompany.BudgetInsources);
                    var outsourceSplit = _SplitCompany.SplitData(existingCompany.BudgetOutsources);
                    var managerSplit = _SplitCompany.SplitData(existingCompany.Manager);

                    foreach(var item in insourceSplit)
                    {
                        var budgetInsource = db.AuditBudgets.SingleOrDefault(a => a.UserId == item && a.ResourceType == "Insource");
                        if(budgetInsource == null)
                        {
                            var insource = existingCompany.BudgetInsources.Split(',');
                            foreach (var sitem in insource)
                            {
                                var budget = new AuditBudget();
                                budget.IsDeleted = false;
                                budget.ResourceId = Convert.ToInt32(sitem);
                                budget.ResourceType = "Insource";
                                budget.CompanyId = existingCompany.Id;
                                db.AuditBudgets.Add(budget);
                                db.SaveChanges();
                            }
                        }
                    }

                  

                    var outsource = existingCompany.BudgetOutsources.Split(',');
                    foreach (var item in outsource)
                    {
                        var budget = new AuditBudget();
                        budget.IsDeleted = false;
                        budget.ResourceId = Convert.ToInt32(item);
                        budget.ResourceType = "Outsource";
                        budget.CompanyId = existingCompany.Id;
                        db.AuditBudgets.Add(budget);
                        db.SaveChanges();
                    }

                    var manager = existingCompany.BudgetManager.Split(',');
                    foreach (var item in manager)
                    {
                        var budget = new AuditBudget();
                        budget.IsDeleted = false;
                        budget.ResourceId = Convert.ToInt32(item);
                        budget.ResourceType = "Manager";
                        budget.CompanyId = existingCompany.Id;
                        db.AuditBudgets.Add(budget);
                        db.SaveChanges();
                    }
                    return NoContent();
                }

                return BadRequest("Invalid ModelState");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error updating company: {ex.Message}" });
            }
        }
    }
}
