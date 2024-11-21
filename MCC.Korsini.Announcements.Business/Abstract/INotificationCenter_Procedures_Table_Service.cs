using MCC.Korsini.Announcements.Entities.Concrete;
using System.Linq.Expressions;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_Procedures_Table_Service
    {
        Task<List<NotificationCenter_Procedures_Table>> GetAllAsync(params Expression<Func<NotificationCenter_Procedures_Table, object>>[] includes);
        Task<NotificationCenter_Procedures_Table> GetByIdAsync(
            int procedureId,
            params Expression<Func<NotificationCenter_Procedures_Table, object>>[] includes);

        Task AddAsync(NotificationCenter_Procedures_Table procedure);
        Task UpdateAsync(NotificationCenter_Procedures_Table procedure);
        Task DeleteAsync(NotificationCenter_Procedures_Table procedure);

    }
}