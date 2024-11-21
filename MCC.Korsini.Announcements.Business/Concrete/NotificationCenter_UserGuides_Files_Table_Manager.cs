using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_UserGuides_Files_Table_Manager :INotificationCenter_UserGuides_Files_Table_Service
    {
        private readonly INotificationCenter_UserGuides_Files_Table_Dal _userGuidesFiles;

        public NotificationCenter_UserGuides_Files_Table_Manager(INotificationCenter_UserGuides_Files_Table_Dal userGuidesFiles)
        {
            _userGuidesFiles = userGuidesFiles;
        }

        public async Task<List<NotificationCenter_UserGuides_Files_Table>> GetAllAsync()
        {
            return await _userGuidesFiles.GetListAsync();
        }

        public async Task<NotificationCenter_UserGuides_Files_Table> GetByIdAsync(int userguideId)
        {
            return await _userGuidesFiles.GetAsync(uf => uf.ID == userguideId);
        }

        public async Task<List<NotificationCenter_UserGuides_Files_Table>> GetFilesByAnnouncementIdAsync(int userguideId)
        {
            return await _userGuidesFiles.GetListAsync(uf => uf.UserGuideID == userguideId);
        }

        public Task AddAsync(NotificationCenter_UserGuides_Files_Table userguide)
        {
            return _userGuidesFiles.AddAsync(userguide);
        }

        public Task UpdateAsync(NotificationCenter_UserGuides_Files_Table userguide)
        {
            return _userGuidesFiles.UpdateAsync(userguide);
        }

        public Task DeleteAsync(NotificationCenter_UserGuides_Files_Table userguide)
        {
            return _userGuidesFiles.DeleteAsync(userguide);
        }
    }
}
