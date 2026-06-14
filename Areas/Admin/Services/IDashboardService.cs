using Project_CNPM.Area.Admin.DTOs;

namespace Project_CNPM.Area.Admin.Services
{
    public interface IDashboardAdminService
    {
        Task<SystemStatisticsDto> GetSystemStatisticsAsync();
    }
}