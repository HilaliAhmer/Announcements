
using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using System.Linq.Expressions;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_Procedures_Table_Manager : INotificationCenter_Procedures_Table_Service
    {
        private readonly INotificationCenter_Procedures_Table_Dal _procedures;

        public NotificationCenter_Procedures_Table_Manager(INotificationCenter_Procedures_Table_Dal procedures)
        {
            _procedures = procedures;
        }

        public async Task<List<NotificationCenter_Procedures_Table>> GetAllAsync(params Expression<Func<NotificationCenter_Procedures_Table, object>>[] includes)
        {
            return await _procedures.GetListAsync(null, includes);
        }

        public async Task<NotificationCenter_Procedures_Table> GetByIdAsync(
            int procedureId,
            params Expression<Func<NotificationCenter_Procedures_Table, object>>[] includes)
        {
            return await _procedures.GetAsync(p => p.ID == procedureId, includes);
        }

        public async Task AddAsync(NotificationCenter_Procedures_Table procedures)
        {
            await _procedures.AddAsync(procedures);
        }

        public async Task UpdateAsync(NotificationCenter_Procedures_Table procedure)
        {
            await _procedures.UpdateAsync(procedure);
        }

        public async Task DeleteAsync(NotificationCenter_Procedures_Table procedures)
        {
            await _procedures.DeleteAsync(procedures);
        }

    }
}
