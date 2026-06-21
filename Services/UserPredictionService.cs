using Microsoft.EntityFrameworkCore;
using Project_CNPM.Data;
using Project_CNPM.DTOs;
using Project_CNPM.Models;
using System.Text;
using System.Text.Json;

namespace Project_CNPM.Services
{
    public class UserPredictionService : IUserPredictionService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public UserPredictionService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5000");
        }

        public async Task<(bool IsSuccess, string Message, List<PredictResultDto>? Results)> PredictDrugsAsync(int userId, PredictRequestDto request)
        {
            var benh = await _context.Benhs.FirstOrDefaultAsync(b => b.MaBenh == request.DiseaseId && b.DangHoatDong == true && b.DeleteAt == null);
            if (benh == null) return (false, "Bệnh không hợp lệ hoặc đã bị vô hiệu hóa.", null);

            if (string.IsNullOrWhiteSpace(request.Symptoms)) 
                return (false, "Vui lòng nhập mô tả triệu chứng của bạn.", null);

            //Gọi sang API Python
            var payload = new
            {
                ma_benh = benh.MaBenh.ToString(),
                ten_benh = benh.TenBenh,
                trieu_chung = new List<string> { request.Symptoms.Trim() }, 
                top_k = 5
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/predict", jsonContent);

            if (!response.IsSuccessStatusCode)
                return (false, "Lỗi từ AI Server.", null);

            var resultString = await response.Content.ReadAsStringAsync();
            var pythonData = JsonSerializer.Deserialize<PythonPredictResponse>(resultString);

            if (pythonData == null || !pythonData.Results.Any())
                return (false, "Hệ thống không tìm thấy thuốc phù hợp.", null);

            var lichSu = new LichSuDuDoan
            {
                MaNguoiDung = userId,
                MaBenh = benh.MaBenh,
                NgayTao = DateTime.Now,
                GhiChu = request.Symptoms.Trim(),
                
                // Lưu kết quả dự đoán (Snapshot)
                KetQuaDuDoans = pythonData.Results.Select(r => new KetQuaDuDoan
                {
                    MaThuoc = int.Parse(r.MaThuoc),
                    Diem = r.Diem,
                    LyDo = r.LyDo,
                    TenThuocSnapshot = r.TenThuoc,
                    LieuDungSnapshot = r.LieuDung
                }).ToList()
            };

            _context.LichSuDuDoans.Add(lichSu);
            await _context.SaveChangesAsync();

            var finalResults = lichSu.KetQuaDuDoans.Select(kq => new PredictResultDto
            {
                ResultId = kq.MaKetQua, 
                MedicineId = kq.MaThuoc ?? 0,
                MedicineName = kq.TenThuocSnapshot ?? "",
                Dosage = kq.LieuDungSnapshot ?? "",
                Score = kq.Diem ?? 0,
                Reason = kq.LyDo ?? ""
            }).ToList();

            return (true, "Dự đoán thành công", finalResults);
        }

        public async Task<(bool IsSuccess, string Message)> SubmitFeedbackAsync(int userId, FeedbackRequestDto request)
        {
            var isValidResult = await _context.KetQuaDuDoans
                .Include(k => k.MaLichSuNavigation)
                .AnyAsync(k => k.MaKetQua == request.ResultId && k.MaLichSuNavigation.MaNguoiDung == userId);

            if (!isValidResult) return (false, "Kết quả dự đoán không hợp lệ hoặc không thuộc về bạn.");

            var exists = await _context.DanhGiaDuDoans.AnyAsync(d => d.MaKetQua == request.ResultId && d.MaNguoiDung == userId);
            if (exists) return (false, "Bạn đã gửi đánh giá cho kết quả này rồi.");

            var danhGia = new DanhGiaDuDoan
            {
                MaKetQua = request.ResultId,
                MaNguoiDung = userId,
                HuuIch = request.IsHelpful,
                GhiChu = request.Note,
                NgayTao = DateTime.Now
            };

            _context.DanhGiaDuDoans.Add(danhGia);
            await _context.SaveChangesAsync();

            return (true, "Cảm ơn bạn đã gửi đánh giá!");
        }
    }
}