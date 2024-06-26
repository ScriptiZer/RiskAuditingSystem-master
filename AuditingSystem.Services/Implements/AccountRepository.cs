﻿    using AuditingSystem.Database;
    using AuditingSystem.Entities.Setup;
    using AuditingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace AuditingSystem.Services.Implements
    {
    public class AccountRepository : IAccountRepository
    {
        private readonly AuditingSystemDbContext db;

        public AccountRepository(AuditingSystemDbContext db)
        {
            this.db = db;
        }

        //public (int? userId, string userName) Login(User user)
        //{
        //    var loggedInUser = db.Users
        //        .Include(r=>r.Role)
        //        .Where(u => u.Email.ToLower() == user.Email.ToLower() && u.Password == user.Password)
        //        .FirstOrDefault();

        //    if (loggedInUser != null)
        //    {
        //        return (loggedInUser.Id, loggedInUser.Name);
        //    }

        //    return (null, null);
        //}
        public User Login(string email, string password)
        {
            var usr = db.Users.SingleOrDefault(u => u.Email == email);

            if (usr != null)
            {
                return usr;
            }
            else
            {
                return null;
            }
        }
        public string GetUserRoleName(string userName)
        {
            return db.Users
                .Where(u => u.Name == userName)
                .Select(u => u.Role.Name)
                .FirstOrDefault();
        }

        public string GetUserRoleId(string userName)
        {
            return db.Users
                .Where(u => u.Name == userName)
                .Select(u => u.Role.Id.ToString())
                .FirstOrDefault();
        }
    }

}
