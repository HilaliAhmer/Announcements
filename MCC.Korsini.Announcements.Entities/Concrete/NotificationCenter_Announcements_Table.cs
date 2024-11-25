using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MCC.Korsini.Announcements.Core.Entities;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_Announcements_Table : IEntity
    {
        [Key]
        public int ID { get; set; }    // Birincil anahtar, duyuru ID'si

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // EF Core, bu alanı hesaplanan bir sütun olarak görür
        public string AnnouncementId { get; private set; }    // Hesaplanan duyuru kodu (Computed Column)

        public string Title_TR { get; set; }       // Duyuru başlığı
        public string Conten_TR { get; set; }     // Duyuru içeriği
        public string Title_EN { get; set; }       // İngilizce duyuru başlığı
        public string Content_EN { get; set; }     // İngilizce duyuru içeriği
        public string Type { get; set; }           // Duyuru türü (kesinti, planlı çalışma vb.)
        public DateTime CreateDate { get; set; }   // Duyuru oluşturulma tarihi
        public int CreatedByUserId { get; set; }   // Duyuruyu oluşturan kullanıcı ID'si
        public bool IsVisibleToAll { get; set; }   // Tüm kullanıcılar görebiliyor mu?
        public bool Publishing { get; set; }       // Duyuru yayınlandı mı?

        [Required] // Bu sütun, hesaplanan `AnnouncementId` için temel sağlar
        public int AnnouncementYear { get; set; }  // Duyuru yılı (Computed Column'ın bağımlılığı)

        public virtual ICollection<NotificationCenter_Announcement_Files_Table> Files { get; set; } = new List<NotificationCenter_Announcement_Files_Table>();
    }
}