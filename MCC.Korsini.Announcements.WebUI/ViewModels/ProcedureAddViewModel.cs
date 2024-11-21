namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class ProcedureAddViewModel
    {
        public string Title { get; set; }
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
    }
}
