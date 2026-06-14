using Project_CNPM.Area.Admin.DTOs;

namespace Project_CNPM.Area.Admin.Services
{
    public interface IDiseaseAdminService
    {
        Task<IEnumerable<DiseaseDetailDto>> GetAllDiseasesAsync(bool includeDeleted);
        Task<DiseaseDetailDto?> GetDiseaseByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> CreateDiseaseAsync(DiseaseCreateDto dto);
        Task<(bool IsSuccess, string Message)> UpdateDiseaseAsync(DiseaseUpdateDto dto);
        Task<(bool IsSuccess, string Message)> SoftDeleteDiseaseAsync(int id);
    }
}