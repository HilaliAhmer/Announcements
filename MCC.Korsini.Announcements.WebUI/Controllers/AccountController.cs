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

        if (!_ldapAuthenticationService.Authenticate(model.UserName, model.Password))
        {
            ModelState.AddModelError(string.Empty, "E-posta adresi veya şifre yanlış.");
            return View();
        }

        if (!_ldapAuthenticationService.IsUserAuthorized(model.UserName))
        {
            ModelState.AddModelError(string.Empty, "Bu sisteme erişim yetkiniz yok.");
            return View();
        }

        // LDAP'dan kullanıcı bilgilerini al
        var userAttributes = _ldapAuthenticationService.GetUserAttributes(model.UserName);
        if (userAttributes == null)
        {
            ModelState.AddModelError(string.Empty, "LDAP'dan kullanıcı bilgileri alınamadı.");
            return View();
        }

        // Kullanıcıyı veritabanında kontrol et
        var existingUser = await _userService.GetUserByUsernameAsync(model.UserName);
        bool isUpdated = false;

        if (existingUser == null)
        {
            // Yeni kullanıcı oluştur ve LDAP'dan alınan bilgileri kullan
            var newUser = new NotificationCenter_Users_Table
            {
                UserName = userAttributes["mail"], // LDAP'dan alınan mail bilgisi
                Email = userAttributes["mail"],
                Role = "User", // Varsayılan rol
                FullName = userAttributes["cn"], // LDAP'dan alınan ad soyad bilgisi
                Department = userAttributes["department"],
                Title = userAttributes["title"],
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            await _userService.AddUserAsync(newUser);
            existingUser = newUser;
        }
        else
        {
            // Mevcut kullanıcıyı LDAP bilgileriyle karşılaştır ve güncelle
            var ldapAttributes = new Dictionary<string, string>
            {
                { "FullName", userAttributes["cn"] },
                { "Department", userAttributes["department"] },
                { "Title", userAttributes["title"] }
            };

            foreach (var key in ldapAttributes.Keys)
            {
                var dbValue = existingUser.GetType().GetProperty(key)?.GetValue(existingUser)?.ToString();
                if (dbValue != ldapAttributes[key])
                {
                    existingUser.GetType().GetProperty(key)?.SetValue(existingUser, ldapAttributes[key]);
                    isUpdated = true;
                }
            }

            if (isUpdated)
            {
                existingUser.UpdateDate= DateTime.Now;
                await _userService.UpdateUserAsync(existingUser);
            }
        }

        // Kullanıcı kimlik bilgilerini (claims) oluştur
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userAttributes["mail"]),
            new Claim(ClaimTypes.Role, existingUser.Role),
            new Claim("FullName", userAttributes["cn"] ?? string.Empty),
            new Claim("Department", userAttributes["department"] ?? string.Empty),
            new Claim("Title", userAttributes["title"] ?? string.Empty),
            new Claim("UserId", existingUser.ID.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

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
