using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class AnnouncementEditViewModel
    {
        public int ID { get; set; } // Düzenlenecek duyurunun benzersiz kimliği

        [Required(ErrorMessage = "Başlık (Türkçe) gereklidir.")]
        public string Title_TR { get; set; }

        [Required(ErrorMessage = "İçerik (Türkçe) gereklidir.")]
        public string Content_TR { get; set; }

        [Required(ErrorMessage = "Başlık (English) gereklidir.")]
        public string Title_EN { get; set; }

        [Required(ErrorMessage = "İçerik (English) gereklidir.")]
        public string Content_EN { get; set; }

        [Required(ErrorMessage = "Tür gereklidir.")]
        public string Type { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        // Mevcut ekli dosyalar (GUID ile tam yol ve GUID’siz ad)
        public List<(string FullPath, string DisplayName)> ExistingFiles { get; set; } = new List<(string, string)>();

        // Yeni yüklenen dosyalar
        public List<IFormFile> NewAttachments { get; set; } = new List<IFormFile>();

        public List<SelectListItem> Types { get; set; } = new List<SelectListItem>();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Type) || Type == "Bir tür seçin")
            {
                yield return new ValidationResult("Geçerli bir duyuru türü seçin.", new[] { nameof(Type) });
            }
        }
    }
}