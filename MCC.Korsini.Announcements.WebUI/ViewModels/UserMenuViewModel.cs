namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class UserMenuViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
    }
}
