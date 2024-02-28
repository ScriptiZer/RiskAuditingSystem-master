using AuditingSystem.Database;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Services.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AuditingSystem.Services.Implements
{
    public class Dashboard_ActualBudgetPlanperMonth : IDashboard_ActualBudgetPlanperMonth
    {
        private readonly AuditingSystemDbContext _db;

        public Dashboard_ActualBudgetPlanperMonth(AuditingSystemDbContext db)
        {
            _db = db;
        }


    }
}
