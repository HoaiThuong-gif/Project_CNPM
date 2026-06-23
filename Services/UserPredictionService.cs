using Microsoft.EntityFrameworkCore;
using Project_CNPM.Data;
using Project_CNPM.DTOs;
using Project_CNPM.Models;
using System.Globalization;
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
            var disease = await _context.Benhs.FirstOrDefaultAsync(b =>
                b.MaBenh == request.DiseaseId &&
                b.DangHoatDong == true &&
                b.DeleteAt == null);

            if (disease == null)
                return (false, "Benh khong hop le hoac da bi tat hien thi.", null);

            if (string.IsNullOrWhiteSpace(request.Symptoms))
                return (false, "Vui long nhap trieu chung.", null);

            if (string.IsNullOrWhiteSpace(request.Severity))
                return (false, "Vui long chon muc do trieu chung.", null);

            if (request.Age is < 0 or > 120)
                return (false, "Tuoi khong hop le.", null);

            var allergies = NormalizeInputList(request.Allergies);
            var backgroundDiseases = NormalizeInputList(request.BackgroundDiseases);
            var currentMedicines = NormalizeInputList(request.CurrentMedicines);
            var symptomPayload = BuildSymptomPayload(request, allergies, backgroundDiseases, currentMedicines);

            var payload = new
            {
                ma_benh = disease.MaBenh.ToString(),
                ten_benh = disease.TenBenh,
                trieu_chung = new List<string> { symptomPayload },
                top_k = 5
            };

            HttpResponseMessage response;
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync("/predict", jsonContent);
            }
            catch
            {
                return (false, "Khong ket noi duoc AI Server. Hay chay Python backend truoc.", null);
            }

            if (!response.IsSuccessStatusCode)
                return (false, "AI Server xu ly that bai.", null);

            var resultString = await response.Content.ReadAsStringAsync();
            var pythonData = JsonSerializer.Deserialize<PythonPredictResponse>(resultString);

            if (pythonData == null || !pythonData.Results.Any())
                return (false, "He thong khong tim thay thuoc phu hop voi du lieu hien co.", null);

            var predictedMedicineIds = pythonData.Results
                .Select(r => int.TryParse(r.MaThuoc, out var id) ? id : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (!predictedMedicineIds.Any())
                return (false, "Du lieu goi y tu AI Server khong hop le.", null);

            var activeMedicineIds = await _context.Thuocs
                .Where(t => predictedMedicineIds.Contains(t.MaThuoc) && t.DangHoatDong == true)
                .Select(t => t.MaThuoc)
                .ToListAsync();

            if (!activeMedicineIds.Any())
                return (false, "Khong co thuoc dang hien thi phu hop voi ket qua goi y.", null);

            var medicinesById = await _context.Thuocs
                .Where(t => activeMedicineIds.Contains(t.MaThuoc))
                .ToDictionaryAsync(t => t.MaThuoc);

            var allergyWarnings = await _context.CanhBaoDiUngThuocs
                .Include(c => c.MaDiUngNavigation)
                .Where(c => activeMedicineIds.Contains(c.MaThuoc) && c.MaDiUngNavigation.DangHoatDong == true)
                .ToListAsync();

            var diseaseWarnings = await _context.CanhBaoBenhNenThuocs
                .Include(c => c.MaBenhNenNavigation)
                .Where(c => activeMedicineIds.Contains(c.MaThuoc) && c.MaBenhNenNavigation.DangHoatDong == true)
                .ToListAsync();

            var currentMedicineIds = new List<int>();
            if (currentMedicines.Any())
            {
                var activeMedicines = await _context.Thuocs
                    .Where(t => t.DangHoatDong == true)
                    .Select(t => new { t.MaThuoc, t.TenThuoc, t.HoatChat })
                    .ToListAsync();

                currentMedicineIds = activeMedicines
                    .Where(t => currentMedicines.Any(input =>
                        t.TenThuoc.Contains(input, StringComparison.OrdinalIgnoreCase) ||
                        (!string.IsNullOrWhiteSpace(t.HoatChat) && t.HoatChat.Contains(input, StringComparison.OrdinalIgnoreCase))))
                    .Select(t => t.MaThuoc)
                    .Distinct()
                    .ToList();
            }

            var interactions = currentMedicineIds.Any()
                ? await _context.TuongTacThuocs
                    .Include(t => t.MaThuoc1Navigation)
                    .Include(t => t.MaThuoc2Navigation)
                    .Where(t =>
                        (activeMedicineIds.Contains(t.MaThuoc1) && currentMedicineIds.Contains(t.MaThuoc2)) ||
                        (activeMedicineIds.Contains(t.MaThuoc2) && currentMedicineIds.Contains(t.MaThuoc1)))
                    .ToListAsync()
                : new List<TuongTacThuoc>();

            var history = new LichSuDuDoan
            {
                MaNguoiDung = userId,
                MaBenh = disease.MaBenh,
                NgayTao = DateTime.Now,
                GhiChu = symptomPayload,
                KetQuaDuDoans = new List<KetQuaDuDoan>()
            };

            var finalResults = new List<PredictResultDto>();

            foreach (var result in pythonData.Results)
            {
                if (!int.TryParse(result.MaThuoc, out var medicineId) || !activeMedicineIds.Contains(medicineId))
                    continue;

                medicinesById.TryGetValue(medicineId, out var medicine);

                var medicineAllergies = allergyWarnings
                    .Where(a => a.MaThuoc == medicineId)
                    .Where(a => MatchesAny(a.MaDiUngNavigation.TenDiUng, allergies))
                    .Select(a => $"Dị ứng {a.MaDiUngNavigation.TenDiUng}: {a.NoiDung}")
                    .ToList();

                var allMedicineAllergyWarnings = allergyWarnings
                    .Where(a => a.MaThuoc == medicineId)
                    .Select(a => $"Dị ứng {a.MaDiUngNavigation.TenDiUng}: {a.NoiDung}")
                    .ToList();

                var medicineDiseases = diseaseWarnings
                    .Where(d => d.MaThuoc == medicineId)
                    .Where(d => MatchesAny(d.MaBenhNenNavigation.TenBenhNen, backgroundDiseases))
                    .Select(d => $"Bệnh nền {d.MaBenhNenNavigation.TenBenhNen}: {d.NoiDung}")
                    .ToList();

                var allMedicineDiseaseWarnings = diseaseWarnings
                    .Where(d => d.MaThuoc == medicineId)
                    .Select(d => $"Bệnh nền {d.MaBenhNenNavigation.TenBenhNen}: {d.NoiDung}")
                    .ToList();

                var medicineInteractions = interactions
                    .Where(i => i.MaThuoc1 == medicineId)
                    .Select(i => $"Cảnh báo dùng chung với {i.MaThuoc2Navigation.TenThuoc}: {i.MoTa}")
                    .Concat(interactions
                        .Where(i => i.MaThuoc2 == medicineId)
                        .Select(i => $"Cảnh báo dùng chung với {i.MaThuoc1Navigation.TenThuoc}: {i.MoTa}"))
                    .ToList();

                var allWarnings = medicineAllergies
                    .Concat(medicineDiseases)
                    .Concat(medicineInteractions)
                    .ToList();

                var contraindications = allMedicineAllergyWarnings
                    .Concat(allMedicineDiseaseWarnings)
                    .ToList();

                var prediction = new KetQuaDuDoan
                {
                    MaThuoc = medicineId,
                    Diem = result.Diem,
                    LyDo = result.LyDo,
                    TenThuocSnapshot = result.TenThuoc,
                    LieuDungSnapshot = result.LieuDung,
                    CanhBao = allWarnings.Any() ? string.Join(" | ", allWarnings) : null
                };

                history.KetQuaDuDoans.Add(prediction);

                finalResults.Add(new PredictResultDto
                {
                    MedicineId = medicineId,
                    MedicineName = medicine?.TenThuoc ?? result.TenThuoc,
                    Uses = medicine?.CongDung ?? string.Empty,
                    Dosage = medicine?.LieuDung ?? result.LieuDung,
                    HowToUse = medicine?.CachDung ?? string.Empty,
                    SideEffects = medicine?.TacDungPhu ?? string.Empty,
                    Notes = medicine?.LuuY ?? string.Empty,
                    Contraindications = contraindications.Any() ? string.Join(" | ", contraindications) : string.Empty,
                    Score = result.Diem,
                    Reason = result.LyDo,
                    AllergyWarnings = medicineAllergies,
                    DiseaseWarnings = medicineDiseases,
                    DrugInteractions = medicineInteractions
                });
            }

            if (!finalResults.Any())
                return (false, "Khong co thuoc dang hien thi phu hop voi yeu cau.", null);

            _context.LichSuDuDoans.Add(history);
            await _context.SaveChangesAsync();

            for (var i = 0; i < finalResults.Count; i++)
            {
                finalResults[i].ResultId = history.KetQuaDuDoans.ElementAt(i).MaKetQua;
            }

            return (true, "Du doan thanh cong", finalResults);
        }

        public async Task<(bool IsSuccess, string Message)> SubmitFeedbackAsync(int userId, FeedbackRequestDto request)
        {
            var isValidResult = await _context.KetQuaDuDoans
                .Include(k => k.MaLichSuNavigation)
                .AnyAsync(k => k.MaKetQua == request.ResultId && k.MaLichSuNavigation != null && k.MaLichSuNavigation.MaNguoiDung == userId);

            if (!isValidResult)
                return (false, "Ket qua du doan khong hop le hoac khong thuoc ve ban.");

            var exists = await _context.DanhGiaDuDoans.AnyAsync(d => d.MaKetQua == request.ResultId && d.MaNguoiDung == userId);
            if (exists)
                return (false, "Ban da gui danh gia cho ket qua nay roi.");

            var feedback = new DanhGiaDuDoan
            {
                MaKetQua = request.ResultId,
                MaNguoiDung = userId,
                HuuIch = request.IsHelpful,
                GhiChu = request.Note,
                NgayTao = DateTime.Now
            };

            _context.DanhGiaDuDoans.Add(feedback);
            await _context.SaveChangesAsync();

            return (true, "Cam on ban da gui danh gia!");
        }

        public async Task<(bool IsSuccess, string Message, object? Data)> GetUserHistoryAsync(int userId)
        {
            var historyRows = await _context.LichSuDuDoans
                .Include(h => h.MaBenhNavigation)
                .Include(h => h.KetQuaDuDoans)
                    .ThenInclude(k => k.MaThuocNavigation)
                        .ThenInclude(t => t!.CanhBaoDiUngThuocs)
                            .ThenInclude(c => c.MaDiUngNavigation)
                .Include(h => h.KetQuaDuDoans)
                    .ThenInclude(k => k.MaThuocNavigation)
                        .ThenInclude(t => t!.CanhBaoBenhNenThuocs)
                            .ThenInclude(c => c.MaBenhNenNavigation)
                .Where(h => h.MaNguoiDung == userId)
                .OrderByDescending(h => h.NgayTao)
                .ToListAsync();

            var history = historyRows.Select(h => new
            {
                HistoryId = h.MaLichSu,
                DiseaseName = h.MaBenhNavigation != null ? h.MaBenhNavigation.TenBenh : "",
                Symptoms = h.GhiChu,
                CreatedAt = h.NgayTao,
                Medicines = h.KetQuaDuDoans.Select(k => k.TenThuocSnapshot).ToList(),
                Results = h.KetQuaDuDoans.Select(k => new
                {
                    ResultId = k.MaKetQua,
                    MedicineId = k.MaThuoc,
                    MedicineName = k.TenThuocSnapshot,
                    Dosage = k.LieuDungSnapshot,
                    Uses = k.MaThuocNavigation != null ? k.MaThuocNavigation.CongDung : "",
                    HowToUse = k.MaThuocNavigation != null ? k.MaThuocNavigation.CachDung : "",
                    SideEffects = k.MaThuocNavigation != null ? k.MaThuocNavigation.TacDungPhu : "",
                    Notes = k.MaThuocNavigation != null ? k.MaThuocNavigation.LuuY : "",
                    Contraindications = BuildMedicineContraindications(k.MaThuocNavigation),
                    Score = k.Diem,
                    Reason = k.LyDo,
                    Warnings = k.CanhBao
                }).ToList()
            }).ToList();

            return (true, "Lay lich su thanh cong", history);
        }

        public async Task<(bool IsSuccess, string Message)> DeleteHistoryAsync(int userId, int historyId)
        {
            var history = await _context.LichSuDuDoans
                .Include(h => h.KetQuaDuDoans)
                .ThenInclude(k => k.DanhGiaDuDoans)
                .FirstOrDefaultAsync(h => h.MaLichSu == historyId && h.MaNguoiDung == userId);

            if (history == null)
                return (false, "Khong tim thay lich su nay hoac ban khong co quyen xoa.");

            foreach (var result in history.KetQuaDuDoans)
            {
                _context.DanhGiaDuDoans.RemoveRange(result.DanhGiaDuDoans);
            }

            _context.KetQuaDuDoans.RemoveRange(history.KetQuaDuDoans);
            _context.LichSuDuDoans.Remove(history);

            await _context.SaveChangesAsync();
            return (true, "Xoa lich su thanh cong.");
        }

        private static List<string> NormalizeInputList(IEnumerable<string>? values)
        {
            return values?
                .Select(v => v?.Trim() ?? string.Empty)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList() ?? new List<string>();
        }

        private static bool MatchesAny(string? source, IEnumerable<string> inputs)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            var normalizedSource = NormalizeSearchText(source);

            return inputs.Any(input =>
            {
                var normalizedInput = NormalizeSearchText(input);
                return normalizedSource.Contains(normalizedInput, StringComparison.OrdinalIgnoreCase) ||
                       normalizedInput.Contains(normalizedSource, StringComparison.OrdinalIgnoreCase) ||
                       HasMeaningfulTokenOverlap(normalizedSource, normalizedInput);
            });
        }

        private static bool HasMeaningfulTokenOverlap(string source, string input)
        {
            var ignoredTokens = new HashSet<string> { "di", "ung", "benh", "nen", "thuoc", "va", "voi" };
            var sourceTokens = source.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(token => token.Length >= 3 && !ignoredTokens.Contains(token))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            var inputTokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(token => token.Length >= 3 && !ignoredTokens.Contains(token));

            return inputTokens.Any(sourceTokens.Contains);
        }

        private static string NormalizeSearchText(string value)
        {
            var normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var c in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(char.IsLetterOrDigit(c) ? c : ' ');
                }
            }

            return string.Join(" ", builder
                .ToString()
                .Normalize(NormalizationForm.FormC)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        private static string BuildMedicineContraindications(Thuoc? medicine)
        {
            if (medicine == null)
            {
                return string.Empty;
            }

            var warnings = medicine.CanhBaoDiUngThuocs
                .Where(x => x.MaDiUngNavigation.DangHoatDong == true)
                .Select(x => $"Dị ứng {x.MaDiUngNavigation.TenDiUng}: {x.NoiDung}")
                .Concat(medicine.CanhBaoBenhNenThuocs
                    .Where(x => x.MaBenhNenNavigation.DangHoatDong == true)
                    .Select(x => $"Bệnh nền {x.MaBenhNenNavigation.TenBenhNen}: {x.NoiDung}"))
                .ToList();

            return warnings.Any() ? string.Join(" | ", warnings) : string.Empty;
        }

        private static string BuildSymptomPayload(
            PredictRequestDto request,
            List<string> allergies,
            List<string> backgroundDiseases,
            List<string> currentMedicines)
        {
            var parts = new List<string>
            {
                $"Trieu chung chinh: {request.Symptoms.Trim()}",
                $"Muc do trieu chung: {request.Severity.Trim()}"
            };

            if (request.Age.HasValue)
                parts.Add($"Tuoi: {request.Age.Value}");

            if (!string.IsNullOrWhiteSpace(request.Gender))
                parts.Add($"Gioi tinh: {request.Gender.Trim()}");

            if (allergies.Any())
                parts.Add($"Di ung thuoc: {string.Join(", ", allergies)}");

            if (backgroundDiseases.Any())
                parts.Add($"Benh nen: {string.Join(", ", backgroundDiseases)}");

            if (currentMedicines.Any())
                parts.Add($"Thuoc dang su dung: {string.Join(", ", currentMedicines)}");

            return string.Join(". ", parts);
        }
    }
}
