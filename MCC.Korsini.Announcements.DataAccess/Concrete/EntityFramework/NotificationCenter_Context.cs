using MCC.Korsini.Announcements.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework
{
    public class NotificationCenter_Context : DbContext

    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=10.138.10.66;Database=NotificationCenter;User ID=sa;Password=Trapper35!;TrustServerCertificate=True;Integrated Security=False");
        }

        public DbSet<NotificationCenter_Announcements_Table> NotificationCenter_Announcements_Table { get; set; }
        public DbSet<NotificationCenter_Procedures_Table> NotificationCenter_Procedures_Table { get; set; }
        public DbSet<NotificationCenter_UserGuides_Table> NotificationCenter_UserGuides_Table { get; set; }
        public DbSet<NotificationCenter_Users_Table> NotificationCenter_Users_Table { get; set; }
        public DbSet<NotificationCenter_Announcement_Files_Table> NotificationCenter_Announcement_Files_Table { get; set; }
        public DbSet<NotificationCenter_Announcement_Type_Table> NotificationCenter_Announcement_Type_Table { get; set; }
        public DbSet<NotificationCenter_ScheduledAnnouncements_Table> NotificationCenter_ScheduledAnnouncements_Table { get; set; }
        public DbSet<NotificationCenter_ScheduledAnnouncements_Files_Table> NotificationCenter_ScheduledAnnouncements_Files_Table { get; set; }
        public DbSet<NotificationCenter_Procedures_Files_Table> NotificationCenter_Procedures_Files_Table { get; set; }
        public DbSet<NotificationCenter_UserGuides_Files_Table> NotificationCenter_UserGuides_Files_Table { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationCenter_Announcements_Table>()
                .HasKey(a => a.ID);
            modelBuilder.Entity<NotificationCenter_Procedures_Table>()
                .HasMany(p => p.Files)
                .WithOne(f => f.Procedure)
                .HasForeignKey(f => f.ProcedureID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NotificationCenter_UserGuides_Table>()
                .HasMany(p => p.Files)
                .WithOne(f => f.UserGuide)
                .HasForeignKey(f => f.UserGuideID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<NotificationCenter_Users_Table>()
                .HasKey(a => a.ID);
            modelBuilder.Entity<NotificationCenter_Announcement_Files_Table>().HasKey(a => a.ID);
            modelBuilder.Entity<NotificationCenter_Announcement_Type_Table>().HasKey(a => a.ID);
            modelBuilder.Entity<NotificationCenter_ScheduledAnnouncements_Table>().HasKey(a => a.ID);
            modelBuilder.Entity<NotificationCenter_ScheduledAnnouncements_Files_Table>().HasKey(a => a.ID);
            modelBuilder.Entity<NotificationCenter_Procedures_Files_Table>().HasKey(a => a.ID);
            modelBuilder.Entity<NotificationCenter_UserGuides_Files_Table>().HasKey(a => a.ID);

            base.OnModelCreating(modelBuilder);
        }
    }

}
