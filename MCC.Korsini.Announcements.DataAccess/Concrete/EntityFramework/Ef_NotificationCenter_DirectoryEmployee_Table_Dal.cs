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
    public class Ef_NotificationCenter_DirectoryEmployee_Table_Dal : EfEntityRepositoryBase<NotificationCenter_DirectoryEmployee_Table, NotificationCenter_Context>, INotificationCenter_DirectoryEmployee_Table_Dal
    {
    }
}
