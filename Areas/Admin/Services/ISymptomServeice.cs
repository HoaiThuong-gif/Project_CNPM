using Project_CNPM.Area.Admin.DTOs;

namespace Project_CNPM.Area.Admin.Services
{
    public interface ISymptomAdminService
    {
        Task<IEnumerable<SymptomDetailDto>> GetAllSymptomsAsync();
        Task<SymptomDetailDto?> GetSymptomByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> CreateSymptomAsync(SymptomCreateUpdateDto dto);
        Task<(bool IsSuccess, string Message)> UpdateSymptomAsync(SymptomCreateUpdateDto dto);
        Task<(bool IsSuccess, string Message)> ToggleSymptomAsync(int id);
    }
}