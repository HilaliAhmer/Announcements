using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_UserGuides_Files_Table_Service
    {
        Task<List<NotificationCenter_UserGuides_Files_Table>> GetAllAsync();
        Task<NotificationCenter_UserGuides_Files_Table> GetByIdAsync(int userGuideId);
        Task<List<NotificationCenter_UserGuides_Files_Table>> GetFilesByAnnouncementIdAsync(int userGuideId);
        Task AddAsync(NotificationCenter_UserGuides_Files_Table userGuide);
        Task UpdateAsync(NotificationCenter_UserGuides_Files_Table userGuide);
        Task DeleteAsync(NotificationCenter_UserGuides_Files_Table userGuide);
    }
}
