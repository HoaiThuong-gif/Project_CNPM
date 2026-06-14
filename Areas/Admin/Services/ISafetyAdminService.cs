using Project_CNPM.Area.Admin.DTOs;

namespace Project_CNPM.Area.Admin.Services
{
    public interface ISafetyWarningAdminService
    {
        Task<IEnumerable<AllergyCreateUpdateDto>> GetAllAllergiesAsync(bool includeInactive = true);
        Task<(bool IsSuccess, string Message)> CreateAllergyAsync(AllergyCreateUpdateDto dto);
        Task<(bool IsSuccess, string Message)> UpdateAllergyAsync(AllergyCreateUpdateDto dto);
        Task<(bool IsSuccess, string Message)> ToggleAllergyStatusAsync(int id, bool isActive);

        Task<IEnumerable<BackgroundDiseaseCreateUpdateDto>> GetAllBackgroundDiseasesAsync(bool includeInactive = true);
        Task<(bool IsSuccess, string Message)> CreateBackgroundDiseaseAsync(BackgroundDiseaseCreateUpdateDto dto);
        Task<(bool IsSuccess, string Message)> UpdateBackgroundDiseaseAsync(BackgroundDiseaseCreateUpdateDto dto);
        Task<(bool IsSuccess, string Message)> ToggleBackgroundDiseaseStatusAsync(int id, bool isActive);

        Task<IEnumerable<DrugInteractionDetailDto>> GetAllDrugInteractionsAsync();
        Task<(bool IsSuccess, string Message)> CreateDrugInteractionAsync(DrugInteractionCreateDto dto);
        Task<(bool IsSuccess, string Message)> DeleteDrugInteractionAsync(int medicine1Id, int medicine2Id);
    }
}
