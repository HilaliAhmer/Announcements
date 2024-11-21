using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Abstract.GenerateAnnouncementIdAbstract
{
    public interface IGenerateAnnouncementIdService
    {
        Task<string> GenerateAnnouncementId();
    }
}
