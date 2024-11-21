using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCC.Korsini.Announcements.Core.Entities;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_Announcement_Type_Table:IEntity
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Announcement_Type { get; set; }
    }
}
