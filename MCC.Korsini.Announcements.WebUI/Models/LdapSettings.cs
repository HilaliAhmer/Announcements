namespace MCC.Korsini.Announcements.WebUI.Models
{
    public class LdapSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        //public string Domain { get; set; }
        public string DistinguishedGroupName { get; set; }
        public string BaseDN { get; set; }
    }

}
