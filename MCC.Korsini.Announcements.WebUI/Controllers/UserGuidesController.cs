using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using MCC.Korsini.Announcements.WebUI.Helpers;
using MCC.Korsini.Announcements.WebUI.Models;
using MCC.Korsini.Announcements.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCC.Korsini.Announcements.WebUI.Controllers
{
    public class UserGuidesController : Controller
    {
        private readonly INotificationCenter_UserGuides_Table_Service _userGuidesService;
        private readonly INotificationCenter_UserGuides_Files_Table_Service _userGuidesFilesService;
        private readonly ToastHelper _toastHelper;

        public UserGuidesController(INotificationCenter_UserGuides_Table_Service userGuidesService,
            INotificationCenter_UserGuides_Files_Table_Service userGuidesFilesService, ToastHelper toastHelper)
        {
            _userGuidesService = userGuidesService;
            _userGuidesFilesService = userGuidesFilesService;
            _toastHelper = toastHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Prosedürleri ve ilişkili dosyaları dahil ederek veriyi çekiyoruz
            var userGuides = await _userGuidesService.GetAllAsync(p => p.Files);
            var descProcedures = userGuides.OrderBy(p => p.Title).ToList();
            // ViewModel'e dönüştürüyoruz
            var viewModel = descProcedures.Select(p => new UserGuidesListViewModel
            {
                ID = p.ID,
                Title = p.Title,
                Files = p.Files.Select(f => new UserGuidesFilesListViewModel
                {
                    ID = f.ID,
                    FileName = f.FileName,
                    FilePath = f.FilePath,
                    UploadedDate = f.UploadedDate
                }).ToList()
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new UserGuideAddViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserGuideAddViewModel model)
        {
            // Model geçerli değilse aynı sayfaya döner
            if (!ModelState.IsValid)
            {
                _toastHelper.CreateError();
                return View(model);
            }

            // Yeni prosedür entity'si oluşturulur
            var userGuide = new NotificationCenter_UserGuides_Table
            {
                Title = model.Title
            };

            // Prosedürü kaydedin ve ID'yi alın
            await _userGuidesService.AddAsync(userGuide);

            // Eğer dosyalar varsa işlem yapılır
            if (model.Attachments != null && model.Attachments.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userguides");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in model.Attachments)
                {
                    if (file.Length > 0)
                    {
                        var fileGUID = Guid.NewGuid().ToString(); // GUID oluştur
                        var fileName = file.FileName; // Orijinal dosya adı
                        var fullFileName = $"{fileGUID}_{fileName}"; // GUID ve dosya adını birleştir
                        var filePath = Path.Combine(uploadsFolder, fullFileName); // Tam dosya yolu

                        // Dosyayı belirtilen klasöre kaydet
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Dosya entity nesnesi oluşturulur
                        var userGuideFiles = new NotificationCenter_UserGuides_Files_Table
                        {
                            UserGuideID = userGuide.ID, // Prosedür ID'si artık mevcut
                            GUID = fileGUID,
                            FileName = fileName,
                            FilePath = fullFileName, // Web yolu
                            UploadedDate = DateTime.Now,
                            UploadedByUserID = 1 // Kimlik doğrulama altyapısı eklenince güncellenebilir
                        };

                        // Dosya kaydını yapın
                        await _userGuidesFilesService.AddAsync(userGuideFiles);
                    }
                }
            }
            _toastHelper.CreateSuccess();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            // İlişkili dosyalar dahil edilerek prosedür alınır
            var userGuides = await _userGuidesService.GetByIdAsync(id, p => p.Files);
            if (userGuides == null)
            {
                return NotFound();
            }

            // İlişkili dosyalar ViewModel'e dönüştürülür
            var files = userGuides.Files.Select(f => new ExistingUserGuideFileViewModel
            {
                ID = f.ID,
                FilePath = f.FilePath,
                DisplayName = f.FileName
            }).ToList();

            var viewModel = new UserGuideUpdateViewModel
            {
                ID = userGuides.ID,
                Title = userGuides.Title,
                ExistingFiles = files
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserGuideUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _toastHelper.UpdateError();
                return View(model);
            }

            var userGuide = await _userGuidesService.GetByIdAsync(model.ID, p => p.Files);
            if (userGuide == null)
            {
                return NotFound();
            }

            // Prosedür başlığını güncelle
            userGuide.Title = model.Title;

            // Silinecek dosyalar
            if (model.FilesToDelete != null && model.FilesToDelete.Any())
            {
                foreach (var fileID in model.FilesToDelete)
                {
                    var file = userGuide.Files.FirstOrDefault(f => f.ID == Convert.ToInt32(fileID));
                    if (file != null)
                    {
                        // Fiziksel dosyayı sil
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userguides", file.FilePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        // Dosyayı veritabanından sil
                        await _userGuidesFilesService.DeleteAsync(file);
                    }
                }
            }

            // Yeni dosyaları yükleme
            if (model.NewAttachments != null && model.NewAttachments.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userguides");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in model.NewAttachments)
                {
                    var fileGUID = Guid.NewGuid().ToString(); // GUID oluştur
                    var fileName = file.FileName; // Orijinal dosya adı
                    var fullFileName = $"{fileGUID}_{fileName}"; // GUID ve dosya adını birleştir
                    var filePath = Path.Combine(uploadsFolder, fullFileName); // Tam dosya yolu

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Yeni dosya bilgilerini veritabanına ekle
                    var newFile = new NotificationCenter_UserGuides_Files_Table
                    {
                        UserGuideID = userGuide.ID,
                        GUID = fileGUID,
                        FileName = fileName,
                        FilePath = fullFileName,
                        UploadedDate = DateTime.Now,
                        UploadedByUserID = 1 // Kimlik doğrulama ile güncellenebilir
                    };

                    await _userGuidesFilesService.AddAsync(newFile);
                }
            }

            // Prosedürü güncelle
            await _userGuidesService.UpdateAsync(userGuide);
            _toastHelper.UpdateSuccess();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                // Prosedürle birlikte ilişkili dosyaları al
                var userGuide = await _userGuidesService.GetByIdAsync(id, p => p.Files);
                if (userGuide == null)
                {
                    return NotFound();
                }

                // İlişkili dosyaları fiziksel olarak sil
                if (userGuide.Files != null && userGuide.Files.Any())
                {
                    foreach (var file in userGuide.Files)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userguides",
                            file.FilePath);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath); // Fiziksel dosyayı sil
                        }

                        // Dosyayı veritabanından sil
                        await _userGuidesFilesService.DeleteAsync(file);
                    }
                }

                // Prosedürü veritabanından sil
                await _userGuidesService.DeleteAsync(userGuide);
                _toastHelper.DeleteSuccess();

            }
            catch (Exception ex)
            {
                _toastHelper.DeleteError(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
