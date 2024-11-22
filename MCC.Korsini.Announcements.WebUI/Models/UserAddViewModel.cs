using System.ComponentModel.DataAnnotations;

namespace MCC.Korsini.Announcements.WebUI.ViewModels;

public class UserAddViewModel
{
    [Required(ErrorMessage = "Kullanıcı Adı Gereklidir.")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Şifre Gereklidir.")]
    public string Password { get; set; }
}