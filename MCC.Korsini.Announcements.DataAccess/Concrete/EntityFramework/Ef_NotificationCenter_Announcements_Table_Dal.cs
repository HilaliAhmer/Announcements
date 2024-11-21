using MCC.Korsini.Announcements.Entities.Concrete;
using MCC.Korsini.Announcements.Core.DataAccess.EntityFramework;
using MCC.Korsini.Announcements.DataAccess.Abstract;

namespace MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework
{
    public class Ef_NotificationCenter_Announcements_Table_Dal : EfEntityRepositoryBase<NotificationCenter_Announcements_Table, NotificationCenter_Context>, INotificationCenter_Announcements_Table_Dal
    {
    }
}