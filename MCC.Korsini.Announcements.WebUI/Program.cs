using Hangfire;
using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.Business.Abstract.AnnouncementMailAbstract;
using MCC.Korsini.Announcements.Business.Abstract.GenerateAnnouncementIdAbstract;
using MCC.Korsini.Announcements.Business.Abstract.HangfireAbstract;
using MCC.Korsini.Announcements.Business.Abstract.HtmlSanitizerAbstract;
using MCC.Korsini.Announcements.Business.Abstract.SummarizeTexAbstract;
using MCC.Korsini.Announcements.Business.Concrete;
using MCC.Korsini.Announcements.Business.Concrete.AnnouncementMailConcrete;
using MCC.Korsini.Announcements.Business.Concrete.GenerateAnnouncementIdConcrete;
using MCC.Korsini.Announcements.Business.Concrete.HangfireConcrete;
using MCC.Korsini.Announcements.Business.Concrete.HtmlSanitizerConcrete;
using MCC.Korsini.Announcements.Business.Concrete.SummarizeTextConcrete;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework;
using MCC.Korsini.Announcements.WebUI.Helpers;
using MCC.Korsini.Announcements.WebUI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<NotificationCenter_Context>();
builder.Services.AddScoped<IHangfireJobService, HangfireJobManager>();
builder.Services.AddSingleton<IHtmlSanitizerService, HtmlSanitizerManager>();
builder.Services.AddScoped<IAnnouncementMailService, AnnouncementMailManager>();
builder.Services.AddScoped<IGenerateAnnouncementIdService, GenerateAnnouncementIdManager>();
builder.Services.AddScoped<IOpenAIClient_Service, OpenAIClient_Manager>();
builder.Services.AddHttpClient<OpenAIClient_Manager>();
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<INotificationCenter_Announcements_Table_Service, NotificationCenter_Announcements_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_Announcements_Table_Dal, Ef_NotificationCenter_Announcements_Table_Dal>();

builder.Services.AddScoped<INotificationCenter_Announcement_Files_Table_Service, NotificationCenter_Announcement_Files_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_Announcement_Files_Table_Dal, Ef_NotificationCenter_Announcement_Files_Table_Dal>();

builder.Services.AddScoped<INotificationCenter_Announcement_Type_Table_Service, NotificationCenter_Announcement_Type_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_Announcement_Type_Table_Dal, Ef_NotificationCenter_Announcement_Type_Table_Dal>();

builder.Services.AddScoped<INotificationCenter_ScheduledAnnouncements_Table_Service, NotificationCenter_ScheduledAnnouncements_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_ScheduledAnnouncements_Table_Dal, Ef_NotificationCenter_ScheduledAnnouncements_Table_Dal>();

builder.Services.AddScoped<INotificationCenter_ScheduledAnnouncements_Files_Table_Service, NotificationCenter_ScheduledAnnouncements_Files_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_ScheduledAnnouncements_Files_Table_Dal, Ef_NotificationCenter_ScheduledAnnouncements_Files_Table_Dal>();

builder.Services.AddScoped<INotificationCenter_Procedures_Table_Service, NotificationCenter_Procedures_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_Procedures_Table_Dal, Ef_NotificationCenter_Procedures_Table_Dal>();

builder.Services.AddScoped<INotificationCenter_UserGuides_Table_Service, NotificationCenter_UserGuides_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_UserGuides_Table_Dal, Ef_NotificationCenter_UserGuides_Table_Dal>();

builder.Services.AddScoped<INotificationCenter_Procedures_Files_Table_Service, NotificationCenter_Procedures_Files_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_Procedures_Files_Table_Dal, Ef_NotificationCenter_Procedures_Files_Table_Dal>();

builder.Services.AddScoped<INotificationCenter_UserGuides_Files_Table_Service, NotificationCenter_UserGuides_Files_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_UserGuides_Files_Table_Dal, Ef_NotificationCenter_UserGuides_Files_Table_Dal>();

builder.Services.AddScoped<INotificationCenter_Users_Table_Service, NotificationCenter_Users_Table_Manager>();
builder.Services.AddScoped<INotificationCenter_Users_Table_Dal, Ef_NotificationCenter_Users_Table_Dal>();

builder.Services.Configure<LdapSettings>(builder.Configuration.GetSection("LdapSettings"));
builder.Services.AddScoped<LdapAuthenticationService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Giriþ yapmamýþ kullanýcýlarý yönlendirme
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz eriþim
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(@"Server=10.138.10.66;Database=NotificationCenter;User ID=sa;Password=Trapper35!;TrustServerCertificate=True;Integrated Security=False");
});
builder.Services.AddHangfireServer();
builder.Services.AddScoped<IHangfireJobService, HangfireJobManager>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
builder.Services.AddScoped<ToastHelper>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Test/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication(); // Kimlik doðrulamayý aktif et
app.UseAuthorization();  // Yetkilendirme kontrollerini aktif et

app.UseHangfireDashboard("/hangfire");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Announcement}/{action=Index}/{id?}");

// Consolidated route definitions for specific controllers
var customRoutes = new[]
{
    new { Name = "PlanScheduledAnnouncement", Pattern = "PlanScheduledAnnouncement/{action=Index}/{id?}", Controller = "PlanScheduledAnnouncement" },
    new { Name = "Procedures", Pattern = "Procedures/{action=Index}/{id?}", Controller = "Procedures" },
    new { Name = "UserGuides", Pattern = "UserGuides/{action=Index}/{id?}", Controller = "UserGuides" }
};

foreach (var route in customRoutes)
{
    app.MapControllerRoute(
        name: route.Name,
        pattern: route.Pattern,
        defaults: new { controller = route.Controller });
}
app.Run();
