using MCC.Korsini.Announcements.Entities.Concrete;
using System.Linq.Expressions;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_UserGuides_Table_Service
    {
        Task<List<NotificationCenter_UserGuides_Table>> GetAllAsync(params Expression<Func<NotificationCenter_UserGuides_Table, object>>[] includes);
        Task<NotificationCenter_UserGuides_Table> GetByIdAsync(
            int userGuideId,
            params Expression<Func<NotificationCenter_UserGuides_Table, object>>[] includes);

        Task AddAsync(NotificationCenter_UserGuides_Table userGuides);
        Task UpdateAsync(NotificationCenter_UserGuides_Table userGuides);
        Task DeleteAsync(NotificationCenter_UserGuides_Table userGuides);
    }
}
