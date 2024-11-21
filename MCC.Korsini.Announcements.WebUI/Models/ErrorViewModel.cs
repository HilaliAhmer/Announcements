namespace MCC.Korsini.Announcements.WebUI.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string? Message { get; set; }
        public string? Path { get; set; }

    }
}
