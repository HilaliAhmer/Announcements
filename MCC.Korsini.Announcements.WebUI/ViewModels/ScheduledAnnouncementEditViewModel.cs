using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class ScheduledAnnouncementEditViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Türkçe Başlık Gereklidir.")]
        public string Title_TR { get; set; }

        [Required(ErrorMessage = "Türkçe İçerik Gereklidir.")]
        public string Content_TR { get; set; }

        [Required(ErrorMessage = "İngilizce Başlık Gereklidir.")]
        public string Title_EN { get; set; }

        [Required(ErrorMessage = "İngilizce İçerik Gereklidir.")]
        public string Content_EN { get; set; }

        [Required(ErrorMessage = "Tür Gereklidir.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Planlanmış Tarih Gereklidir.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Lütfen bir planlama şekli seçiniz.")]
        public string SelectedScheduleType { get; set; }
        public List<(string FullPath, string DisplayName)> ExistingFiles { get; set; } = new List<(string, string)>();

        // Mevcut ekli dosyalar
        // public List<UploadedFileViewModel> ExistingFiles { get; set; } = new List<UploadedFileViewModel>();

        // Yeni yüklenecek dosyalar
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();

        public List<SelectListItem> ScheduleTypes { get; set; } = new List<SelectListItem>();
    }
}


