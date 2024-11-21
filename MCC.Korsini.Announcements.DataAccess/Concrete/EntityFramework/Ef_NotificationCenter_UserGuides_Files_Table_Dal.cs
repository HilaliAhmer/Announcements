using MCC.Korsini.Announcements.Core.DataAccess.EntityFramework;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework
{
    public class Ef_NotificationCenter_UserGuides_Files_Table_Dal : EfEntityRepositoryBase<NotificationCenter_UserGuides_Files_Table, NotificationCenter_Context>, INotificationCenter_UserGuides_Files_Table_Dal
    {
    }
}
