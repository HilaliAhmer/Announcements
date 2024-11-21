using MCC.Korsini.Announcements.Entities.Concrete;
using MCC.Korsini.Announcements.Core.DataAccess.EntityFramework;
using MCC.Korsini.Announcements.DataAccess.Abstract;

namespace MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework
{
    public class Ef_NotificationCenter_Announcement_Files_Table_Dal: EfEntityRepositoryBase<NotificationCenter_Announcement_Files_Table, NotificationCenter_Context>, INotificationCenter_Announcement_Files_Table_Dal
    {
    }
}
