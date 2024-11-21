using MCC.Korsini.Announcements.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_ScheduledAnnouncements_Files_Table_Service
    {
        Task<List<NotificationCenter_ScheduledAnnouncements_Files_Table>> GetAllAsync();
        Task<NotificationCenter_ScheduledAnnouncements_Files_Table> GetByIdAsync(int announcementId);
        Task<List<NotificationCenter_ScheduledAnnouncements_Files_Table>> GetFilesByAnnouncementIdAsync(int announcementId);
        Task AddAsync(NotificationCenter_ScheduledAnnouncements_Files_Table announcement);
        Task UpdateAsync(NotificationCenter_ScheduledAnnouncements_Files_Table announcement);
        Task DeleteAsync(NotificationCenter_ScheduledAnnouncements_Files_Table announcement);
    }
}
