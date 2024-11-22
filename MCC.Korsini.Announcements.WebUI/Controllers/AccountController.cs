using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using MCC.Korsini.Announcements.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MCC.Korsini.Announcements.WebUI.ViewModels;

public class AccountController : Controller
{
    private readonly LdapAuthenticationService _ldapAuthenticationService;
    private readonly INotificationCenter_Users_Table_Service _userService;

    public AccountController(
        LdapAuthenticationService ldapAuthenticationService,
        INotificationCenter_Users_Table_Service userService)
    {
        _ldapAuthenticationService = ldapAuthenticationService;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        // Eğer kullanıcı zaten giriş yapmışsa, ana sayfaya yönlendir
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Announcement");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        //LDAP ile kimlik doğrulama
        if (!_ldapAuthenticationService.Authenticate(model.UserName, model.Password))
        {
            ModelState.AddModelError(string.Empty, "E-posta adresi veya şifre yanlış.");
            return View();
        }

        // Kullanıcının yetkili grupta olup olmadığını kontrol et
        if (!_ldapAuthenticationService.IsUserAuthorized(model.UserName))
        {
            ModelState.AddModelError(string.Empty, "Bu sisteme erişim yetkiniz yok.");
            return View();
        }

        // Kullanıcıyı veritabanında kontrol et
        var existingUser = await _userService.GetUserByUsernameAsync(model.UserName);
        if (existingUser == null)
        {
            // Kullanıcıyı oluştur ve kaydet
            var newUser = new NotificationCenter_Users_Table
            {
                UserName = model.UserName,
                Email = model.UserName,
                Role = "User", // Varsayılan rol
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            await _userService.AddUserAsync(newUser);
            existingUser = newUser;
        }

        // Kullanıcı kimlik bilgilerini (claims) oluştur
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.Role, existingUser.Role) // Rol bilgisini ekle
            };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Kimlik doğrulama oturumunu başlat
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        var existing1User = await _userService.GetUserByUsernameAsync(model.UserName);
        if (existing1User != null)
        {
            Console.WriteLine($"User Role: {existing1User.Role}"); // Rol bilgisi burada doğru mu kontrol edin
        }

        // Başarılı giriş sonrası ana sayfaya yönlendir
        return RedirectToAction("Index", "Announcement");

    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        // Kullanıcının oturumunu sonlandır
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Announcement");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

}
