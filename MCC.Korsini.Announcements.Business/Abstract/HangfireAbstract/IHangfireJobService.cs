using MCC.Korsini.Announcements.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Abstract.HangfireAbstract
{
    public interface IHangfireJobService
    {
        /// <summary>
        /// Planlanmış bir duyuruyu belirli bir zamanda çalışacak şekilde planlar.
        /// </summary>
        /// <param name="announcementId">Duyuru ID'si.</param>
        /// <param name="scheduledTime">Planlanan zaman.</param>
        void ScheduleAnnouncement(int announcementId, DateTime scheduledTime);

        /// <summary>
        /// Planlanmış bir duyuruyu Hangfire ile belirli bir cron ifadesine göre zamanlar.
        /// </summary>
        /// <param name="scheduledAnnouncement">Planlanmış duyuru nesnesi.</param>
        void ScheduleAnnouncementWithHangfire(NotificationCenter_ScheduledAnnouncements_Table scheduledAnnouncement);

        /// <summary>
        /// Planlanmış bir duyuruyu gönderir. Bu metot Hangfire job olarak çalışacaktır.
        /// </summary>
        /// <param name="announcementId">Duyuru ID'si.</param>
        /// <returns>Task</returns>
        Task ExecuteScheduledAnnouncement(int announcementId);
        Task ExecuteMonthlyFirstMondayScheduledAnnouncement(int announcementId);
        Task ExecuteMonthlyOnBesthScheduledAnnouncement(int announcementId);
        Task ExecuteWeeklyMondayScheduledAnnouncement(int announcementId);
    }
}
