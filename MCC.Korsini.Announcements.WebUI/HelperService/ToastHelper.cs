using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MCC.Korsini.Announcements.WebUI.Helpers
{
    public class ToastHelper
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ToastHelper(ITempDataDictionaryFactory tempDataFactory, IHttpContextAccessor httpContextAccessor)
        {
            _tempDataFactory = tempDataFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private ITempDataDictionary TempData =>
            _tempDataFactory.GetTempData(_httpContextAccessor.HttpContext);

        private void SetToast(string type, string message)
        {
            TempData["ToastMessage"] = message;
            TempData["ToastType"] = type;
        }

        public void CreateSuccess() => SetToast("success", "Kayıt başarıyla oluşturuldu.");
        public void CreateError() => SetToast("error", "Kayıt oluşturulurken bir hata oluştu.");

        public void UpdateSuccess() => SetToast("success", "Güncelleme işlemi başarıyla tamamlandı.");
        public void UpdateError(string errorMessage = null)
        {
            var message = string.IsNullOrWhiteSpace(errorMessage)
                ? "Güncelleme işlemi sırasında bir hata oluştu."
                : $"Güncelleme işlemi sırasında bir hata oluştu: {errorMessage}";
            SetToast("error", message);
        }

        public void DeleteSuccess() => SetToast("success", "Silme işlemi başarıyla tamamlandı.");
        public void DeleteError(string errorMessage = null)
        {
            var message = string.IsNullOrWhiteSpace(errorMessage)
                ? "Silme işlemi sırasında bir hata oluştu."
                : $"Silme işlemi sırasında bir hata oluştu: {errorMessage}";
            SetToast("error", message);
        }

        public void EmailSendSuccess() => SetToast("success", "E-posta başarıyla gönderildi.");
        public void EmailSendError(string errorMessage = null)
        {
            var message = string.IsNullOrWhiteSpace(errorMessage)
                ? "E-posta gönderimi sırasında bir hata oluştu."
                : $"E-posta gönderimi sırasında bir hata oluştu: {errorMessage}";
            SetToast("error", message);
        }
    }
}
