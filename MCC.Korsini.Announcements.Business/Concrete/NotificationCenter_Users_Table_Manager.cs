using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    public class NotificationCenter_Users_Table_Manager: INotificationCenter_Users_Table_Service
    {
        private readonly INotificationCenter_Users_Table_Dal _users;

        public NotificationCenter_Users_Table_Manager(INotificationCenter_Users_Table_Dal users)
        {
            _users = users;
        }

        public async Task<NotificationCenter_Users_Table> GetUserByUsernameAsync(string email)
        {
            var allUsers= await _users.GetListAsync();
            return allUsers.FirstOrDefault(u => u.UserName.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task AddUserAsync(NotificationCenter_Users_Table user)
        {
            await _users.AddAsync(user);
        }

        public async Task UpdateUserAsync(NotificationCenter_Users_Table user)
        {
            await _users.UpdateAsync(user);
        }
    }
}
