using System.ComponentModel.DataAnnotations;
using MCC.Korsini.Announcements.Core.Entities;

namespace MCC.Korsini.Announcements.Entities.Concrete;

public class NotificationCenter_ScheduledAnnouncements_Table : IEntity
{
    [Key]
    public int ID { get; set; }    //Birincil anahtar, duyuru ID'si
    public string Title_TR { get; set; }       //Duyuru başlığı
    public string Conten_TR { get; set; }     //Duyuru içeriği
    public string Title_EN { get; set; }       //Duyuru başlığı
    public string Content_EN { get; set; }     //Duyuru içeriği
    public string Type { get; set; }        //Duyuru türü (kesinti, planlı çalışma vb.)
    public DateTime CreateDate { get; set; }        //	Duyuru Oluşturulma tarihi
    public string ScheduleType { get; set; }        //Planlama tipi (Örn: "MonthlyFirstMonday", "Monthly15th", vs.)
    public string ScheduleTypeShow { get; set; }        //Planlama tipi (Örn: "MonthlyFirstMonday", "Monthly15th", vs.)
    public DateTime ScheduledDate { get; set; }     //  Planlanan gönderim tarihi
    public DateTime? NextRunTime { get; set; }       //Bir sonraki çalıştırma zamanı
    public Boolean IsActive { get; set; }        //  Planlamanın aktif olup olmadığını belirtmek için?
    public int CreatedByUserId { get; set; }        // 	Duyuruyu oluşturan kullanıcı ID'si
    public virtual ICollection<NotificationCenter_ScheduledAnnouncements_Files_Table> Files { get; set; } = new List<NotificationCenter_ScheduledAnnouncements_Files_Table>(); // NotificationCenter_ScheduledAnnouncements_Files_Table ile ilişkisi
}