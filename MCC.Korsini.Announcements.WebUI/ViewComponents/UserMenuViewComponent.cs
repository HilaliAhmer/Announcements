using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MCC.Korsini.Announcements.WebUI.ViewModels;

namespace MCC.Korsini.Announcements.WebUI.ViewComponents
{
    public class UserMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var claimsPrincipal = User as ClaimsPrincipal; // ClaimsPrincipal olarak dönüştür

            // Kullanıcı kimlik bilgilerini al
            var isAuthenticated = claimsPrincipal?.Identity?.IsAuthenticated ?? false;
            var fullName = isAuthenticated ? claimsPrincipal.FindFirst("FullName")?.Value : null;
            var email = isAuthenticated ? claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value : null;
            var title = isAuthenticated ? claimsPrincipal.FindFirst("Title")?.Value : null;
            var role = isAuthenticated ? claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value : null; // Role bilgisi

            // Model oluştur
            var model = new UserMenuViewModel
            {
                IsAuthenticated = isAuthenticated,
                FullName = fullName,
                Email = email,
                Title = title,
                Role= role
            };

            return View(model);
        }
    }
}