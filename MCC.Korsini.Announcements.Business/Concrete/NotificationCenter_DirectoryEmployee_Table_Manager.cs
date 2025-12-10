using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_DirectoryEmployee_Table_Manager
        : INotificationCenter_DirectoryEmployee_Table_Service
    {
        private readonly INotificationCenter_DirectoryEmployee_Table_Dal _employeeDal;

        public NotificationCenter_DirectoryEmployee_Table_Manager(
            INotificationCenter_DirectoryEmployee_Table_Dal employeeDal)
        {
            _employeeDal = employeeDal;
        }

        public async Task<List<NotificationCenter_DirectoryEmployee_Table>> GetAllAsync()
        {
            return await _employeeDal.GetListAsync();
        }

        public async Task<NotificationCenter_DirectoryEmployee_Table> GetByIdAsync(int id)
        {
            return await _employeeDal.GetAsync(x => x.Id == id);
        }

        public async Task<NotificationCenter_DirectoryEmployee_Table> GetBySamAccountNameAsync(string sam)
        {
            return await _employeeDal.GetAsync(x => x.SamAccountName == sam);
        }

        public async Task<List<NotificationCenter_DirectoryEmployee_Table>> GetDirectReportsAsync(int managerId)
        {
            return await _employeeDal.GetListAsync(x => x.ManagerId == managerId);
        }

        public async Task AddAsync(NotificationCenter_DirectoryEmployee_Table employee)
        {
            await _employeeDal.AddAsync(employee);
        }

        public async Task UpdateAsync(NotificationCenter_DirectoryEmployee_Table employee)
        {
            await _employeeDal.UpdateAsync(employee);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _employeeDal.GetAsync(x => x.Id == id);
            if (entity != null)
                await _employeeDal.DeleteAsync(entity);
        }

        public async Task<List<NotificationCenter_DirectoryEmployee_Table>> SearchByNameAsync(string search)
        {
            search = (search ?? "").ToLower();

            return await _employeeDal.GetListAsync(x =>
                x.DisplayName.ToLower().Contains(search) ||
                x.SamAccountName.ToLower().Contains(search)
            );
        }
    }
}
