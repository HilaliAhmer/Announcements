namespace MCC.Korsini.Announcements.Business.Abstract.AnnouncementMailAbstract
{
    public interface IAnnouncementMailService
    {
        Task SendAnnouncementEmailAsync(string recipientEmail, string subject, string htmlContent, List<string> attachmentPaths);
    }
}
