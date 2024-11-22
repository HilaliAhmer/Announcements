using MCC.Korsini.Announcements.Core.Entities;

namespace MCC.Korsini.Announcements.Entities.Concrete
{
    public class NotificationCenter_Users_Table : IEntity
    {
        public int ID { get; set; }     //Birincil anahtar, kullanıcı ID'si
        public string UserName { get; set; }       //Kullanıcı adı
        public string Role { get; set; }        //Kullanıcı rolü (IT Admin/User)
        public string Email { get; set; }       //Kullanıcı e-posta adresi
        public Boolean IsActive { get; set; }       //Kullanıcı aktiflik durumu
        public DateTime CreatedDate { get; set; }
    }
}
