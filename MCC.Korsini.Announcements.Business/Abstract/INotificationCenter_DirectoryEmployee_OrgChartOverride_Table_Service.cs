using MCC.Korsini.Announcements.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Abstract
{
    public interface INotificationCenter_DirectoryEmployee_OrgChartOverride_Table_Service
    {
        // Tüm override kayıtları
        Task<List<NotificationCenter_DirectoryEmployee_OrgChartOverride_Table>> GetAllAsync();

        // Id'ye göre tek kayıt
        Task<NotificationCenter_DirectoryEmployee_OrgChartOverride_Table?> GetByIdAsync(int id);

        // Belirli bir çalışan için (isteğe bağlı scope ile)
        Task<NotificationCenter_DirectoryEmployee_OrgChartOverride_Table?> GetByEmployeeSamAsync(
            string employeeSam,
            byte? scope = null
        );

        // Yeni kayıt ekle (Id döndürsün istersin diye int)
        Task<int> AddAsync(NotificationCenter_DirectoryEmployee_OrgChartOverride_Table entity);

        // Kayıt güncelle
        Task UpdateAsync(NotificationCenter_DirectoryEmployee_OrgChartOverride_Table entity);

        // Kayıt sil
        Task DeleteAsync(int id);
    }
}
