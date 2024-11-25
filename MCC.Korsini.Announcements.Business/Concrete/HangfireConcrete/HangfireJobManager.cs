using Hangfire;
using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.Business.Abstract.AnnouncementMailAbstract;
using MCC.Korsini.Announcements.Business.Abstract.HangfireAbstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using Microsoft.Extensions.Configuration;

namespace MCC.Korsini.Announcements.Business.Concrete.HangfireConcrete
{
    public class HangfireJobManager : IHangfireJobService
    {
        private readonly IAnnouncementMailService _announcementMailService;
        private readonly INotificationCenter_ScheduledAnnouncements_Table_Service _scheduledAnnouncementsService;
        private readonly INotificationCenter_ScheduledAnnouncements_Files_Table_Service _scheduledAnnouncementsFilesService;
        private readonly INotificationCenter_Announcements_Table_Service _announcementsTableService;
        private readonly INotificationCenter_Announcement_Files_Table_Service _announcementFilesTableService;
        private readonly IConfiguration _configuration;

        public HangfireJobManager(
            IAnnouncementMailService announcementMailService,
            INotificationCenter_ScheduledAnnouncements_Table_Service scheduledAnnouncementsService,
            INotificationCenter_ScheduledAnnouncements_Files_Table_Service announcementsFilesService, INotificationCenter_Announcements_Table_Service announcementsTableService, INotificationCenter_Announcement_Files_Table_Service announcementFilesTableService, IConfiguration configuration)
        {
            _announcementMailService = announcementMailService;
            _scheduledAnnouncementsService = scheduledAnnouncementsService;
            _scheduledAnnouncementsFilesService = announcementsFilesService;
            _announcementsTableService = announcementsTableService;
            _announcementFilesTableService = announcementFilesTableService;
            _configuration = configuration;
        }
        private async Task SendEmail(NotificationCenter_ScheduledAnnouncements_Table announcement, string ID)
        {
            var htmlContent = $@"
                <div>
                    <h3>{announcement.Title_TR}</h3>
                    <div>{announcement.Conten_TR}</div>
                    <hr style='margin-top: 20px; margin-bottom: 20px;' />
                    <h3>{announcement.Title_EN}</h3>
                    <div>{announcement.Content_EN}</div>
                    <p><small>{announcement.CreateDate}</small></p>
                </div>";

            string recipientEmail = _configuration["SmtpSettings:RecipientEmail"];
            string subject = $"{ID} - [{announcement.Type}]: {announcement.Title_TR} / {announcement.Title_EN}";
            var files = await _scheduledAnnouncementsFilesService.GetFilesByAnnouncementIdAsync(announcement.ID);
            await _announcementMailService.SendAnnouncementEmailAsync(
                recipientEmail,
                subject,
                htmlContent,
                files.Select(f => f.FilePath).ToList()
            );
        }

        private DateTime CalculateFirstMondayOfNextMonth()
        {
            var currentDay = DateTime.Now;
            var firstDayOfNextMonth = new DateTime(currentDay.Year, currentDay.Month, 1).AddMonths(1);
            while (firstDayOfNextMonth.DayOfWeek != DayOfWeek.Monday)
            {
                firstDayOfNextMonth = firstDayOfNextMonth.AddDays(1);
            }
            return firstDayOfNextMonth.AddHours(10).AddMinutes(30); // Saat 10:30 olarak ayarlanır
        }

        private DateTime AdjustToNextMondayIfWeekend(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
                return date.AddDays(2); // Cumartesi -> Pazartesi
            if (date.DayOfWeek == DayOfWeek.Sunday)
                return date.AddDays(1); // Pazar -> Pazartesi
            return date; // Hafta içindeyse değişiklik yapma
        }
        public void ScheduleAnnouncementWithHangfire(NotificationCenter_ScheduledAnnouncements_Table scheduledAnnouncement)
        {
            switch (scheduledAnnouncement.ScheduleType)
            {
                case "MonthlyFirstMonday":
                    RecurringJob.AddOrUpdate(
                        scheduledAnnouncement.ID.ToString(),
                        () => ExecuteMonthlyFirstMondayScheduledAnnouncement(scheduledAnnouncement.ID),
                        "30 10 * * 1",
                        TimeZoneInfo.Local); // Her ayın ilk Pazartesi 10:30
                    break;

                case "Monthly15th":
                    RecurringJob.AddOrUpdate(
                        scheduledAnnouncement.ID.ToString(),
                        () => ExecuteMonthlyOnBesthScheduledAnnouncement(scheduledAnnouncement.ID),
                        $"30 10 {scheduledAnnouncement.NextRunTime.Value.Day} * *", // Cron ifadesi: her ayın belirli bir günü saat 10:30
                        TimeZoneInfo.Local);
                    break;

                case "WeeklyMonday":
                    RecurringJob.AddOrUpdate(
                        scheduledAnnouncement.ID.ToString(),
                        () => ExecuteWeeklyMondayScheduledAnnouncement(scheduledAnnouncement.ID),
                        Cron.Weekly(DayOfWeek.Monday, hour: 10, minute: 30),
                        TimeZoneInfo.Local); // Her hafta Pazartesi
                    break;

                default:
                    BackgroundJob.Schedule(
                        () => ExecuteScheduledAnnouncement(scheduledAnnouncement.ID),
                        scheduledAnnouncement.ScheduledDate
                    );
                    break;
            }
        }
        public void ScheduleAnnouncement(int announcementId, DateTime scheduledTime)
        {
            var scheduledAnnouncement = _scheduledAnnouncementsService.GetByIdAsync(announcementId).Result;
            if (scheduledAnnouncement != null)
            {
                ScheduleAnnouncementWithHangfire(scheduledAnnouncement);
            }
        }

        public async Task ExecuteScheduledAnnouncement(int announcementId)
        {
            var announcement = await _scheduledAnnouncementsService.GetByIdAsync(announcementId);
            if (announcement != null && announcement.IsActive)
            {
                var files = await _scheduledAnnouncementsFilesService.GetFilesByAnnouncementIdAsync(announcement.ID);
                //var announcementFiles = files.Select(f => new NotificationCenter_Announcement_Files_Table
                //{
                //    FilePath = f.FilePath
                //}).ToList();
                var announcementsTable = new NotificationCenter_Announcements_Table
                {
                    Title_TR = announcement.Title_TR,
                    Conten_TR = announcement.Conten_TR,
                    Title_EN = announcement.Title_EN,
                    Content_EN = announcement.Content_EN,
                    CreateDate = DateTime.Now,
                    CreatedByUserId = 1,
                    IsVisibleToAll = true,
                    Publishing = true,
                    Type = announcement.Type,
                    AnnouncementYear = DateTime.Now.Year
                };
                // 4. Duyuruyu veritabanına ekle ve kaydet
                await _announcementsTableService.AddAsync(announcementsTable);

                // 5. SaveChanges işlemiyle ID'yi kesinleştir
                var savedAnnouncementId = announcementsTable.ID;

                // 6. Dosyaları ilişkilendir ve tek tek kaydet
                foreach (var file in files)
                {
                    var announcementFile = new NotificationCenter_Announcement_Files_Table
                    {
                        AnnouncementId = savedAnnouncementId, // Artık kesinleşmiş bir ID'yi kullanıyoruz
                        FilePath = file.FilePath
                    };
                    await _announcementFilesTableService.AddAsync(announcementFile);
                }

                await SendEmail(announcement, announcementsTable.AnnouncementId);
                announcement.IsActive = false; // Tek seferlik olduğu için pasif hale getir
                await _scheduledAnnouncementsService.UpdateAsync(announcement);
            }
        }

        public async Task ExecuteMonthlyFirstMondayScheduledAnnouncement(int announcementId)
        {
            var announcement = await _scheduledAnnouncementsService.GetByIdAsync(announcementId);
            if (announcement != null && announcement.IsActive)
            {
                var files = await _scheduledAnnouncementsFilesService.GetFilesByAnnouncementIdAsync(announcement.ID);
                //var announcementFiles = files.Select(f => new NotificationCenter_Announcement_Files_Table
                //{
                //    FilePath = f.FilePath
                //}).ToList();
                var announcementsTable = new NotificationCenter_Announcements_Table
                {
                    Title_TR = announcement.Title_TR,
                    Conten_TR = announcement.Conten_TR,
                    Title_EN = announcement.Title_EN,
                    Content_EN = announcement.Content_EN,
                    CreateDate = DateTime.Now,
                    CreatedByUserId = 1,
                    IsVisibleToAll = true,
                    Publishing = true,
                    Type = announcement.Type,
                    AnnouncementYear = DateTime.Now.Year
                };
                // 4. Duyuruyu veritabanına ekle ve kaydet
                await _announcementsTableService.AddAsync(announcementsTable);

                // 5. SaveChanges işlemiyle ID'yi kesinleştir
                var savedAnnouncementId = announcementsTable.ID;

                // 6. Dosyaları ilişkilendir ve tek tek kaydet
                foreach (var file in files)
                {
                    var announcementFile = new NotificationCenter_Announcement_Files_Table
                    {
                        AnnouncementId = savedAnnouncementId, // Artık kesinleşmiş bir ID'yi kullanıyoruz
                        FilePath = file.FilePath
                    };
                    await _announcementFilesTableService.AddAsync(announcementFile);
                }

                await SendEmail(announcement, announcementsTable.AnnouncementId);
                announcement.NextRunTime = CalculateFirstMondayOfNextMonth();
                await _scheduledAnnouncementsService.UpdateAsync(announcement);
            }
        }

        public async Task ExecuteMonthlyOnBesthScheduledAnnouncement(int announcementId)
        {
            var announcement = await _scheduledAnnouncementsService.GetByIdAsync(announcementId);
            if (announcement != null && announcement.IsActive)
            {
                var files = await _scheduledAnnouncementsFilesService.GetFilesByAnnouncementIdAsync(announcement.ID);
                //var announcementFiles = files.Select(f => new NotificationCenter_Announcement_Files_Table
                //{
                //    FilePath = f.FilePath
                //}).ToList();
                var announcementsTable = new NotificationCenter_Announcements_Table
                {
                    Title_TR = announcement.Title_TR,
                    Conten_TR = announcement.Conten_TR,
                    Title_EN = announcement.Title_EN,
                    Content_EN = announcement.Content_EN,
                    CreateDate = DateTime.Now,
                    CreatedByUserId = 1,
                    IsVisibleToAll = true,
                    Publishing = true,
                    Type = announcement.Type,
                    AnnouncementYear = DateTime.Now.Year
                };
                // 4. Duyuruyu veritabanına ekle ve kaydet
                await _announcementsTableService.AddAsync(announcementsTable);

                // 5. SaveChanges işlemiyle ID'yi kesinleştir
                var savedAnnouncementId = announcementsTable.ID;

                // 6. Dosyaları ilişkilendir ve tek tek kaydet
                foreach (var file in files)
                {
                    var announcementFile = new NotificationCenter_Announcement_Files_Table
                    {
                        AnnouncementId = savedAnnouncementId, // Artık kesinleşmiş bir ID'yi kullanıyoruz
                        FilePath = file.FilePath
                    };
                    await _announcementFilesTableService.AddAsync(announcementFile);
                }

                await SendEmail(announcement, announcementsTable.AnnouncementId);
                announcement.NextRunTime = AdjustToNextMondayIfWeekend(
                    new DateTime(announcement.NextRunTime.Value.Year, announcement.NextRunTime.Value.Month, 15).AddMonths(1)
                );
                await _scheduledAnnouncementsService.UpdateAsync(announcement);
            }
        }

        public async Task ExecuteWeeklyMondayScheduledAnnouncement(int announcementId)
        {
            var announcement = await _scheduledAnnouncementsService.GetByIdAsync(announcementId);
            if (announcement != null && announcement.IsActive)
            {
                var files = await _scheduledAnnouncementsFilesService.GetFilesByAnnouncementIdAsync(announcement.ID);
                //var announcementFiles = files.Select(f => new NotificationCenter_Announcement_Files_Table
                //{
                //    FilePath = f.FilePath
                //}).ToList();
                var announcementsTable = new NotificationCenter_Announcements_Table
                {
                    Title_TR = announcement.Title_TR,
                    Conten_TR = announcement.Conten_TR,
                    Title_EN = announcement.Title_EN,
                    Content_EN = announcement.Content_EN,
                    CreateDate = DateTime.Now,
                    CreatedByUserId = 1,
                    IsVisibleToAll = true,
                    Publishing = true,
                    Type = announcement.Type,
                    AnnouncementYear = DateTime.Now.Year
                };
                // 4. Duyuruyu veritabanına ekle ve kaydet
                await _announcementsTableService.AddAsync(announcementsTable);

                // 5. SaveChanges işlemiyle ID'yi kesinleştir
                var savedAnnouncementId = announcementsTable.ID;

                // 6. Dosyaları ilişkilendir ve tek tek kaydet
                foreach (var file in files)
                {
                    var announcementFile = new NotificationCenter_Announcement_Files_Table
                    {
                        AnnouncementId = savedAnnouncementId, // Artık kesinleşmiş bir ID'yi kullanıyoruz
                        FilePath = file.FilePath
                    };
                    await _announcementFilesTableService.AddAsync(announcementFile);
                }

                await SendEmail(announcement, announcementsTable.AnnouncementId);
                announcement.NextRunTime = announcement.NextRunTime?.AddDays(7).Date.AddHours(10).AddMinutes(30);
                await _scheduledAnnouncementsService.UpdateAsync(announcement);
            }
        }


    }
}
