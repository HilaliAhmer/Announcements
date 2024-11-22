using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.Business.Abstract.AnnouncementMailAbstract;
using MCC.Korsini.Announcements.Business.Abstract.GenerateAnnouncementIdAbstract;
using MCC.Korsini.Announcements.Business.Abstract.HtmlSanitizerAbstract;
using MCC.Korsini.Announcements.Business.Abstract.SummarizeTexAbstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using MCC.Korsini.Announcements.WebUI.Helpers;
using MCC.Korsini.Announcements.WebUI.Models;
using MCC.Korsini.Announcements.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MCC.Korsini.Announcements.WebUI.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly INotificationCenter_Announcements_Table_Service _announcementsTableService;
        private readonly IHtmlSanitizerService _htmlSanitizerService;
        private readonly IAnnouncementMailService _announcementMailService;
        private readonly INotificationCenter_Announcement_Files_Table_Service _announcementFilesTableService;
        private readonly INotificationCenter_Announcement_Type_Table_Service _announcementTypeTableService;
        private readonly IGenerateAnnouncementIdService _generateAnnouncementIdService;
        private readonly ToastHelper _toastHelper;
        private readonly IConfiguration _configuration;
        //private readonly IOpenAIClient_Service _openAiClientService;

        public AnnouncementController(IHtmlSanitizerService htmlSanitizerService,
            IAnnouncementMailService announcementMailService, INotificationCenter_Announcements_Table_Service announcementsTableService, INotificationCenter_Announcement_Files_Table_Service announcementFilesTableService, INotificationCenter_Announcement_Type_Table_Service announcementTypeTableService, IOpenAIClient_Service openAiClientService, IGenerateAnnouncementIdService generateAnnouncementIdService, ToastHelper toastHelper, IConfiguration configuration)
        {
            _announcementsTableService = announcementsTableService;
            _announcementFilesTableService = announcementFilesTableService;
            _announcementTypeTableService = announcementTypeTableService;
            _generateAnnouncementIdService = generateAnnouncementIdService;
            _toastHelper = toastHelper;
            _configuration = configuration;
            //_openAiClientService = openAiClientService;
            _htmlSanitizerService = htmlSanitizerService;
            _announcementMailService = announcementMailService;
            _announcementsTableService = announcementsTableService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 15)
        {
            var announcements = await _announcementsTableService.GetAllDescAsync();
            announcements
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            AnnouncementsListViewModel model = new AnnouncementsListViewModel
            {
                Announcements = announcements

            };
            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var announcementEntity = await _announcementsTableService.GetByIdAsync(id);
            if (announcementEntity == null)
            {
                return NotFound();
            }

            var files = await _announcementFilesTableService.GetFilesByAnnouncementIdAsync(id);
            var filePaths = files.Select(f =>
            {
                var fileName = Path.GetFileName(f.FilePath);
                var parts = fileName.Split('_', 2); // İlk "_" karakterine göre ayırır
                var displayName = (parts.Length > 1 && Guid.TryParse(parts[0], out _)) ? parts[1] : fileName;

                return (FullPath: f.FilePath, DisplayName: displayName); // Tuple ile hem tam yolu hem de gösterilecek adı ekliyoruz
            }).ToList();

            var model = new AnnouncementDetailViewModel
            {
                ID = announcementEntity.ID,
                AnnouncementId = announcementEntity.AnnouncementId,
                Title_TR = announcementEntity.Title_TR,
                Content_TR = announcementEntity.Conten_TR,
                Title_EN = announcementEntity.Title_EN,
                Content_EN = announcementEntity.Content_EN,
                Type = announcementEntity.Type,
                CreateDate = announcementEntity.CreateDate,
                Publishing = announcementEntity.Publishing,
                FilePaths = filePaths // Dosya yollarını ve adlarını modeldeki FilePaths alanına ekliyoruz
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddAnnouncements()
        {
            var announcementTypes = await _announcementTypeTableService.GetAllAsync();

            // Veriyi SelectListItem listesine dönüştürme
            var types = announcementTypes
                .Select(t => new SelectListItem
                {
                    Value = t.ID.ToString(), // ID değeri
                    Text = t.Announcement_Type// Gösterilecek isim
                }).ToList();

            // ViewModel’e ekleme
            var viewModel = new AnnouncementAddViewModel()
            {
                Types = types
            };

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAnnouncements(AnnouncementAddViewModel model)
        {
            // Model geçerli değilse aynı sayfaya döner
            if (!ModelState.IsValid)
            {
                _toastHelper.CreateError();
                return View(model);
            }

            // Content_TR içeriğini sanitize et ve doğrula
            var (sanitizedContentTR, isValidTR, errorMessageTR) = _htmlSanitizerService.SanitizeAndValidate(model.Content_TR);

            if (!isValidTR)
            {
                ModelState.AddModelError("TinyMCETextArea_TR", errorMessageTR);
                return View(model);
            }

            // Content_EN içeriğini sanitize et ve doğrula
            var (sanitizedContentEN, isValidEN, errorMessageEN) = _htmlSanitizerService.SanitizeAndValidate(model.Content_EN);

            if (!isValidEN)
            {
                ModelState.AddModelError("TinyMCETextArea_EN", errorMessageEN);
                return View(model);
            }

            // Duyuru ID'si için benzersiz bir format oluştur
            var formattedAnnouncementId = await _generateAnnouncementIdService.GenerateAnnouncementId();

            // Yeni duyuru entity'sini oluştur
            var announcement = new NotificationCenter_Announcements_Table
            {
                AnnouncementId = formattedAnnouncementId,
                Title_TR = model.Title_TR,
                Conten_TR = model.Content_TR,
                Title_EN = model.Title_EN,
                Content_EN = model.Content_EN,
                Type = model.Type,
                CreateDate = model.CreateDate
            };

            // Duyuruyu veritabanına kaydedin ve ID'sini alın
            await _announcementsTableService.AddAsync(announcement);

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
                        var announcementFile = new NotificationCenter_Announcement_Files_Table
                        {
                            AnnouncementId = announcement.ID, // Duyuru kaydedildiği için geçerli ID alınır
                            FilePath = fileName // sadece dosya ismini saklıyoruz
                        };

                        // Dosya kaydını veritabanına ekle
                        await _announcementFilesTableService.AddAsync(announcementFile);
                    }
                }
            }
            _toastHelper.CreateSuccess();
            // İşlem tamamlandıktan sonra duyuruların listelendiği sayfaya yönlendir
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PublishAnnouncementEmail(int id)
        {
            try
            {
                var announcementEntity = await _announcementsTableService.GetByIdAsync(id);
                if (announcementEntity == null)
                {
                    return NotFound();
                }

                var htmlContent = $@"
                                <div>
                                    <h3>{announcementEntity.Title_TR}</h3>
                                    <div>{announcementEntity.Conten_TR}</div>
                                    <hr style='margin-top: 20px; margin-bottom: 20px;' />
                                    <h3>{announcementEntity.Title_EN}</h3>
                                    <div>{announcementEntity.Content_EN}</div>
                                    <p><small>{announcementEntity.CreateDate}</small></p>
                                </div>";

                string recipientEmail = _configuration["SmtpSettings:RecipientEmail"];
                string subject = $"{announcementEntity.AnnouncementId} - [{announcementEntity.Type}]: {announcementEntity.Title_TR} / {announcementEntity.Title_EN}";
                var files = await _announcementFilesTableService.GetFilesByAnnouncementIdAsync(id);
                await _announcementMailService.SendAnnouncementEmailAsync(recipientEmail, subject, htmlContent, files.Select(f => f.FilePath).ToList());
                announcementEntity.Publishing = true;
                await _announcementsTableService.UpdateAsync(announcementEntity);
                _toastHelper.EmailSendSuccess();
            }
            catch (Exception ex)
            {
                _toastHelper.EmailSendError(ex.Message);
            }
            
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditAnnouncement(int id)
        {
            var announcementTypes = await _announcementTypeTableService.GetAllAsync();

            // Veriyi SelectListItem listesine dönüştürme
            var types = announcementTypes
                .Select(t => new SelectListItem
                {
                    Value = t.ID.ToString(), // ID değeri
                    Text = t.Announcement_Type// Gösterilecek isim
                }).ToList();

            var announcement = await _announcementsTableService.GetByIdAsync(id);
            if (announcement == null || announcement.Publishing)
            {
                return NotFound();
            }

            var files = await _announcementFilesTableService.GetFilesByAnnouncementIdAsync(id);
            var fileDetails = files.Select(f =>
            {
                var fileName = Path.GetFileName(f.FilePath);
                var parts = fileName.Split('_', 2);
                var displayName = (parts.Length > 1 && Guid.TryParse(parts[0], out _)) ? parts[1] : fileName;

                return (FullPath: f.FilePath, DisplayName: displayName);
            }).ToList();

            var model = new AnnouncementEditViewModel
            {
                ID = announcement.ID,
                Title_TR = announcement.Title_TR,
                Content_TR = announcement.Conten_TR,
                Title_EN = announcement.Title_EN,
                Content_EN = announcement.Content_EN,
                Type = announcement.Type,
                CreateDate = announcement.CreateDate,
                ExistingFiles = fileDetails, // Mevcut dosyaları modeldeki ExistingFiles alanına ekliyoruz
                Types = types
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditAnnouncement(AnnouncementEditViewModel model, List<string> FilesToDelete)
        {
            if (!ModelState.IsValid)
            {
                _toastHelper.UpdateError();
                return View(model);
            }

            var announcement = await _announcementsTableService.GetByIdAsync(model.ID);
            if (announcement == null || announcement.Publishing)
            {
                return NotFound(); // Yayınlanan duyurular düzenlenemez
            }

            // Duyuru bilgilerini güncelleme
            announcement.Title_TR = model.Title_TR;
            announcement.Conten_TR = model.Content_TR;
            announcement.Title_EN = model.Title_EN;
            announcement.Content_EN = model.Content_EN;
            announcement.Type = model.Type;
            announcement.CreateDate = model.CreateDate;

            // Duyuruya ait mevcut dosyaları alıyoruz
            var existingFiles = await _announcementFilesTableService.GetFilesByAnnouncementIdAsync(model.ID);

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
                        await _announcementFilesTableService.DeleteAsync(fileEntity);
                    }
                }
            }
            // Yeni dosyaları yükleme ve veritabanına ekleme
            if (model.NewAttachments != null && model.NewAttachments.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in model.NewAttachments)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Yeni dosyayı veritabanına kaydetme
                    var newFile = new NotificationCenter_Announcement_Files_Table
                    {
                        AnnouncementId = announcement.ID,
                        FilePath = fileName
                    };
                    await _announcementFilesTableService.AddAsync(newFile);
                }
            }

            await _announcementsTableService.UpdateAsync(announcement);
            _toastHelper.UpdateSuccess();
            return RedirectToAction("Detail", new { id = model.ID });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            try
            {
                var announcement = await _announcementsTableService.GetByIdAsync(id);
                if (announcement == null || announcement.Publishing)
                {
                    return NotFound(); // Yayınlanan duyurular silinemez
                }

                await _announcementsTableService.DeleteAsync(announcement);
                _toastHelper.DeleteSuccess();
            }
            catch (Exception ex)
            {
                _toastHelper.DeleteError(ex.Message);
            }
            
            return RedirectToAction("Index"); // Silme işleminden sonra listeye yönlendirme
        }
    }
}
