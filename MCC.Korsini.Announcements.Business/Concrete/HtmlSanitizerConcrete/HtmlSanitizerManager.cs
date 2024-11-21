using Ganss.Xss;
using MCC.Korsini.Announcements.Business.Abstract.HtmlSanitizerAbstract;

namespace MCC.Korsini.Announcements.Business.Concrete.HtmlSanitizerConcrete
{
    public class HtmlSanitizerManager : IHtmlSanitizerService
    {
        private readonly HtmlSanitizer _htmlSanitizer;

        public HtmlSanitizerManager()
        {
            _htmlSanitizer = new HtmlSanitizer(); // HtmlSanitizer doğrudan burada örnekleniyor

            // Tüm etiketlere izin verir, ancak belirli etiketleri kaldırmak için bir olay işleyici tanımlar
            _htmlSanitizer.AllowedTags.Clear(); // Öncelikle tüm etiketlere izin veriyoruz

            _htmlSanitizer.RemovingTag += (sender, args) =>
            {
                if (args.Tag.TagName.Equals("script", StringComparison.OrdinalIgnoreCase) ||
                    args.Tag.TagName.Equals("iframe", StringComparison.OrdinalIgnoreCase))
                {
                    args.Cancel = false; // <script> ve <iframe> etiketlerini kaldır
                }
                else
                {
                    args.Cancel = true; // Diğer tüm etiketleri koru
                }
            };
        }

        public (string sanitizedContent, bool isValid, string errorMessage) SanitizeAndValidate(string content)
        {
            // İçeriği sanitize ediyoruz
            var sanitizedContent = _htmlSanitizer.Sanitize(content);

            // Yasaklı etiketleri içeren içerik geçersiz olarak işaretlenir
            bool isValid = !content.Contains("<script") && !content.Contains("<iframe");
            string errorMessage = isValid ? null : "Girilen içerikte yasaklı etiketler (<script> veya <iframe>) mevcut. Lütfen bu etiketleri kullanmayın.";

            return (sanitizedContent, isValid, errorMessage);
        }
    }
}