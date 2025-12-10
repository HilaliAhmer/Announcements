using MCC.Korsini.Announcements.Business.Abstract;
using MCC.Korsini.Announcements.DataAccess.Abstract;
using MCC.Korsini.Announcements.Entities.Concrete;
using Microsoft.Extensions.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.Versioning;

namespace MCC.Korsini.Announcements.Business.Concrete
{
    [SupportedOSPlatform("windows")]
    public class ActiveDirectorySyncManager : IActiveDirectorySyncService
    {
        private readonly INotificationCenter_DirectoryEmployee_Table_Dal _employeeDal;
        private readonly IConfiguration _config;

        public ActiveDirectorySyncManager(
            INotificationCenter_DirectoryEmployee_Table_Dal employeeDal,
            IConfiguration config)
        {
            _employeeDal = employeeDal;
            _config = config;
        }

        public async Task<int> SyncFromOUAsync()
        {
            // appsettings.json'daki Server + Port + BaseDN ile tam path
            string server = _config["LdapSettings:Server"];
            string port = _config["LdapSettings:Port"] ?? "389";
            string baseDn = _config["LdapSettings:BaseDN"];

            string ldapPath = $"LDAP://{server}:{port}/{baseDn}";
            string username = _config["LdapSettings:UserName"];
            string password = _config["LdapSettings:Password"];

            using var entry = new DirectoryEntry(ldapPath, username, password);
            using var searcher = new DirectorySearcher(entry)
            {
                // disabled olmayan user’lar
                Filter = "(&(objectCategory=person)(objectClass=user)(!(userAccountControl:1.2.840.113556.1.4.803:=2)))"
            };

            searcher.PropertiesToLoad.Add("samaccountname");
            searcher.PropertiesToLoad.Add("userprincipalname");
            searcher.PropertiesToLoad.Add("displayname");
            searcher.PropertiesToLoad.Add("title");
            searcher.PropertiesToLoad.Add("department");
            searcher.PropertiesToLoad.Add("mail");
            searcher.PropertiesToLoad.Add("manager");
            searcher.PropertiesToLoad.Add("distinguishedName");
            searcher.PropertiesToLoad.Add("useraccountcontrol");

            var results = searcher.FindAll();

            // DB'deki mevcut kayıtlar
            var allEmployees = await _employeeDal.GetListAsync();
            bool isEmpty = !allEmployees.Any();

            int createdCount = 0;

            if (isEmpty)
            {
                // ===========================
                // 1) İLK SYNC (TABLO BOŞ)
                // ===========================
                var relations = new List<(string EmployeeDn, string ManagerDn)>();

                foreach (SearchResult result in results)
                {
                    string employeeDn = GetProp(result, "distinguishedName");
                    if (string.IsNullOrWhiteSpace(employeeDn))
                        continue;

                    string managerDn = GetProp(result, "manager");
                    string sam = GetProp(result, "samaccountname");
                    string upn = GetProp(result, "userprincipalname");
                    string displayName = GetProp(result, "displayname");
                    string title = GetProp(result, "title");
                    string department = GetProp(result, "department");
                    string email = GetProp(result, "mail");

                    var emp = new NotificationCenter_DirectoryEmployee_Table
                    {
                        DistinguishedName = employeeDn,
                        SamAccountName = sam,
                        UserPrincipalName = upn,
                        DisplayName = displayName,
                        Title = title,
                        Department = department,
                        Email = email,
                        IsActive = true,
                        LastSyncedAt = DateTime.UtcNow,
                        ManagerId = null // birazdan dolduracağız
                    };

                    await _employeeDal.AddAsync(emp);
                    createdCount++;

                    relations.Add((employeeDn, managerDn));
                }

                // Insert sonrası son durumları tekrar çek
                allEmployees = await _employeeDal.GetListAsync();

                // DN → Employee map'i
                var employeesByDn = new Dictionary<string, NotificationCenter_DirectoryEmployee_Table>(
                    StringComparer.OrdinalIgnoreCase
                );

                foreach (var emp in allEmployees.Where(e => !string.IsNullOrWhiteSpace(e.DistinguishedName)))
                {
                    if (!employeesByDn.ContainsKey(emp.DistinguishedName))
                        employeesByDn[emp.DistinguishedName] = emp;
                }

                // ManagerId’leri doldur
                foreach (var rel in relations)
                {
                    if (!employeesByDn.TryGetValue(rel.EmployeeDn, out var emp))
                        continue;

                    if (string.IsNullOrWhiteSpace(rel.ManagerDn))
                        continue;

                    if (!employeesByDn.TryGetValue(rel.ManagerDn, out var mgr))
                        continue;

                    if (emp.ManagerId != mgr.Id)
                    {
                        emp.ManagerId = mgr.Id;
                        await _employeeDal.UpdateAsync(emp);
                    }
                }
            }
            else
            {
                // ===========================
                // 2) SONRAKİ SYNC'LER
                //    (DB doluyken update)
                // ===========================
                var employeesByDn = new Dictionary<string, NotificationCenter_DirectoryEmployee_Table>(
                    StringComparer.OrdinalIgnoreCase
                );

                foreach (var emp in allEmployees.Where(e => !string.IsNullOrWhiteSpace(e.DistinguishedName)))
                {
                    if (!employeesByDn.ContainsKey(emp.DistinguishedName))
                        employeesByDn[emp.DistinguishedName] = emp;
                }

                var relations = new List<(NotificationCenter_DirectoryEmployee_Table Employee, string ManagerDn)>();

                foreach (SearchResult result in results)
                {
                    string employeeDn = GetProp(result, "distinguishedName");
                    if (string.IsNullOrWhiteSpace(employeeDn))
                        continue;

                    if (!employeesByDn.TryGetValue(employeeDn, out var emp))
                        continue; // yeni user için insert istersen buraya ekleyebiliriz

                    string managerDn = GetProp(result, "manager");
                    string sam = GetProp(result, "samaccountname");
                    string upn = GetProp(result, "userprincipalname");
                    string displayName = GetProp(result, "displayname");
                    string title = GetProp(result, "title");
                    string department = GetProp(result, "department");
                    string email = GetProp(result, "mail");

                    emp.SamAccountName = sam;
                    emp.UserPrincipalName = upn;
                    emp.DisplayName = displayName;
                    emp.Title = title;
                    emp.Department = department;
                    emp.Email = email;
                    emp.IsActive = true;
                    emp.LastSyncedAt = DateTime.UtcNow;

                    await _employeeDal.UpdateAsync(emp);

                    relations.Add((emp, managerDn));
                }

                // ManagerId güncelle
                foreach (var rel in relations)
                {
                    var emp = rel.Employee;

                    if (string.IsNullOrWhiteSpace(rel.ManagerDn))
                    {
                        if (emp.ManagerId != null)
                        {
                            emp.ManagerId = null;
                            await _employeeDal.UpdateAsync(emp);
                        }
                        continue;
                    }

                    if (!employeesByDn.TryGetValue(rel.ManagerDn, out var mgr))
                        continue;

                    if (emp.ManagerId != mgr.Id)
                    {
                        emp.ManagerId = mgr.Id;
                        await _employeeDal.UpdateAsync(emp);
                    }
                }

                createdCount = 0;
            }

            return createdCount;
        }




        private string GetProp(SearchResult result, string key)
        {
            if (!result.Properties.Contains(key))
                return "";

            var prop = result.Properties[key];
            if (prop == null || prop.Count == 0 || prop[0] == null)
                return "";

            return prop[0]!.ToString()!;
        }
    }
}
