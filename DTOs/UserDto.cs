using System.Text.Json.Serialization;

namespace Project_CNPM.DTOs
{
    // 1. DTO Nhận yêu cầu dự đoán từ Frontend
    public class PredictRequestDto
    {
        public int DiseaseId { get; set; }
       public string Symptoms { get; set; } = string.Empty;
    }

    // 2. DTO Trả kết quả dự đoán cho Frontend
    public class PredictResultDto
    {
        public int ResultId { get; set; } // Trả về MaKetQua để user có thể đánh giá sau này
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public double Score { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    // 3. DTO Nhận đánh giá từ Frontend
    public class FeedbackRequestDto
    {
        public int ResultId { get; set; } // MaKetQua
        public bool IsHelpful { get; set; } // HuuIch
        public string? Note { get; set; } // GhiChu
    }

    // --- Các class nội bộ dùng để đọc JSON từ Python ---
    public class PythonPredictResponse
    {
        [JsonPropertyName("results")]
        public List<PythonDrugResult> Results { get; set; } = new();
    }

    public class PythonDrugResult
    {
        [JsonPropertyName("ma_thuoc")]
        public string MaThuoc { get; set; } = string.Empty;
        [JsonPropertyName("ten_thuoc")]
        public string TenThuoc { get; set; } = string.Empty;
        [JsonPropertyName("lieu_dung")]
        public string LieuDung { get; set; } = string.Empty;
        [JsonPropertyName("diem")]
        public double Diem { get; set; }
        [JsonPropertyName("ly_do")]
        public string LyDo { get; set; } = string.Empty;
    }
}