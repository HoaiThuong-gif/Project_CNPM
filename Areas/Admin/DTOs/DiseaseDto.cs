namespace Project_CNPM.Area.Admin.DTOs
{
    public class DiseaseCreateDto
    {
        public required string DiseaseName { get; set; }
        public string Description { get; set; } = string.Empty;
        public required string DiseaseGroup { get; set; }
        public int SeverityLevel { get; set; }
        public List<int> SymptomIds { get; set; } = new();
    }

    public class DiseaseUpdateDto : DiseaseCreateDto
    {
        public int DiseaseId { get; set; }
        public bool IsActive { get; set; }
    }

    public class DiseaseDetailDto : DiseaseUpdateDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<SymptomDetailDto> Symptoms { get; set; } = new();
    }
}
