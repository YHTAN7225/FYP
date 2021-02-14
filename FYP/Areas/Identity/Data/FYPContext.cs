using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FYP.Data
{
    public class FYPContext : IdentityDbContext<IdentityUser>
    {
        public FYPContext(DbContextOptions<FYPContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<AdminAccess> AdminAccess { get; set; }
        public DbSet<UserAccess> UserAccess { get; set; }
        public DbSet<LinkStatus> LinkStatus { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequest { get; set; }
    }
}
