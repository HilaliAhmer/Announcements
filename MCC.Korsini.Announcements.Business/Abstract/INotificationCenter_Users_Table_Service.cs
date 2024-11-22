using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_Users_Table_Service
    {
        Task<NotificationCenter_Users_Table> GetUserByUsernameAsync(string email);
        Task AddUserAsync(NotificationCenter_Users_Table user);
        Task UpdateUserAsync(NotificationCenter_Users_Table user);
    }
}
