namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class ScheduledAnnouncementListViewModel
    {
        public int ID { get; set; }    //Birincil anahtar, duyuru ID'si
        public string ScheduledAnnouncementId { get; set; }    //Duyuru kodu
        public string Title_TR { get; set; }       //Duyuru başlığı
        public string Conten_TR { get; set; }     //Duyuru içeriği
        public string Title_EN { get; set; }       //Duyuru başlığı
        public string Content_EN { get; set; }     //Duyuru içeriği
        public string Type { get; set; }        //Duyuru türü (kesinti, planlı çalışma vb.)
        public DateTime CreateDate { get; set; }        //	Duyuru Oluşturulma tarihi
        public string ScheduleType { get; set; }        //Planlama tipi (Örn: "MonthlyFirstMonday", "Monthly15th", vs.)
        public DateTime ScheduledDate { get; set; }     //  Planlanan gönderim tarihi
        public string ScheduleTypeShow { get; set; }        //Planlama tipi (Örn: "MonthlyFirstMonday", "Monthly15th", vs.)
        public DateTime? NextRunTime { get; set; }       //Bir sonraki çalıştırma zamanı
        public Boolean IsActive { get; set; }        //  Planlamanın aktif olup olmadığını belirtmek için?
        public int CreatedByUserId { get; set; }        // 	Duyuruyu oluşturan kullanıcı ID'si
    }
}