using MCC.Korsini.Announcements.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_Announcement_Type_Table_Manager: INotificationCenter_Announcement_Type_Table_Service
    {
        private readonly INotificationCenter_Announcement_Type_Table_Dal _announcementType;

        public NotificationCenter_Announcement_Type_Table_Manager(INotificationCenter_Announcement_Type_Table_Dal announcementType)
        {
            _announcementType = announcementType;
        }

        public Task<List<NotificationCenter_Announcement_Type_Table>> GetAllAsync()
        {
            return _announcementType.GetListAsync();
        }
    }
}
