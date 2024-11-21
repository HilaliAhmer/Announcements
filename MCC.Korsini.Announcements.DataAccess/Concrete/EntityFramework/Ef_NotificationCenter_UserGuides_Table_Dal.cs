using MCC.Korsini.Announcements.Entities.Concrete;
using MCC.Korsini.Announcements.Core.DataAccess.EntityFramework;
using MCC.Korsini.Announcements.DataAccess.Abstract;

namespace MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework
{
    public class Ef_NotificationCenter_UserGuides_Table_Dal : EfEntityRepositoryBase<NotificationCenter_UserGuides_Table, NotificationCenter_Context>, INotificationCenter_UserGuides_Table_Dal
    {
    }
}