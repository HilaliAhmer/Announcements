using Microsoft.AspNetCore.Mvc;
using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.Business.Abstract.GenerateAnnouncementIdAbstract;
using MCC.Korsini.Announcements.Business.Abstract.HangfireAbstract;
using MCC.Korsini.Announcements.WebUI.ViewModels;
using MCC.Korsini.Announcements.Entities.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hangfire;
using MCC.Korsini.Announcements.WebUI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MCC.Korsini.Announcements.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PlanScheduledAnnouncementController : Controller
    {
        private readonly INotificationCenter_ScheduledAnnouncements_Table_Service _scheduledAnnouncementsService;
        private readonly INotificationCenter_ScheduledAnnouncements_Files_Table_Service _scheduledFilesAnnouncementsService;
        private readonly IHangfireJobService _hangfireJobService;
        private readonly INotificationCenter_Announcements_Table_Service _announcementsTableService;
        private readonly IGenerateAnnouncementIdService _generateAnnouncementIdService;
        private readonly ToastHelper _toastHelper;

        public PlanScheduledAnnouncementController(INotificationCenter_ScheduledAnnouncements_Table_Service scheduledAnnouncementsService, INotificationCenter_ScheduledAnnouncements_Files_Table_Service scheduledFilesAnnouncements, IHangfireJobService hangfireJobService, INotificationCenter_Announcements_Table_Service announcementsTableService, IGenerateAnnouncementIdService generateAnnouncementIdService, ToastHelper toastHelper)
        {
            _scheduledAnnouncementsService = scheduledAnnouncementsService;
            _scheduledFilesAnnouncementsService = scheduledFilesAnnouncements;
            _hangfireJobService = hangfireJobService;
            _announcementsTableService = announcementsTableService;
            _generateAnnouncementIdService = generateAnnouncementIdService;
            _toastHelper = toastHelper;
        }
        //private async Task<string> GenerateAnnouncementId()
        //{
        //    var yearSuffix = DateTime.Now.Year.ToString().Substring(2);
        //    var lastAnnouncement = (await _announcementsTableService.GetAllAsync())
        //        .OrderByDescending(a => a.ID)
        //        .FirstOrDefault();

        //    int nextId = 1;
        //    if (lastAnnouncement != null && lastAnnouncement.AnnouncementId.Length >= 2)
        //    {
        //        var lastNumberPart = lastAnnouncement.AnnouncementId.Split('-').Last();
        //        if (int.TryParse(lastNumberPart, out int lastNumber))
        //        {
        //            nextId = lastNumber + 1;
        //        }
        //    }

        //    return $"DYR-IT-{yearSuffix}-{nextId:D2}";
        //}

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Veritabanından tüm planlanmış duyuruları çekiyoruz
            var scheduledAnnouncements = (await _scheduledAnnouncementsService.GetAllAsync())
                .Where(s => s.IsActive)
                .ToList();

            var model = scheduledAnnouncements.Select(a => new ScheduledAnnouncementListViewModel
            {
                ID = a.ID,
                Title_TR = a.Title_TR,
                Type = a.Type,
                CreateDate = a.CreateDate,
                ScheduleType = a.ScheduleType,
                ScheduledDate = a.ScheduledDate,
                ScheduleTypeShow = a.ScheduleTypeShow,
                NextRunTime = a.NextRunTime ?? DateTime.MinValue,
                IsActive = a.IsActive
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Planlama türleri Dropdown için hazırlanıyor
            var model = new ScheduledAnnouncementViewModel
            {
                ScheduleTypes = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Seçiniz", Value = "" },
                    new SelectListItem { Text = "Her Ayın İlk Pazartesi", Value = "MonthlyFirstMonday" },
                    new SelectListItem { Text = "Her Ayın 15. Günü", Value = "Monthly15th" },
                    new SelectListItem { Text = "Her Hafta Pazartesi", Value = "WeeklyMonday" },
                    new SelectListItem { Text = "Belirli Bir Tarih", Value = "Once" }
                }
            };
            return View(model);
        }

        public DateTime AdjustToNextMondayIfWeekend(DateTime date)
        {
            // Eğer Cumartesi ise 2 gün ekle, Pazar ise 1 gün ekle
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                return date.AddDays(2);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.AddDays(1);
            }

            // Hafta içiyse aynen döndür
            return date;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ScheduledAnnouncementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _toastHelper.CreateError();
                return View(model);
            }
            DateTime baseDate = model.ScheduledDate == default ? DateTime.Now : model.ScheduledDate;
            DateTime? nextRunTime = null;
            var currentDay = DateTime.Now;
            var scheduleTypeShowData = "";
            if (model.SelectedScheduleType == "MonthlyFirstMonday")
            {
                // Pazartesi kontrolü
                var isTodayMonday = currentDay.DayOfWeek == DayOfWeek.Monday;
                var targetTime = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, 10, 30, 0);

                if (isTodayMonday && currentDay < targetTime)
                {
                    // Bugün Pazartesi ve saat 10:30'dan önce
                    nextRunTime = targetTime;
                }
                else
                {
                    // Bir sonraki Pazartesi
                    var daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)currentDay.DayOfWeek + 7) % 7;
                    daysUntilNextMonday = daysUntilNextMonday == 0 ? 7 : daysUntilNextMonday;
                    var nextMonday = currentDay.AddDays(daysUntilNextMonday);
                    nextRunTime = new DateTime(nextMonday.Year, nextMonday.Month, nextMonday.Day, 10, 30, 0);
                }

                model.ScheduledDate = nextRunTime.Value;
                scheduleTypeShowData = "Her Ayın İlk Pazartesi";
            }
            else if (model.SelectedScheduleType == "Monthly15th")
            {
                var thisMonth15th = new DateTime(baseDate.Year, baseDate.Month, 15);
                if (baseDate <= thisMonth15th)
                {
                    nextRunTime = AdjustToNextMondayIfWeekend(thisMonth15th);
                }
                else
                {
                    var nextMonth15th = new DateTime(baseDate.Year, baseDate.Month, 15).AddMonths(1);
                    nextRunTime = AdjustToNextMondayIfWeekend(nextMonth15th);
                }

                // nextRunTime değerini 10:30 olarak sabitle
                if (nextRunTime.HasValue)
                {
                    nextRunTime = new DateTime(
                        nextRunTime.Value.Year,
                        nextRunTime.Value.Month,
                        nextRunTime.Value.Day,
                        10, 30, 0 // Saat 10:30 olarak sabitlenir
                    );
                }
                else
                {
                    nextRunTime = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 10, 30, 0);
                }

                // ScheduledDate'i nextRunTime değerine ayarla
                model.ScheduledDate = nextRunTime.Value;

                scheduleTypeShowData = "Her Ayın 15. Günü";
            }
            else if (model.SelectedScheduleType == "WeeklyMonday")
            {
                // Pazartesi kontrolü
                var isTodayMonday = currentDay.DayOfWeek == DayOfWeek.Monday;
                var targetTime = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, 10, 30, 0);

                if (isTodayMonday && currentDay < targetTime)
                {
                    // Bugün Pazartesi ve saat 10:30'dan önce
                    nextRunTime = targetTime;
                }
                else
                {
                    // Bir sonraki Pazartesi'yi hesapla
                    var daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)currentDay.DayOfWeek + 7) % 7;
                    daysUntilNextMonday = daysUntilNextMonday == 0 ? 7 : daysUntilNextMonday;
                    var nextMonday = currentDay.AddDays(daysUntilNextMonday);
                    nextRunTime = new DateTime(nextMonday.Year, nextMonday.Month, nextMonday.Day, 10, 30, 0);
                }

                model.ScheduledDate = nextRunTime.Value;
                scheduleTypeShowData = "Her Hafta Pazartesi";
            }

            else if (model.SelectedScheduleType == "Once")
            {
                nextRunTime = model.ScheduledDate; // Belirli bir tarih seçilmişse aynı tarihi kullan
                scheduleTypeShowData = "Belirli Bir Tarih";
            }


            //var formattedAnnouncementId = await _generateAnnouncementIdService.GenerateAnnouncementId();
            var scheduledAnnouncement = new NotificationCenter_ScheduledAnnouncements_Table
            {
                Title_TR = model.Title_TR,
                Conten_TR = model.Content_TR,
                Title_EN = model.Title_EN,
                Content_EN = model.Content_EN,
                Type = model.Type,
                ScheduleType = model.SelectedScheduleType,
                ScheduledDate = model.ScheduledDate,
                ScheduleTypeShow = scheduleTypeShowData,
                NextRunTime = nextRunTime,
                IsActive = true,
                CreateDate = DateTime.Now,
                CreatedByUserId = 1 // Örnek kullanıcı ID
            };

            await _scheduledAnnouncementsService.AddAsync(scheduledAnnouncement);
            // ID kaydedildikten sonra dosyaları ilişkilendirin
            if (model.Attachments != null && model.Attachments.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                // Yükleme klasörü mevcut değilse oluştur
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Dosyaları işlemeye başla
                foreach (var file in model.Attachments)
                {
                    if (file.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        // Dosyayı belirtilen klasöre kaydet
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Dosya entity nesnesini oluştur
                        var scheduledAnnouncementFile = new NotificationCenter_ScheduledAnnouncements_Files_Table
                        {
                            ScheduledAnnouncementId = scheduledAnnouncement.ID, // Duyuru kaydedildiği için geçerli ID alınır
                            FilePath = fileName // sadece dosya ismini saklıyoruz
                        };

                        // Dosya kaydını veritabanına ekle
                        await _scheduledFilesAnnouncementsService.AddAsync(scheduledAnnouncementFile);
                    }
                }
            }

            // Hangfire işini planla
            _hangfireJobService.ScheduleAnnouncementWithHangfire(scheduledAnnouncement);
            _toastHelper.CreateSuccess();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var announcement = await _scheduledAnnouncementsService.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            var files = await _scheduledFilesAnnouncementsService.GetFilesByAnnouncementIdAsync(id);
            var fileDetails = files.Select(f =>
            {
                var fileName = Path.GetFileName(f.FilePath);
                var parts = fileName.Split('_', 2);
                var displayName = (parts.Length > 1 && Guid.TryParse(parts[0], out _)) ? parts[1] : fileName;

                return (FullPath: f.FilePath, DisplayName: displayName);
            }).ToList();

            var model = new ScheduledAnnouncementEditViewModel
            {
                ID = announcement.ID,
                Title_TR = announcement.Title_TR,
                Content_TR = announcement.Conten_TR,
                Title_EN = announcement.Title_EN,
                Content_EN = announcement.Content_EN,
                Type = announcement.Type,
                ScheduledDate = announcement.ScheduledDate, // Belirli bir tarih önceden seçilmişse burada olacak
                SelectedScheduleType = announcement.ScheduleType,
                ScheduleTypes = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Her Ayın İlk Pazartesi", Value = "MonthlyFirstMonday" },
                    new SelectListItem { Text = "Her Ayın 15. Günü", Value = "Monthly15th" },
                    new SelectListItem { Text = "Her Hafta Pazartesi", Value = "WeeklyMonday" },
                    new SelectListItem { Text = "Belirli Bir Tarih", Value = "Once" }
                },
                ExistingFiles = fileDetails
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ScheduledAnnouncementEditViewModel model, List<string> FilesToDelete)
        {
            if (!ModelState.IsValid)
            {
                model.ScheduleTypes = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Her Ayın İlk Pazartesi", Value = "MonthlyFirstMonday" },
                    new SelectListItem { Text = "Her Ayın 15. Günü", Value = "Monthly15th" },
                    new SelectListItem { Text = "Her Hafta Pazartesi", Value = "WeeklyMonday" },
                    new SelectListItem { Text = "Belirli Bir Tarih", Value = "Once" }
                };
                _toastHelper.UpdateError();
                return View(model);
            }
            DateTime baseDate = model.ScheduledDate == default ? DateTime.Now : model.ScheduledDate;
            DateTime? nextRunTime = null;
            var currentDay = DateTime.Now;
            var scheduleTypeShowData = "";

            if (model.SelectedScheduleType == "MonthlyFirstMonday")
            {
                // Pazartesi kontrolü
                var isTodayMonday = currentDay.DayOfWeek == DayOfWeek.Monday;
                var targetTime = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, 10, 30, 0);

                if (isTodayMonday && currentDay < targetTime)
                {
                    // Bugün Pazartesi ve saat 10:30'dan önce
                    nextRunTime = targetTime;
                }
                else
                {
                    // Bir sonraki Pazartesi
                    var daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)currentDay.DayOfWeek + 7) % 7;
                    daysUntilNextMonday = daysUntilNextMonday == 0 ? 7 : daysUntilNextMonday;
                    var nextMonday = currentDay.AddDays(daysUntilNextMonday);
                    nextRunTime = new DateTime(nextMonday.Year, nextMonday.Month, nextMonday.Day, 10, 30, 0);
                }

                model.ScheduledDate = nextRunTime.Value;
                scheduleTypeShowData = "Her Ayın İlk Pazartesi";
            }
            else if (model.SelectedScheduleType == "Monthly15th")
            {
                // Bu ayın 15. günü
                var thisMonth15th = new DateTime(baseDate.Year, baseDate.Month, 15, 10, 30, 0);

                if (baseDate <= thisMonth15th && currentDay <= thisMonth15th)
                {
                    // Bugün veya ileride 15. gün (10:30'dan önce)
                    if (currentDay <= thisMonth15th && currentDay.TimeOfDay < new TimeSpan(10, 30, 0))
                    {
                        nextRunTime = thisMonth15th;
                    }
                    else
                    {
                        // Bir sonraki ayın 15. günü
                        var nextMonth15th = new DateTime(baseDate.Year, baseDate.Month, 15).AddMonths(1);
                        nextRunTime = AdjustToNextMondayIfWeekend(nextMonth15th);
                        nextRunTime = new DateTime(nextRunTime.Value.Year, nextRunTime.Value.Month, nextRunTime.Value.Day, 10, 30, 0);
                    }
                }
                else
                {
                    // Bir sonraki ayın 15. günü
                    var nextMonth15th = new DateTime(baseDate.Year, baseDate.Month, 15).AddMonths(1);
                    nextRunTime = AdjustToNextMondayIfWeekend(nextMonth15th);
                    nextRunTime = new DateTime(nextRunTime.Value.Year, nextRunTime.Value.Month, nextRunTime.Value.Day, 10, 30, 0);
                }

                model.ScheduledDate = nextRunTime.Value;
                scheduleTypeShowData = "Her Ayın 15. Günü";
            }

            else if (model.SelectedScheduleType == "WeeklyMonday")
            {
                // Pazartesi kontrolü
                var isTodayMonday = currentDay.DayOfWeek == DayOfWeek.Monday;
                var targetTime = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, 10, 30, 0);

                if (isTodayMonday && currentDay < targetTime)
                {
                    // Bugün Pazartesi ve saat 10:30'dan önce
                    nextRunTime = targetTime;
                }
                else
                {
                    // Bir sonraki Pazartesi'yi hesapla
                    var daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)currentDay.DayOfWeek + 7) % 7;
                    daysUntilNextMonday = daysUntilNextMonday == 0 ? 7 : daysUntilNextMonday;
                    var nextMonday = currentDay.AddDays(daysUntilNextMonday);
                    nextRunTime = new DateTime(nextMonday.Year, nextMonday.Month, nextMonday.Day, 10, 30, 0);
                }

                model.ScheduledDate = nextRunTime.Value;
                scheduleTypeShowData = "Her Hafta Pazartesi";
            }


            else if (model.SelectedScheduleType == "Once")
            {
                nextRunTime = model.ScheduledDate; // Belirli bir tarih seçilmişse aynı tarihi kullan
                scheduleTypeShowData = "Belirli Bir Tarih";
            }

            var scheduledAnnouncement = await _scheduledAnnouncementsService.GetByIdAsync(model.ID);
            if (scheduledAnnouncement == null)
            {
                return NotFound();
            }
            var previousScheduleType = scheduledAnnouncement.ScheduleType;
            // Duyuru güncelleme
            scheduledAnnouncement.Title_TR = model.Title_TR;
            scheduledAnnouncement.Conten_TR = model.Content_TR;
            scheduledAnnouncement.Title_EN = model.Title_EN;
            scheduledAnnouncement.Content_EN = model.Content_EN;
            scheduledAnnouncement.Type = model.Type;
            scheduledAnnouncement.ScheduleType = model.SelectedScheduleType;
            scheduledAnnouncement.ScheduledDate = model.ScheduledDate;
            scheduledAnnouncement.ScheduleTypeShow = scheduleTypeShowData;
            scheduledAnnouncement.NextRunTime = nextRunTime;

            // Duyuruya ait mevcut dosyaları alıyoruz
            var existingFiles = await _scheduledFilesAnnouncementsService.GetFilesByAnnouncementIdAsync(model.ID);

            // Silinmesi gereken dosyalar varsa işleme al
            if (FilesToDelete != null && FilesToDelete.Any())
            {
                foreach (var filePath in FilesToDelete)
                {
                    var fileEntity = existingFiles.FirstOrDefault(f => f.FilePath == filePath); // Dosya yoluna göre eşleştirme

                    if (fileEntity != null)
                    {
                        // Fiziksel dosyayı sil
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        // Veritabanından dosya kaydını sil
                        await _scheduledFilesAnnouncementsService.DeleteAsync(fileEntity);
                    }
                }
            }

            // Yeni dosyaları yükleme ve veritabanına ekleme
            if (model.Attachments != null && model.Attachments.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in model.Attachments)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Yeni dosyayı veritabanına kaydetme
                    var newFile = new NotificationCenter_ScheduledAnnouncements_Files_Table
                    {
                        ScheduledAnnouncementId = scheduledAnnouncement.ID,
                        FilePath = fileName
                    };
                    await _scheduledFilesAnnouncementsService.AddAsync(newFile);
                }
            }

            // Duyurunun mevcut job'larını silme


            if (previousScheduleType == "Once" && model.SelectedScheduleType == "Once")
            {
                using (var connection = JobStorage.Current.GetConnection())
                {
                    var monitoringApi = JobStorage.Current.GetMonitoringApi();
                    var scheduledJobs = monitoringApi.ScheduledJobs(0, int.MaxValue); // Tüm planlanmış işleri getir

                    Console.WriteLine("Silinecek Tek Seferlik İşler:");
                    foreach (var job in scheduledJobs)
                    {
                        var jobId = job.Key; // Job ID
                        var jobDetails = job.Value.Job;
                        var jobArgs = jobDetails.Args;
                        Console.WriteLine($"Siliniyor: Job ID: {jobId}, Args: {string.Join(", ", jobArgs)}");
                        // Argümanlarda duyuru ID'si var mı kontrol et
                        if (jobArgs.Any(arg => arg.ToString() == scheduledAnnouncement.ID.ToString()))
                        {
                            Console.WriteLine($"Siliniyor: Job ID: {jobId}, Args: {string.Join(", ", jobArgs)}");
                            BackgroundJob.Delete(jobId); // Job'ı sil
                        }
                    }
                }

            }
            else if (previousScheduleType == "Once" && model.SelectedScheduleType != "Once")
            {
                using (var connection = JobStorage.Current.GetConnection())
                {
                    var monitoringApi = JobStorage.Current.GetMonitoringApi();
                    var scheduledJobs = monitoringApi.ScheduledJobs(0, int.MaxValue); // Tüm planlanmış işleri getir

                    Console.WriteLine("Silinecek Tek Seferlik İşler:");
                    foreach (var job in scheduledJobs)
                    {
                        var jobId = job.Key; // Job ID
                        var jobDetails = job.Value.Job;
                        var jobArgs = jobDetails.Args;
                        Console.WriteLine($"Siliniyor: Job ID: {jobId}, Args: {string.Join(", ", jobArgs)}");
                        // Argümanlarda duyuru ID'si var mı kontrol et
                        if (jobArgs.Any(arg => arg.ToString() == scheduledAnnouncement.ID.ToString()))
                        {
                            Console.WriteLine($"Siliniyor: Job ID: {jobId}, Args: {string.Join(", ", jobArgs)}");
                            BackgroundJob.Delete(jobId); // Job'ı sil
                        }
                    }
                }
            }
            else
            {
                RecurringJob.RemoveIfExists(scheduledAnnouncement.ID.ToString());
            }
            // Yeni job oluştur
            _hangfireJobService.ScheduleAnnouncementWithHangfire(scheduledAnnouncement);
            await _scheduledAnnouncementsService.UpdateAsync(scheduledAnnouncement);
            _toastHelper.UpdateSuccess();
            return RedirectToAction("Index");
        }





        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var announcement = await _scheduledAnnouncementsService.GetByIdAsync(id);
                if (announcement == null)
                {
                    return NotFound();
                }

                RecurringJob.RemoveIfExists(announcement.ID.ToString());
                announcement.IsActive = false;
                await _scheduledAnnouncementsService.UpdateAsync(announcement);
                _toastHelper.DeleteSuccess();

            }
            catch (Exception ex)
            {
                _toastHelper.DeleteError(ex.Message);
            }
            return RedirectToAction("Index");
        }

        private DateTime? CalculateNextRunTime(DateTime scheduledDate, string scheduleType)
        {
            if (scheduleType == "MonthlyFirstMonday")
            {
                return GetFirstMondayOfNextMonth(scheduledDate);
            }
            else if (scheduleType == "Monthly15th")
            {
                return new DateTime(scheduledDate.Year, scheduledDate.Month, 15).AddMonths(1);
            }
            else if (scheduleType == "WeeklyMonday")
            {
                return scheduledDate.AddDays(7 - (int)scheduledDate.DayOfWeek + 1); // Pazartesiye ayarlar
            }
            return null;
        }

        private DateTime GetFirstMondayOfNextMonth(DateTime date)
        {
            var nextMonth = new DateTime(date.Year, date.Month, 1).AddMonths(1);
            while (nextMonth.DayOfWeek != DayOfWeek.Monday)
            {
                nextMonth = nextMonth.AddDays(1);
            }
            return nextMonth;
        }


    }
}
