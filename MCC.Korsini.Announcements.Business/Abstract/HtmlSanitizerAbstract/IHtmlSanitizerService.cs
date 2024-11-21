using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Abstract.HtmlSanitizerAbstract
{
    public interface IHtmlSanitizerService
    {
        (string sanitizedContent, bool isValid, string errorMessage) SanitizeAndValidate(string content);
    }
}
