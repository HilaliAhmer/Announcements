using MCC.Korsini.Announcements.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_ScheduledAnnouncements_Table_Service
    {
        Task<List<NotificationCenter_ScheduledAnnouncements_Table>> GetAllAsync();
        Task<NotificationCenter_ScheduledAnnouncements_Table> GetByIdAsync(int announcementId);
        Task AddAsync(NotificationCenter_ScheduledAnnouncements_Table announcement);
        Task UpdateAsync(NotificationCenter_ScheduledAnnouncements_Table announcement);
        Task DeleteAsync(NotificationCenter_ScheduledAnnouncements_Table announcement);
    }
}
