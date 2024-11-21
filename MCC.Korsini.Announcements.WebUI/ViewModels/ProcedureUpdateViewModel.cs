﻿using System.ComponentModel.DataAnnotations;
namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class ProcedureUpdateViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        public string Title { get; set; }
        public List<ExistingFileViewModel> ExistingFiles { get; set; } = new(); // Mevcut dosyalar
        public List<IFormFile> NewAttachments { get; set; } = new(); // Yeni yüklenecek dosyalar
        public List<string> FilesToDelete { get; set; } = new(); // Silinmesi gereken dosyalar
    }

    public class ExistingFileViewModel
    {
        public int ID { get; set; }
        public string FilePath { get; set; }
        public string DisplayName { get; set; }
    }


}