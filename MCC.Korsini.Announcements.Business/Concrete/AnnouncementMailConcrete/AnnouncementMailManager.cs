using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using MCC.Korsini.Announcements.Business.Abstract.AnnouncementMailAbstract;

namespace MCC.Korsini.Announcements.Business.Concrete.AnnouncementMailConcrete
{
    public class AnnouncementMailManager : IAnnouncementMailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _senderEmail;

        public AnnouncementMailManager(IConfiguration configuration)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings");
            _senderEmail = smtpSettings["SenderEmail"];

            _smtpClient = new SmtpClient(smtpSettings["Server"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new System.Net.NetworkCredential(smtpSettings["Username"], smtpSettings["Password"])
            };
        }
        public async Task SendAnnouncementEmailAsync(string recipientEmail, string subject, string htmlContent, List<string> attachmentPaths)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_senderEmail);
                mailMessage.To.Add(recipientEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = htmlContent;
                mailMessage.IsBodyHtml = true;

                // Dosya eklerini mailMessage'a ekliyoruz
                if (attachmentPaths != null && attachmentPaths.Any())
                {
                    foreach (var filePath in attachmentPaths)
                    {
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filePath);

                        if (File.Exists(fullPath))
                        {
                            // GUID kısmını kaldırmak için dosya adını işliyoruz
                            var fileName = Path.GetFileName(filePath);
                            var parts = fileName.Split('_', 2);
                            var displayName = (parts.Length > 1 && Guid.TryParse(parts[0], out _)) ? parts[1] : fileName;

                            // Attachment nesnesine displayName ile ekliyoruz
                            var attachment = new Attachment(fullPath)
                            {
                                Name = displayName // E-posta ekinde görünen ad olarak düzenlenmiş dosya adını ayarlıyoruz
                            };
                            mailMessage.Attachments.Add(attachment);
                        }
                    }
                }

                await _smtpClient.SendMailAsync(mailMessage);
            }
        }

    }
}
