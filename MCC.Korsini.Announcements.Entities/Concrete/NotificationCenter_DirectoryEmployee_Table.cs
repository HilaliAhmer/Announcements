using MCC.Korsini.Announcements.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_DirectoryEmployee_Table:IEntity
    {
        public int Id { get; set; }

        // AD Kimlik Bilgileri
        public string SamAccountName { get; set; }     // örn: s.acikgoz
        public string UserPrincipalName { get; set; }  // örn: s.acikgoz@multicolor.local
        public string DistinguishedName { get; set; }  // AD’deki DN

        // Kullanıcı Bilgileri
        public string DisplayName { get; set; }        // Görünen isim
        public string Title { get; set; }              // Ünvan
        public string Department { get; set; }
        public string Email { get; set; }

        // Organizasyon ilişkisi
        public int? ManagerId { get; set; }
        public NotificationCenter_DirectoryEmployee_Table Manager { get; set; }
        public ICollection<NotificationCenter_DirectoryEmployee_Table> DirectReports { get; set; }

        // Genel
        public bool IsActive { get; set; } = true;
        public DateTime LastSyncedAt { get; set; } = DateTime.UtcNow;

        public NotificationCenter_DirectoryEmployee_Table()
        {
            DirectReports = new List<NotificationCenter_DirectoryEmployee_Table>();
        }
    }
}
