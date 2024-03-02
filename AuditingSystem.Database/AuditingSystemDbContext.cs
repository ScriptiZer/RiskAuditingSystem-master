
using Microsoft.EntityFrameworkCore;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Entities.AuditPlan;
using AuditingSystem.Entities;
using AuditingSystem.Entities.AuditFieldWork;

namespace AuditingSystem.Database
{
    public class AuditingSystemDbContext : DbContext
    {
        public AuditingSystemDbContext()
        {
        }

        public AuditingSystemDbContext(DbContextOptions<AuditingSystemDbContext> options)
        : base(options) { }

        public virtual DbSet<Year> Years { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Quarter> Quarters { get; set; }
        public virtual DbSet<Calendar> Calendars { get; set; }

        public virtual DbSet<AuditUniverse> AuditUniverses { get; set; }
        public virtual DbSet<Industry> Industries { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Function> Functions { get; set; }
        public virtual DbSet<Entities.AuditProcess.Activity> Activities { get; set; }
        public virtual DbSet<Objective> Objectives { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Practice> Practices { get; set; }

        public virtual DbSet<ControlEffectiveness> ControlEffectivenesses { get; set; }
        public virtual DbSet<ControlFrequency> ControlFrequencies { get; set; }
        public virtual DbSet<ControlProcess> ControlProcesses { get; set; }
        public virtual DbSet<ControlType> ControlTypes { get; set; }
        public virtual DbSet<RiskCategory> RiskCategories { get; set; }
        public virtual DbSet<RiskImpact> RiskImpacts { get; set; }
        public virtual DbSet<RiskLikehood> RiskLikehoods { get; set; }

        public virtual DbSet<RiskIdentification> RiskIdentifications { get; set; }
        public virtual DbSet<Control> Controls { get; set; }
        public virtual DbSet<RiskAssessmentList> RiskAssessmentsList { get; set; }

        public virtual DbSet<AuditResources> AuditResources { get; set; }
        public virtual DbSet<AuditResourcesList> AuditResourcesLists { get; set; }
        public virtual DbSet<AuditBudget> AuditBudgets { get; set; }
        public virtual DbSet<AuditBudgetList> AuditBudgetLists { get; set; }
        public virtual DbSet<AuditResourcesListStartEndDates> AuditResourcesListStartEndDates { get; set; }
        public virtual DbSet<DraftAuditPlan> DraftAuditPlans { get; set; }
        public virtual DbSet<DraftAuditPlanList> DraftAuditPlanLists { get; set; }
        public virtual DbSet<FinalAuditPlan> FinalAuditPlans { get; set; }
        public virtual DbSet<FinalAuditPlanList> FinalAuditPlanLists { get; set; }
        public virtual DbSet<Permission>? Permissions { get; set; }
        public virtual DbSet<RolesPermissions>? RolesPermissions { get; set; }
        public virtual DbSet<AuditProgram>? AuditPrograms { get; set; }
        public virtual DbSet<AuditProgramList>? AuditProgramLists { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Industry>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Company>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Department>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Function>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Activity>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Objective>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Tasks>().Property(e => e.Id).ValueGeneratedNever();
            modelBuilder.Entity<Practice>().Property(e => e.Id).ValueGeneratedNever();
        }
    }
}
