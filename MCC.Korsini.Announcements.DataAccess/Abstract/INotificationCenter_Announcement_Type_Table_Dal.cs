using MCC.Korsini.Announcements.Core.DataAccess;
using MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework;
using MCC.Korsini.Announcements.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.DataAccess.Abstract
{
    public interface INotificationCenter_Announcement_Type_Table_Dal:IEntityRepository<NotificationCenter_Announcement_Type_Table>
    {
    }
}
