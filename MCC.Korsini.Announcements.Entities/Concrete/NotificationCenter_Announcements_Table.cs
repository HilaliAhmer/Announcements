using System.ComponentModel.DataAnnotations;
using MCC.Korsini.Announcements.Core.Entities;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_Announcements_Table : IEntity
    {
        [Key]
        public int ID { get; set; }    //Birincil anahtar, duyuru ID'si
        public string AnnouncementId { get; set; }    //Duyuru kodu
        public string Title_TR { get; set; }       //Duyuru başlığı
        public string Conten_TR { get; set; }     //Duyuru içeriği
        public string Title_EN { get; set; }       //Duyuru başlığı
        public string Content_EN { get; set; }     //Duyuru içeriği
        public string Type { get; set; }        //Duyuru türü (kesinti, planlı çalışma vb.)
        public DateTime CreateDate { get; set; }        //	Duyuru Oluşturulma tarihi
        public int CreatedByUserId { get; set; }        // 	Duyuruyu oluşturan kullanıcı ID'si
        public Boolean IsVisibleToAll { get; set; }     // Tüm kullanıcılar görebiliyor mu?
        public Boolean Publishing { get; set; }         // Duyuru Yayınlandı mı?
        public virtual ICollection<NotificationCenter_Announcement_Files_Table> Files { get; set; } = new List<NotificationCenter_Announcement_Files_Table>();
    }
}
