using Microsoft.AspNetCore.Mvc;
using MCC.Korsini.Announcements.Business.Abstract;


namespace MCC.Korsini.Announcements.WebUI.Controllers
{
    public class OrgChartController : Controller
    {
        private readonly INotificationCenter_DirectoryEmployee_Table_Service _employeeService;
        private readonly IActiveDirectorySyncService _adSyncService;
        private readonly INotificationCenter_DirectoryEmployee_OrgChartOverride_Table_Service _orgOverrideService;

        public OrgChartController(
            INotificationCenter_DirectoryEmployee_Table_Service employeeService,
            IActiveDirectorySyncService adSyncService,
            INotificationCenter_DirectoryEmployee_OrgChartOverride_Table_Service orgOverrideService)
        {
            _employeeService = employeeService;
            _adSyncService = adSyncService;
            _orgOverrideService = orgOverrideService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllAsync();
            var overrides = await _orgOverrideService.GetAllAsync();

            // Çalışanları Sam’e göre map’leyelim
            var empBySam = employees
                .Where(e => !string.IsNullOrWhiteSpace(e.SamAccountName))
                .ToDictionary(
                    e => e.SamAccountName.Trim().ToLower(),
                    e => e
                );

            // 1) Override tablosunu uygula
            foreach (var ov in overrides)
            {
                if (string.IsNullOrWhiteSpace(ov.EmployeeSam))
                    continue;

                var empSam = ov.EmployeeSam.Trim().ToLower();
                if (!empBySam.TryGetValue(empSam, out var emp))
                    continue;

                // Manager override: DisplayManagerSam doluysa
                if (!string.IsNullOrWhiteSpace(ov.DisplayManagerSam))
                {
                    var mgrSam = ov.DisplayManagerSam.Trim().ToLower();
                    if (empBySam.TryGetValue(mgrSam, out var mgr))
                    {
                        // AD’de ne yazıyorsa yazsın, şemada bu kişiye bağlı gözükecek
                        emp.ManagerId = mgr.Id;
                    }
                }

                // İleride istersen IsLocalTopManager / SortOrder’ı DTO’ya da ekleyebiliriz
                // şimdilik Management Team görünümü zaten ManagerId üzerinden çalışıyor.
            }

            // 2) Frontend’e gidecek DTO
            var result = employees.Select(e => new
            {
                id = e.Id,
                managerId = e.ManagerId,
                displayName = e.DisplayName,
                title = e.Title,
                department = e.Department,
                samAccountName = e.SamAccountName
            }).ToList();

            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(new { results = new List<object>() });

            var results = await _employeeService.SearchByNameAsync(q);

            return Json(new
            {
                results = results
                    .Where(x => x.IsActive)
                    .Select(x => new
                    {
                        id = x.Id,
                        name = x.DisplayName,
                        title = x.Title
                    })
            });
        }

        [HttpPost]
        public async Task<IActionResult> SyncAD()
        {
            try
            {
                var addedCount = await _adSyncService.SyncFromOUAsync();

                return Json(new
                {
                    success = true,
                    added = addedCount
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message,
                    detail = ex.InnerException?.Message ?? ex.ToString()
                });
            }
        }
    }
}
