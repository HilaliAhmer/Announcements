using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_Procedures_Files_Table_Manager :INotificationCenter_Procedures_Files_Table_Service
    {
        private readonly INotificationCenter_Procedures_Files_Table_Dal _procedureFiles;

        public NotificationCenter_Procedures_Files_Table_Manager(INotificationCenter_Procedures_Files_Table_Dal procedureFiles)
        {
            _procedureFiles = procedureFiles;
        }

        public async Task<List<NotificationCenter_Procedures_Files_Table>> GetAllAsync()
        {
            return await _procedureFiles.GetListAsync();
        }

        public async Task<NotificationCenter_Procedures_Files_Table> GetByIdAsync(int procedureId)
        {
            return await _procedureFiles.GetAsync(pf => pf.ID == procedureId);
        }

        public async Task<List<NotificationCenter_Procedures_Files_Table>> GetFilesByAnnouncementIdAsync(int procedureId)
        {
            return await _procedureFiles.GetListAsync(pf => pf.ProcedureID == procedureId);
        }

        public Task AddAsync(NotificationCenter_Procedures_Files_Table procedure)
        {
            return _procedureFiles.AddAsync(procedure);
        }

        public Task UpdateAsync(NotificationCenter_Procedures_Files_Table procedure)
        {
            return _procedureFiles.UpdateAsync(procedure);
        }

        public Task DeleteAsync(NotificationCenter_Procedures_Files_Table procedure)
        {
            return _procedureFiles.DeleteAsync(procedure);
        }
    }
}
