using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MCC.Korsini.Announcements.Core.Entities;

namespace MCC.Korsini.Announcements.Entities.Concrete;

public class NotificationCenter_ScheduledAnnouncements_Files_Table : IEntity
{
    [Key]
    public int ID { get; set; }

    [Required]
    public int ScheduledAnnouncementId { get; set; } // Foreign key

    [Required]
    public string FilePath { get; set; } // Dosya yolu


    // Navigation property to Announcement
    [ForeignKey("ScheduledAnnouncementId")]
    public virtual NotificationCenter_ScheduledAnnouncements_Table ScheduledAnnouncements { get; set; }
}