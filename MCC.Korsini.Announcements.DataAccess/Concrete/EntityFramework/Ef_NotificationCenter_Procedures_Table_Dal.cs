using MCC.Korsini.Announcements.Entities.Concrete;
using MCC.Korsini.Announcements.Core.DataAccess.EntityFramework;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework
{
    public class Ef_NotificationCenter_Procedures_Table_Dal : EfEntityRepositoryBase<NotificationCenter_Procedures_Table, NotificationCenter_Context>, INotificationCenter_Procedures_Table_Dal
    {
        
    }
}