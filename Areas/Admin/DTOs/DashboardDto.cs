namespace Project_CNPM.Area.Admin.DTOs
{
    public class SystemStatisticsDto
    {
        public int TotalActiveUsers { get; set; }
        public int TotalActiveMedicines { get; set; }
        public int TotalActiveDiseases { get; set; }
        public int TotalPredictionsMade { get; set; }
        public List<TopSearchedMedicineDto> TopSearchedMedicines { get; set; } = new();
    }

    public class TopSearchedMedicineDto
    {
        public string MedicineName { get; set; } = string.Empty;
        public int SearchCount { get; set; }
    }
}
