using System.Linq.Expressions;
using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_UserGuides_Table_Manager: INotificationCenter_UserGuides_Table_Service
    {
        private readonly INotificationCenter_UserGuides_Table_Dal _userGuides;

        public NotificationCenter_UserGuides_Table_Manager(INotificationCenter_UserGuides_Table_Dal userGuides)
        {
            _userGuides = userGuides;
        }

        public async Task<List<NotificationCenter_UserGuides_Table>> GetAllAsync(params Expression<Func<NotificationCenter_UserGuides_Table, object>>[] includes)
        {
            return await _userGuides.GetListAsync(null, includes);
        }

        public async Task<NotificationCenter_UserGuides_Table> GetByIdAsync(int userGuideId, params Expression<Func<NotificationCenter_UserGuides_Table, object>>[] includes)
        {
            return await _userGuides.GetAsync(p => p.ID == userGuideId, includes);
        }

        public async Task AddAsync(NotificationCenter_UserGuides_Table userGuides)
        {
            await _userGuides.AddAsync(userGuides);
        }

        public async Task UpdateAsync(NotificationCenter_UserGuides_Table userGuides)
        {
            await _userGuides.UpdateAsync(userGuides);
        }

        public  async Task DeleteAsync(NotificationCenter_UserGuides_Table userGuides)
        {
            await _userGuides.DeleteAsync(userGuides);
        }
    }
}
