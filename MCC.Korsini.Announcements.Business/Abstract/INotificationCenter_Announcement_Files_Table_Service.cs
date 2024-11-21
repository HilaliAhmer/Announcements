using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_Announcement_Files_Table_Service
    {
        Task<List<NotificationCenter_Announcement_Files_Table>> GetAllAsync();
        Task<NotificationCenter_Announcement_Files_Table> GetByIdAsync(int announcementId);
        Task<List<NotificationCenter_Announcement_Files_Table>> GetFilesByAnnouncementIdAsync(int announcementId);
        Task AddAsync(NotificationCenter_Announcement_Files_Table announcement);
        Task UpdateAsync(NotificationCenter_Announcement_Files_Table announcement);
        Task DeleteAsync(NotificationCenter_Announcement_Files_Table announcement);
    }
}
