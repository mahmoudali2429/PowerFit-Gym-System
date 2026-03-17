using Microsoft.EntityFrameworkCore;
using PowerFitDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerFitDAL.Data.Contexts
{
    public class PowerFitDbContext : DbContext
    {
        public PowerFitDbContext(DbContextOptions<PowerFitDbContext> options) : base(options)
        {

        } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #region DbSets
        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }
        #endregion
    }
}
