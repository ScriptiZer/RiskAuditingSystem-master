﻿using AuditingSystem.Database;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Implements;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace AuditingSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly PermissionService _permissionService;
        private readonly IUserContext _userContext;


        public AccountController(IAccountRepository accountRepository, PermissionService permissionService,
            IUserContext userContext)
        {
            _accountRepository = accountRepository;
            _permissionService = permissionService;
            _userContext = userContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            (int? userId, string userName) = _accountRepository.Login(user);

            if (userId.HasValue)
            {
                HttpContext.Session.SetInt32("UserId", userId.Value);
                HttpContext.Session.SetString("UserName", userName);

                _userContext.UserName = userName;

                string roleName = _accountRepository.GetUserRoleName(userName);

                _userContext.UserPermissions = _permissionService.GetPermissionsForUserRole(roleName);

                HttpContext.Session.SetString("UserPermissions", JsonConvert.SerializeObject(_userContext.UserPermissions, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));


                return RedirectToAction("Dashboard", "Home");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }
    }

    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

    }
}
