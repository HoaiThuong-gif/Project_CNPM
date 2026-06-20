using Project_CNPM.Area.Admin.DTOs;

namespace Project_CNPM.Area.Admin.Services
{
    public interface IMedicineDiseaseMappingService
    {
        Task<(bool IsSuccess, string Message)> LinkMedicineWithDiseaseAsync(int medicineId, int diseaseId, int priority, string treatmentType);
        Task<(bool IsSuccess, string Message)> UnlinkMedicineFromDiseaseAsync(int medicineId, int diseaseId);
        Task<IEnumerable<DiseaseLinkedWithMedicineDto>> GetDiseasesOfMedicineAsync(int medicineId);
        Task<IEnumerable<MedicineLinkedWithDiseaseDto>> GetMedicinesOfDiseaseAsync(int diseaseId);
    }
}