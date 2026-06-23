namespace Project_CNPM.Area.Admin.DTOs
{
    public class SymptomCreateUpdateDto
    {
        public int SymptomId { get; set; }
        public string SymptomName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<int> DiseaseIds { get; set; } = new();
    }

    public class SymptomDetailDto : SymptomCreateUpdateDto
    {
        public DateTime CreatedAt { get; set; }
        public List<SymptomDiseaseLinkDto> Diseases { get; set; } = new();
    }

    public class SymptomDiseaseLinkDto
    {
        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; } = string.Empty;
    }
}
