using Project_CNPM.Area.Admin.DTOs;

namespace Project_CNPM.Area.Admin.Services
{
    public interface IMedicineAdminService
    {
        Task<IEnumerable<MedicineDetailDto>> GetAllMedicinesAsync(bool includeInactive = true);
        Task<MedicineDetailDto?> GetMedicineByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> CreateMedicineAsync(MedicineCreateDto dto);
        Task<(bool IsSuccess, string Message)> UpdateMedicineAsync(MedicineUpdateDto dto);
        Task<(bool IsSuccess, string Message)> ToggleMedicineStatusAsync(int id, bool isActive);
    }
}
