using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebLMS.Models;

namespace WebLMS.Data
{
    public class LMSDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<CodingHomework> CodingHomeworks { get; set; }
        public DbSet<CodingTest> CodingTests { get; set; }
        public DbSet<CodingHomeworkTestRun> CodingHomeworkTestRuns { get; set; }

        public LMSDbContext(DbContextOptions<LMSDbContext> options)
            : base(options)
        {
        }
    }
}
