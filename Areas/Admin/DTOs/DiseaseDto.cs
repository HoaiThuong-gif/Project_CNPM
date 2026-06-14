namespace Project_CNPM.Area.Admin.DTOs
{
    public class DiseaseCreateDto
    {
        public required string DiseaseName { get; set; }
        public  string Description { get; set; } = string.Empty;
        public required string DiseaseGroup { get; set; }
        public int SeverityLevel { get; set; } // 1: Nhẹ, 2: Trung bình, 3: Nặng
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
    }
}