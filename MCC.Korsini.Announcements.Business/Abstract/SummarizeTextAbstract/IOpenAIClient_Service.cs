using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Abstract.SummarizeTexAbstract
{
    public interface IOpenAIClient_Service
    {
        Task<string> SummarizeTextAsync(string text);
    }
}
