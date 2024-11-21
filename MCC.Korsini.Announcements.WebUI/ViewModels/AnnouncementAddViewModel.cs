using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MCC.Korsini.Announcements.WebUI.ViewModels
{
    public class AnnouncementAddViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Başlık gereklidir.")]
        public string Title_TR { get; set; }
        [Required(ErrorMessage = "İçerik gereklidir.")]
        public string Content_TR { get; set; }
        [Required(ErrorMessage = "Başlık gereklidir.")]
        public string Title_EN { get; set; }
        [Required(ErrorMessage = "İçerik gereklidir.")]
        public string Content_EN { get; set; }
        [Required(ErrorMessage = "Tür gereklidir.")]
        public string Type { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
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
