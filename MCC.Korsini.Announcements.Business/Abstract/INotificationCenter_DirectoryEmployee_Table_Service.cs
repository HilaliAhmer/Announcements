using MCC.Korsini.Announcements.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_DirectoryEmployee_Table_Service
    {
        Task<List<NotificationCenter_DirectoryEmployee_Table>> GetAllAsync();
        Task<NotificationCenter_DirectoryEmployee_Table> GetByIdAsync(int id);
        Task<NotificationCenter_DirectoryEmployee_Table> GetBySamAccountNameAsync(string samAccountName);
        Task<List<NotificationCenter_DirectoryEmployee_Table>> GetDirectReportsAsync(int managerId);
        Task AddAsync(NotificationCenter_DirectoryEmployee_Table employee);
        Task UpdateAsync(NotificationCenter_DirectoryEmployee_Table employee);
        Task DeleteAsync(int id);

        /// <summary>
        /// Arama çubuğu için isim filtreleme
        /// </summary>
        Task<List<NotificationCenter_DirectoryEmployee_Table>> SearchByNameAsync(string searchText);
    }
}
