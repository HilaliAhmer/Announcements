using MCC.Korsini.Announcements.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_ScheduledAnnouncements_Table_Manager : INotificationCenter_ScheduledAnnouncements_Table_Service
    {
        private readonly INotificationCenter_ScheduledAnnouncements_Table_Dal _scheduledAnnouncements;

        public NotificationCenter_ScheduledAnnouncements_Table_Manager(INotificationCenter_ScheduledAnnouncements_Table_Dal scheduledAnnouncements)
        {
            _scheduledAnnouncements = scheduledAnnouncements;
        }

        public Task<List<NotificationCenter_ScheduledAnnouncements_Table>> GetAllAsync()
        {
            return _scheduledAnnouncements.GetListAsync();
        }

        public Task<NotificationCenter_ScheduledAnnouncements_Table> GetByIdAsync(int announcementId)
        {
            return _scheduledAnnouncements.GetAsync(s => s.ID == announcementId);
        }

        public Task AddAsync(NotificationCenter_ScheduledAnnouncements_Table announcement)
        {
            return _scheduledAnnouncements.AddAsync(announcement);
        }

        public Task UpdateAsync(NotificationCenter_ScheduledAnnouncements_Table announcement)
        {
            return _scheduledAnnouncements.UpdateAsync(announcement);
        }

        public Task DeleteAsync(NotificationCenter_ScheduledAnnouncements_Table announcement)
        {
            return _scheduledAnnouncements.DeleteAsync(announcement);
        }
    }
}
