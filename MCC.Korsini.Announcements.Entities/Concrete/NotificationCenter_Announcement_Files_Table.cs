using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MCC.Korsini.Announcements.Core.Entities;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_Announcement_Files_Table : IEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int AnnouncementId { get; set; } // Foreign key

        [Required]
        public string FilePath { get; set; } // Dosya yolu

        // Navigation property to Announcement
        [ForeignKey("AnnouncementId")]
        public virtual NotificationCenter_Announcements_Table Announcement { get; set; }
    }
}
