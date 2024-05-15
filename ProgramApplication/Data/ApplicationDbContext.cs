using ProgramApplication.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace ProgramApplication.Data
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //public DbSet<Question> Questions { get; set; }
        /////<remark/>
        //public DbSet<ApplicationForm> ApplicationForms { get; set; }
        /////<remark/>
        //public DbSet<CandidateApplication> CandidateApplications { get; set; }
        /////<remark/>

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Question>()
            //    .Property(a => a.Type)
            //    .HasConversion(
            //        u => u.ToString(),
            //        u => Enum.Parse<QuestionType>(u, true));
        }


        public int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }

            catch (Exception)
            {
                throw;
            }
        }

    }
}
