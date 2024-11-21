using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_Announcements_Table_Service
    {
        Task<List<NotificationCenter_Announcements_Table>> GetAllAsync();
        Task<List<NotificationCenter_Announcements_Table>> GetAllDescAsync();
        Task<NotificationCenter_Announcements_Table> GetByIdAsync(int announcementId);
        Task AddAsync(NotificationCenter_Announcements_Table announcement);
        Task UpdateAsync(NotificationCenter_Announcements_Table announcement);
        Task DeleteAsync(NotificationCenter_Announcements_Table announcement);
    }
}
