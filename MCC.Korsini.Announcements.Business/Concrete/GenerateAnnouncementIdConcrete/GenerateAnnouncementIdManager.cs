using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.Business.Abstract.GenerateAnnouncementIdAbstract;

namespace MCC.Korsini.Announcements.Business.Concrete.GenerateAnnouncementIdConcrete
{
    public class GenerateAnnouncementIdManager : IGenerateAnnouncementIdService
    {
        private readonly INotificationCenter_Announcements_Table_Service _announcementsTableService;
        private readonly INotificationCenter_ScheduledAnnouncements_Table_Service _scheduledAnnouncementsTableService;

        public GenerateAnnouncementIdManager(INotificationCenter_Announcements_Table_Service announcementsTableService, INotificationCenter_ScheduledAnnouncements_Table_Service scheduledAnnouncementsTableService)
        {
            _announcementsTableService = announcementsTableService;
            _scheduledAnnouncementsTableService = scheduledAnnouncementsTableService;
        }

        public async Task<string> GenerateAnnouncementId()
        {
            var yearSuffix = DateTime.Now.Year.ToString().Substring(2);
            var lastAnnouncement = (await _announcementsTableService.GetAllAsync())
                .OrderByDescending(a => a.ID)
                .FirstOrDefault();

            int nextId = 1;
            if (lastAnnouncement != null && lastAnnouncement.AnnouncementId.Length >= 2)
            {
                var lastNumberPart = lastAnnouncement.AnnouncementId.Split('-').Last();
                if (int.TryParse(lastNumberPart, out int lastNumber))
                {
                    nextId = lastNumber + 1;
                }
            }

            return $"DYR-IT-{yearSuffix}-{nextId:D2}";
        }
    }
}
