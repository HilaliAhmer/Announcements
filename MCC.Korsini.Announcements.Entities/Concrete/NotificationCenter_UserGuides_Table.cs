using MCC.Korsini.Announcements.Core.Entities;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_UserGuides_Table : IEntity
    {
        public int ID { get; set; }         // Birincil anahtar, prosedür ID'si
        public string Title { get; set; }     //Başlık
        public virtual ICollection<NotificationCenter_UserGuides_Files_Table> Files { get; set; } = new List<NotificationCenter_UserGuides_Files_Table>();
    }
}
