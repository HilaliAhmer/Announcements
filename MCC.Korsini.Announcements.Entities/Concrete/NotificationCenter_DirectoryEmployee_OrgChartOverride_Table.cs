using MCC.Korsini.Announcements.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_DirectoryEmployee_OrgChartOverride_Table:IEntity
    {
        public int Id { get; set; }
        public string EmployeeSam { get; set; } = null!;
        public string DisplayManagerSam { get; set; } = null!;
        public byte Scope { get; set; } = 1;
        public bool IsLocalTopManager { get; set; } = false;
        public int? SortOrder { get; set; }
    }
}
