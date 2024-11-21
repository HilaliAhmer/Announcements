using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class ScheduledAnnouncementViewModel
    {
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


        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();

        public List<SelectListItem> ScheduleTypes { get; set; } = new List<SelectListItem>();
    }
}