using MCC.Korsini.Announcements.Core.Entities;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_Procedures_Table : IEntity
    {
        public int ID { get; set; }         // Birincil anahtar, prosedür ID'si
        public string Title { get; set; }     //Başlık

        public virtual ICollection<NotificationCenter_Procedures_Files_Table> Files { get; set; } = new List<NotificationCenter_Procedures_Files_Table>();
    }
}
