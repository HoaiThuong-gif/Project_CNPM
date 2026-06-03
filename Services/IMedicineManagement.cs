using Projcet_CNPM.DTOs.Medicin;

namespace Project_CNPM.Services
{
    public interface IMedicineManagement
    {
        Task<(bool IsSuccess, string Message)> AddMedicineAsync(MedicinDTO medicineDto);
        Task<(bool IsSuccess, string Message)> UpdateMedicineAsync(int id, MedicinDTO medicineDto);
        Task<(bool IsSuccess, string Message)> DeleteMedicineAsync(int id);
        Task<List<MedicinDTO>> GetAllMedicinesAsync();
        Task<MedicinDTO?> GetMedicineByIdAsync(int id);
    }
}