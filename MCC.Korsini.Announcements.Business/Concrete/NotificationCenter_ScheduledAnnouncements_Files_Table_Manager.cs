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
    public class NotificationCenter_ScheduledAnnouncements_Files_Table_Manager: INotificationCenter_ScheduledAnnouncements_Files_Table_Service
    {
        private readonly INotificationCenter_ScheduledAnnouncements_Files_Table_Dal _scheduledAnnouncements_Files_Table_Dal;

        public NotificationCenter_ScheduledAnnouncements_Files_Table_Manager(INotificationCenter_ScheduledAnnouncements_Files_Table_Dal scheduledAnnouncementsFilesTableDal)
        {
            _scheduledAnnouncements_Files_Table_Dal = scheduledAnnouncementsFilesTableDal;
        }

        public Task<List<NotificationCenter_ScheduledAnnouncements_Files_Table>> GetAllAsync()
        {
            return _scheduledAnnouncements_Files_Table_Dal.GetListAsync();
        }

        public Task<NotificationCenter_ScheduledAnnouncements_Files_Table> GetByIdAsync(int announcementId)
        {
            return _scheduledAnnouncements_Files_Table_Dal.GetAsync(s=>s.ID==announcementId);
        }

        public Task<List<NotificationCenter_ScheduledAnnouncements_Files_Table>> GetFilesByAnnouncementIdAsync(int announcementId)
        {
            return _scheduledAnnouncements_Files_Table_Dal.GetListAsync(f => f.ScheduledAnnouncementId== announcementId);
        }

        public Task AddAsync(NotificationCenter_ScheduledAnnouncements_Files_Table announcement)
        {
            return _scheduledAnnouncements_Files_Table_Dal.AddAsync(announcement);  
        }

        public Task UpdateAsync(NotificationCenter_ScheduledAnnouncements_Files_Table announcement)
        {
            return _scheduledAnnouncements_Files_Table_Dal.UpdateAsync(announcement);   
        }

        public Task DeleteAsync(NotificationCenter_ScheduledAnnouncements_Files_Table announcement)
        {
            return _scheduledAnnouncements_Files_Table_Dal.DeleteAsync(announcement);
        }
    }
}
