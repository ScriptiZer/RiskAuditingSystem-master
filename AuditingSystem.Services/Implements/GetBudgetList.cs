using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Services.Interfaces;

namespace AuditingSystem.Services.Implements
{
    public class GetBudgetList 
    {
        private readonly AuditingSystemDbContext db;

        public GetBudgetList(AuditingSystemDbContext db)
        {
            this.db = db;
        }

        //public IEnumerable<AuditBudget> AuditBudgets()
        //{
        //    //var list = from b in db.AuditBudgets
        //    //           join r in db.AuditResources
        //    //           on b.AuditResourceId equals r.Id
        //    //           join c in db.Companies
        //    //           on r.CompanyId equals c.Id
        //    //           join d in db.Departments
        //    //           on Convert.ToInt32(r.DepartmentId) equals d.Id
        //    //           join u in db.Users
        //    //           on b.UserId equals u.Id
        //    //           select new GetBudgetListVM { AuditResources = r,  };

        //    //return list;
        //}
    }
}
