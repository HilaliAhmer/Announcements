namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class AnnouncementDetailViewModel
    {
        public int ID { get; set; }
        public string AnnouncementId { get; set; }
        public string Title_TR { get; set; }
        public string Content_TR { get; set; }
        public string Title_EN { get; set; }
        public string Content_EN { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
        public Boolean Publishing { get; set; }
        public List<(string FullPath, string DisplayName)> FilePaths { get; set; } = new List<(string, string)>();
    }
}
