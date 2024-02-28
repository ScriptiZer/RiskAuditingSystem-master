using AuditingSystem.Database;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services;
using AuditingSystem.Services.Implements;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditingSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaseRepository<User, int> _userRepository;
        private readonly AuditingSystemDbContext _db;
        private readonly IUserContext _userContext;
        private readonly IAccountRepository _accountRepository;
        private readonly PermissionService _permissionService;
        public HomeController(IBaseRepository<User, int> userRepository, AuditingSystemDbContext db,
            IUserContext userContext, IAccountRepository accountRepository, PermissionService permissionService)
        {
            _userRepository = userRepository;
            _db = db;
            _userContext = userContext;
            _accountRepository = accountRepository;
            _permissionService = permissionService;
        }

        public  IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var userName = HttpContext.Session.GetInt32("UserName");

                if (userId == null)
                    return RedirectToAction("Login", "Account");
                _userContext.UserName = userName.ToString();

                string roleName = _accountRepository.GetUserRoleName(userName.ToString());

                //_userContext.UserPermissions = _permissionService.GetPermissionsForUserRole(roleName);

                var getCompany = await _userRepository.FindByAsync(Convert.ToInt32(userId));

                var companyId = getCompany.CompanyId;

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("AuditingSystemConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    //1. SP → ActualBudgetPlanperMonth

                    using (var command = new SqlCommand("ActualBudgetPlanperMonth", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // إضافة المعلمة
                        command.Parameters.Add(new SqlParameter("@companyId", companyId));
                        var months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        var actualValues = new List<double>();
                        var estimatedValues = new List<double>();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var type = reader.GetString(reader.GetOrdinal("Type"));
                                if (type.Equals("Actual", StringComparison.OrdinalIgnoreCase))
                                {
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Jan")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Jan")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Feb")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Feb")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Mar")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Mar")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Apr")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Apr")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("May")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("May")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Jun")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Jun")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Jul")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Jul")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Aug")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Aug")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Sep")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Sep")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Oct")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Oct")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Nov")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Nov")));
                                    actualValues.Add(reader.IsDBNull(reader.GetOrdinal("Dec")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Dec")));
                                }
                                else if (type.Equals("Estimated", StringComparison.OrdinalIgnoreCase))
                                {
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Jan")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Jan")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Feb")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Feb")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Mar")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Mar")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Apr")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Apr")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("May")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("May")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Jun")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Jun")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Jul")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Jul")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Aug")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Aug")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Sep")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Sep")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Oct")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Oct")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Nov")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Nov")));
                                    estimatedValues.Add(reader.IsDBNull(reader.GetOrdinal("Dec")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("Dec")));
                                }
                            }

                        }

                        ViewData["Months"] = months;
                        ViewData["ActualValues"] = actualValues;
                        ViewData["EstimatedValues"] = estimatedValues;

                        var totalActual = 0.0;
                        var totalEstimated = 0.0;
                        foreach(var item in actualValues)
                        {
                            totalActual += item;
                        }
                        ViewData["totalActual"] = totalActual;
                        foreach (var item in estimatedValues)
                        {
                            totalEstimated += item;
                        }
                        ViewData["totalEstimated"] = totalEstimated;
                    }

                    //2. SP → TotalPlanandActualperDepartment
                    using (var command = new SqlCommand("TotalPlanandActualperDepartment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@companyId", companyId));

                        var departments = new List<string>();
                        var actualValues = new List<int>();
                        var estimatedValues = new List<int>();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                var department = reader.GetString(reader.GetOrdinal("DepartmentName"));
                                var actualValue = reader.GetInt32(reader.GetOrdinal("Actual"));
                                var estimatedValue = reader.GetInt32(reader.GetOrdinal("Plan"));

                                departments.Add(department);
                                actualValues.Add(actualValue);
                                estimatedValues.Add(estimatedValue);
                            }
                        }

                        ViewData["Departments"] = departments;
                        ViewData["ActualValuesperDepartment"] = actualValues;
                        ViewData["PlanValuesperDepartment"] = estimatedValues;

                        var totalActual = 0.0;
                        var totalPlan = 0.0;
                        foreach (var item in actualValues)
                        {
                            totalActual += item;
                        }
                        ViewData["totalActualperDepartment"] = totalActual;
                        foreach (var item in estimatedValues)
                        {
                            totalPlan += item;
                        }
                        ViewData["totalPlanperDepartment"] = totalPlan;
                    }

                }

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
