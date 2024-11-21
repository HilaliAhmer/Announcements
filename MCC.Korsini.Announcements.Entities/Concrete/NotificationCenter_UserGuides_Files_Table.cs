using MCC.Korsini.Announcements.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_UserGuides_Files_Table : IEntity
    {
        public int ID { get; set; }              // Birincil anahtar
        public string GUID { get; set; }
        [ForeignKey("UserGuideID")]
        public int UserGuideID { get; set; }    // Prosedür tablosunun ID'sine referans
        public string FileName { get; set; }     // Dosya adı
        public string FilePath { get; set; }     // Dosya yolu
        public int UploadedByUserID { get; set; } // Dosyayı yükleyen kullanıcı ID'si
        public DateTime UploadedDate { get; set; } // Yüklenme tarihi

        // Prosedüre geri dönüş ilişkisi
        public virtual NotificationCenter_UserGuides_Table UserGuide { get; set; }
    }
}
