using MCC.Korsini.Announcements.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_Announcement_Type_Table_Service
    {
        Task<List<NotificationCenter_Announcement_Type_Table>> GetAllAsync();
    }
}
