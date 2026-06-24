using Project_CNPM.DTOs.Medicine;

namespace Project_CNPM.Area.Admin.Services{
    public interface IUserAdminService
    {
        Task<IEnumerable<UserAdminViewDto>> GetAllUsersAsync();
        Task<bool> ToggleUserLockStatusAsync(int userId, bool isLocked);
        Task<bool> SoftDeleteUserAsync(int userId);
    }
}