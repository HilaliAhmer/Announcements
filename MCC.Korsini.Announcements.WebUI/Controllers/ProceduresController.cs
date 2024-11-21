using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using MCC.Korsini.Announcements.WebUI.Helpers;
using MCC.Korsini.Announcements.WebUI.Models;
using MCC.Korsini.Announcements.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCC.Korsini.Announcements.WebUI.Controllers
{
    public class ProceduresController : Controller
    {
        private readonly INotificationCenter_Procedures_Table_Service _proceduresService;
        private readonly INotificationCenter_Procedures_Files_Table_Service _proceduresFilesService;
        private readonly ToastHelper _toastHelper;

        public ProceduresController(INotificationCenter_Procedures_Table_Service proceduresService, INotificationCenter_Procedures_Files_Table_Service proceduresFilesService, ToastHelper toastHelper)
        {
            _proceduresService = proceduresService;
            _proceduresFilesService = proceduresFilesService;
            _toastHelper = toastHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Prosedürleri ve ilişkili dosyaları dahil ederek veriyi çekiyoruz
            var procedures = await _proceduresService.GetAllAsync(p => p.Files);
            var descProcedures = procedures.OrderBy(p=>p.Title).ToList();
            // ViewModel'e dönüştürüyoruz
            var viewModel = descProcedures.Select(p => new ProceduresListViewModel
            {
                ID = p.ID,
                Title = p.Title,
                Files = p.Files.Select(f => new ProcedureFilesListViewModel
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
            var viewModel = new ProcedureAddViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProcedureAddViewModel model)
        {
            // Model geçerli değilse aynı sayfaya döner
            if (!ModelState.IsValid)
            {
                _toastHelper.CreateError();
                return View(model);
            }

            // Yeni prosedür entity'si oluşturulur
            var procedure = new NotificationCenter_Procedures_Table
            {
                Title = model.Title
            };

            // Prosedürü kaydedin ve ID'yi alın
            await _proceduresService.AddAsync(procedure);

            // Eğer dosyalar varsa işlem yapılır
            if (model.Attachments != null && model.Attachments.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/procedures");
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
                        var procedureFiles = new NotificationCenter_Procedures_Files_Table
                        {
                            ProcedureID = procedure.ID, // Prosedür ID'si artık mevcut
                            GUID = fileGUID,
                            FileName = fileName,
                            FilePath = fullFileName, // Web yolu
                            UploadedDate = DateTime.Now,
                            UploadedByUserID = 1 // Kimlik doğrulama altyapısı eklenince güncellenebilir
                        };

                        // Dosya kaydını yapın
                        await _proceduresFilesService.AddAsync(procedureFiles);
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
            var procedure = await _proceduresService.GetByIdAsync(id, p => p.Files);
            if (procedure == null)
            {
                return NotFound();
            }

            // İlişkili dosyalar ViewModel'e dönüştürülür
            var files = procedure.Files.Select(f => new ExistingFileViewModel
            {
                ID = f.ID,
                FilePath = f.FilePath,
                DisplayName = f.FileName
            }).ToList();

            var viewModel = new ProcedureUpdateViewModel
            {
                ID = procedure.ID,
                Title = procedure.Title,
                ExistingFiles = files
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProcedureUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _toastHelper.UpdateError();
                return View(model);
            }

            var procedure = await _proceduresService.GetByIdAsync(model.ID, p => p.Files);
            if (procedure == null)
            {
                return NotFound();
            }

            // Prosedür başlığını güncelle
            procedure.Title = model.Title;

            // Silinecek dosyalar
            if (model.FilesToDelete != null && model.FilesToDelete.Any())
            {
                foreach (var fileID in model.FilesToDelete)
                {
                    var file = procedure.Files.FirstOrDefault(f => f.ID == Convert.ToInt32(fileID));
                    if (file != null)
                    {
                        // Fiziksel dosyayı sil
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/procedures", file.FilePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        // Dosyayı veritabanından sil
                        await _proceduresFilesService.DeleteAsync(file);
                    }
                }
            }

            // Yeni dosyaları yükleme
            if (model.NewAttachments != null && model.NewAttachments.Any())
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/procedures");
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
                    var newFile = new NotificationCenter_Procedures_Files_Table
                    {
                        ProcedureID = procedure.ID,
                        GUID = fileGUID,
                        FileName = fileName,
                        FilePath = fullFileName,
                        UploadedDate = DateTime.Now,
                        UploadedByUserID = 1 // Kimlik doğrulama ile güncellenebilir
                    };

                    await _proceduresFilesService.AddAsync(newFile);
                }
            }

            // Prosedürü güncelle
            await _proceduresService.UpdateAsync(procedure);
            _toastHelper.UpdateSuccess();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                // Prosedürle birlikte ilişkili dosyaları al
                var procedure = await _proceduresService.GetByIdAsync(id, p => p.Files);
                if (procedure == null)
                {
                    return NotFound();
                }

                // İlişkili dosyaları fiziksel olarak sil
                if (procedure.Files != null && procedure.Files.Any())
                {
                    foreach (var file in procedure.Files)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/procedures",
                            file.FilePath);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath); // Fiziksel dosyayı sil
                        }

                        // Dosyayı veritabanından sil
                        await _proceduresFilesService.DeleteAsync(file);
                    }
                }

                // Prosedürü veritabanından sil
                await _proceduresService.DeleteAsync(procedure);
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
