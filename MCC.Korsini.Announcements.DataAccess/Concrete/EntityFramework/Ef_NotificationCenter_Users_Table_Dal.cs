using MCC.Korsini.Announcements.Entities.Concrete;
using MCC.Korsini.Announcements.Core.DataAccess.EntityFramework;
using MCC.Korsini.Announcements.DataAccess.Abstract;

namespace MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework
{
    public class Ef_NotificationCenter_Users_Table_Dal : EfEntityRepositoryBase<NotificationCenter_Users_Table, NotificationCenter_Context>, INotificationCenter_Users_Table_Dal
    {
    }
}