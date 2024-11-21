using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_Procedures_Files_Table_Service
    {
        Task<List<NotificationCenter_Procedures_Files_Table>> GetAllAsync();
        Task<NotificationCenter_Procedures_Files_Table> GetByIdAsync(int procedureId);
        Task<List<NotificationCenter_Procedures_Files_Table>> GetFilesByAnnouncementIdAsync(int procedureId);
        Task AddAsync(NotificationCenter_Procedures_Files_Table procedure);
        Task UpdateAsync(NotificationCenter_Procedures_Files_Table procedure);
        Task DeleteAsync(NotificationCenter_Procedures_Files_Table procedure);
    }
}
