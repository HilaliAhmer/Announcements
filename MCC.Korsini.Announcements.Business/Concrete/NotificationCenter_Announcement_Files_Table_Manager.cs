using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    
    public class NotificationCenter_Announcement_Files_Table_Manager: INotificationCenter_Announcement_Files_Table_Service
    {
        private readonly INotificationCenter_Announcement_Files_Table_Dal _announcement_Files_Table_Dal;

        public NotificationCenter_Announcement_Files_Table_Manager(INotificationCenter_Announcement_Files_Table_Dal announcementFilesTableDal)
        {
            _announcement_Files_Table_Dal = announcementFilesTableDal;
        }

        public Task<List<NotificationCenter_Announcement_Files_Table>> GetAllAsync()
        {
            return _announcement_Files_Table_Dal.GetListAsync();
        }

        public Task<NotificationCenter_Announcement_Files_Table> GetByIdAsync(int announcementId)
        {
            return _announcement_Files_Table_Dal.GetAsync(f => f.ID == announcementId);
        }

        public Task<List<NotificationCenter_Announcement_Files_Table>> GetFilesByAnnouncementIdAsync(int announcementId)
        {
            return _announcement_Files_Table_Dal.GetListAsync(f => f.AnnouncementId == announcementId);
        }

        public Task AddAsync(NotificationCenter_Announcement_Files_Table announcement)
        {
            return _announcement_Files_Table_Dal.AddAsync(announcement);
        }

        public Task UpdateAsync(NotificationCenter_Announcement_Files_Table announcement)
        {
            return _announcement_Files_Table_Dal.UpdateAsync(announcement);
        }

        public Task DeleteAsync(NotificationCenter_Announcement_Files_Table announcement)
        {
            return _announcement_Files_Table_Dal.DeleteAsync(announcement);
        }
    }
}
