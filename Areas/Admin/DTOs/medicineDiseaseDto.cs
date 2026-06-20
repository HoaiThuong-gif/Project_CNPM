namespace Project_CNPM.Area.Admin.DTOs
{
    public class DiseaseLinkedWithMedicineDto
    {
        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string TreatmentType { get; set; } = string.Empty;
    }

    public class MedicineLinkedWithDiseaseDto
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string TreatmentType { get; set; } = string.Empty;
    }

    public class LinkMedicineDiseaseDto
    {
        public int MedicineId { get; set; }
        public int DiseaseId { get; set; }
        public int Priority { get; set; } = 3;
        public string TreatmentType { get; set; } = "unknown";
    }
}