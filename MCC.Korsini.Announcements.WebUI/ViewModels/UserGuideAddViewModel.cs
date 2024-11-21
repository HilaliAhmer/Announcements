namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class UserGuideAddViewModel
    {
        public string Title { get; set; }
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
    }
}
