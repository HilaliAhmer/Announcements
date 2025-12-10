using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_DirectoryEmployee_OrgChartOverride_Table_Manager:INotificationCenter_DirectoryEmployee_OrgChartOverride_Table_Service
    {
        private readonly INotificationCenter_DirectoryEmployee_OrgChartOverride_Table_Dal _overrideDal;

        public NotificationCenter_DirectoryEmployee_OrgChartOverride_Table_Manager(
            INotificationCenter_DirectoryEmployee_OrgChartOverride_Table_Dal overrideDal)
        {
            _overrideDal = overrideDal;
        }

        public async Task<List<NotificationCenter_DirectoryEmployee_OrgChartOverride_Table>> GetAllAsync()
        {
            return await _overrideDal.GetListAsync();
        }

        public async Task<NotificationCenter_DirectoryEmployee_OrgChartOverride_Table?> GetByIdAsync(int id)
        {
            var all = await _overrideDal.GetListAsync();
            return all.FirstOrDefault(x => x.Id == id);
        }

        public async Task<NotificationCenter_DirectoryEmployee_OrgChartOverride_Table?> GetByEmployeeSamAsync(
            string employeeSam,
            byte? scope = null)
        {
            if (string.IsNullOrWhiteSpace(employeeSam))
                return null;

            employeeSam = employeeSam.Trim().ToLowerInvariant();

            var all = await _overrideDal.GetListAsync();

            var query = all.Where(x =>
                x.EmployeeSam != null &&
                x.EmployeeSam.ToLower() == employeeSam
            );

            if (scope.HasValue)
            {
                query = query.Where(x => x.Scope == scope.Value);
            }

            // Aynı employee için birden fazla kayıt varsa SortOrder'a göre seç
            return query
                .OrderBy(x => x.SortOrder ?? int.MaxValue)
                .FirstOrDefault();
        }

        public async Task<int> AddAsync(NotificationCenter_DirectoryEmployee_OrgChartOverride_Table entity)
        {
            await _overrideDal.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(NotificationCenter_DirectoryEmployee_OrgChartOverride_Table entity)
        {
            await _overrideDal.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var all = await _overrideDal.GetListAsync();
            var entity = all.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return;

            await _overrideDal.DeleteAsync(entity);
        }
    }
}
