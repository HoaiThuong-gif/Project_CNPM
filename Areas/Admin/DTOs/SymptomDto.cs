namespace Project_CNPM.Area.Admin.DTOs
{
    public class SymptomCreateUpdateDto
    {
        public int SymptomId { get; set; }
        public string SymptomName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class SymptomDetailDto : SymptomCreateUpdateDto
    {
        public DateTime CreatedAt { get; set; }
    }
}