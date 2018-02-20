using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using APIServer.Models;

namespace APIServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Primary keys
            builder.Entity<Calendar>().HasKey(c => c.CalendarID);
            builder.Entity<Event>().HasKey(e => e.EventID);
            builder.Entity<Subscription>().HasKey(s => new { s.UserID, s.CalendarID });
            // One to many relationships
            builder.Entity<Calendar>()
                .HasOne(c => c.Owner).WithMany(u => u.OwnedCalendars)
                .HasForeignKey(c => c.OwnerID).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Event>()
                .HasOne(e => e.Calendar).WithMany(c => c.Events)
                .HasForeignKey(e => e.CalendarID).OnDelete(DeleteBehavior.Restrict);
            // Bridge Table
            builder.Entity<Subscription>()
                .HasOne(s => s.Calendar).WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.CalendarID).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Subscription>()
                .HasOne(s => s.User).WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserID).OnDelete(DeleteBehavior.Restrict);
            // Experimental unique column https://stackoverflow.com/questions/41246614/entity-framework-core-add-unique-constraint-code-first
            builder.Entity<Calendar>().HasIndex(c => c.AccessCode).IsUnique();

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
