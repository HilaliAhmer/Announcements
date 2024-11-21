using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_Announcements_Table_Manager : INotificationCenter_Announcements_Table_Service
    {
        private readonly INotificationCenter_Announcements_Table_Dal _announcementsTableDal;

        public NotificationCenter_Announcements_Table_Manager(INotificationCenter_Announcements_Table_Dal announcementsTableDal)
        {
            _announcementsTableDal = announcementsTableDal;
        }

        public Task<List<NotificationCenter_Announcements_Table>> GetAllAsync()
        {
            return _announcementsTableDal.GetListAsync(a=>a.IsVisibleToAll==true);
        }

        public async Task<List<NotificationCenter_Announcements_Table>> GetAllDescAsync()
        {
            return (await _announcementsTableDal.GetListAsync())
                .OrderByDescending(a => a.ID)
                .ToList();
        }

        public Task<NotificationCenter_Announcements_Table> GetByIdAsync(int announcementId)
        {
            return _announcementsTableDal.GetAsync(a => a.ID == announcementId);
        }

        public Task AddAsync(NotificationCenter_Announcements_Table announcement)
        {
            return _announcementsTableDal.AddAsync(announcement);
        }

        public Task UpdateAsync(NotificationCenter_Announcements_Table announcement)
        {
            return _announcementsTableDal.UpdateAsync(announcement);
        }

        public Task DeleteAsync(NotificationCenter_Announcements_Table announcement)
        {
            announcement.IsVisibleToAll = false;

            // Veritabanında güncelleme işlemi yapıyoruz
           return _announcementsTableDal.UpdateAsync(announcement);
        }
    }
}
