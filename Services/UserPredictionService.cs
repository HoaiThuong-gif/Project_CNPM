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

            var predictedMedicineIds = pythonData.Results.Select(r => int.Parse(r.MaThuoc)).ToList();

            var allergyWarnings = await _context.CanhBaoDiUngThuocs
                .Include(c => c.MaDiUngNavigation)
                .Where(c => predictedMedicineIds.Contains(c.MaThuoc))
                .ToListAsync();

            var diseaseWarnings = await _context.CanhBaoBenhNenThuocs
                .Include(c => c.MaBenhNenNavigation)
                .Where(c => predictedMedicineIds.Contains(c.MaThuoc))
                .ToListAsync();

            var interactions = await _context.TuongTacThuocs
                .Include(t => t.MaThuoc1Navigation)
                .Include(t => t.MaThuoc2Navigation)
                .Where(t => predictedMedicineIds.Contains(t.MaThuoc1) || predictedMedicineIds.Contains(t.MaThuoc2))
                .ToListAsync();
            
            var lichSu = new LichSuDuDoan
            {
                MaNguoiDung = userId,
                MaBenh = benh.MaBenh,
                NgayTao = DateTime.Now,
                GhiChu = request.Symptoms.Trim(),
                KetQuaDuDoans = new List<KetQuaDuDoan>()
            };

            var finalResults = new List<PredictResultDto>();

            // Lặp qua từng thuốc AI gợi ý để lọc ra cảnh báo tương ứng
            foreach (var r in pythonData.Results)
            {
                int maThuoc = int.Parse(r.MaThuoc);

                // 1. Lọc cảnh báo riêng cho thuốc đang xét
                var thuocAllergies = allergyWarnings.Where(a => a.MaThuoc == maThuoc)
                    .Select(a => $"Dị ứng {a.MaDiUngNavigation.TenDiUng}: {a.NoiDung}").ToList();

                var thuocDiseases = diseaseWarnings.Where(d => d.MaThuoc == maThuoc)
                    .Select(d => $"Bệnh nền {d.MaBenhNenNavigation.TenBenhNen}: {d.NoiDung}").ToList();

                // Tương tác thuốc phải tra cả cột Thuốc 1 và Thuốc 2
                var thuocInteractions = interactions.Where(i => i.MaThuoc1 == maThuoc)
                    .Select(i => $"Tránh dùng chung với {i.MaThuoc2Navigation.TenThuoc}: {i.MoTa}")
                    .Concat(interactions.Where(i => i.MaThuoc2 == maThuoc)
                    .Select(i => $"Tránh dùng chung với {i.MaThuoc1Navigation.TenThuoc}: {i.MoTa}")).ToList();

                //Gộp tất cả để lưu vào 1 dòng Text trong SQL
                var allWarningsForDb = new List<string>();
                allWarningsForDb.AddRange(thuocAllergies);
                allWarningsForDb.AddRange(thuocDiseases);
                allWarningsForDb.AddRange(thuocInteractions);
                
                string dbCanhBaoString = allWarningsForDb.Any() ? string.Join(" | ", allWarningsForDb) : null;

                // 3. Gắn vào Entity để EF Core lưu
                var ketQua = new KetQuaDuDoan
                {
                    MaThuoc = maThuoc,
                    Diem = r.Diem,
                    LyDo = r.LyDo,
                    TenThuocSnapshot = r.TenThuoc,
                    LieuDungSnapshot = r.LieuDung,
                    CanhBao = dbCanhBaoString
                };
                lichSu.KetQuaDuDoans.Add(ketQua);

                finalResults.Add(new PredictResultDto
                {
                    MedicineId = maThuoc,
                    MedicineName = r.TenThuoc,
                    Dosage = r.LieuDung,
                    Score = r.Diem,
                    Reason = r.LyDo,
                    AllergyWarnings = thuocAllergies,      
                    DiseaseWarnings = thuocDiseases,        
                    DrugInteractions = thuocInteractions    
                });
            }

            _context.LichSuDuDoans.Add(lichSu);
            await _context.SaveChangesAsync();

            // Gán lại MaKetQua (vừa được SQL sinh ra) cho DTO để Frontend làm chức năng Feedback
            for (int i = 0; i < finalResults.Count; i++)
            {
                finalResults[i].ResultId = lichSu.KetQuaDuDoans.ElementAt(i).MaKetQua;
            }

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

        public async Task<(bool IsSuccess, string Message, object? Data)> GetUserHistoryAsync(int userId)
        {
            var history = await _context.LichSuDuDoans
                .Include(h => h.MaBenhNavigation)
                .Include(h => h.KetQuaDuDoans)
                .Where(h => h.MaNguoiDung == userId)
                .OrderByDescending(h => h.NgayTao)
                .Select(h => new
                {
                    HistoryId = h.MaLichSu,
                    DiseaseName = h.MaBenhNavigation.TenBenh,
                    Symptoms = h.GhiChu,
                    CreatedAt = h.NgayTao,
                    Medicines = h.KetQuaDuDoans.Select(k => k.TenThuocSnapshot).ToList()
                })
                .ToListAsync();

            return (true, "Lấy lịch sử thành công", history);
        }

        public async Task<(bool IsSuccess, string Message)> DeleteHistoryAsync(int userId, int historyId)
        {
            var history = await _context.LichSuDuDoans
                .Include(h => h.KetQuaDuDoans)
                .FirstOrDefaultAsync(h => h.MaLichSu == historyId && h.MaNguoiDung == userId);

            if (history == null) return (false, "Không tìm thấy lịch sử này hoặc bạn không có quyền xóa.");

            _context.KetQuaDuDoans.RemoveRange(history.KetQuaDuDoans);
            _context.LichSuDuDoans.Remove(history);
            
            await _context.SaveChangesAsync();
            return (true, "Xóa lịch sử thành công.");
        }
    }
}