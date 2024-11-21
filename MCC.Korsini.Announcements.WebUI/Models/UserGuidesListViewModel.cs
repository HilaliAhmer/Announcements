namespace MCC.Korsini.Announcements.WebUI.Models
{
    public class UserGuidesListViewModel
    {
        public int ID { get; set; } // Prosedür ID'si
        public string Title { get; set; } // Başlık
        public List<UserGuidesFilesListViewModel> Files { get; set; } = new List<UserGuidesFilesListViewModel>();
    }
    public class UserGuidesFilesListViewModel
    {
        public int ID { get; set; }              // Birincil anahtar
        public string FileName { get; set; }     // Dosya adı
        public string FilePath { get; set; }     // Dosya yolu
        public DateTime UploadedDate { get; set; } // Yüklenme tarihi
    }
}
