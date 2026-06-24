using Microsoft.EntityFrameworkCore;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Data;
using Project_CNPM.Models;

namespace Project_CNPM.Area.Admin.Services
{
    public class SafetyWarningAdminService : ISafetyWarningAdminService
    {
        private readonly ApplicationDbContext _context;

        public SafetyWarningAdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AllergyCreateUpdateDto>> GetAllAllergiesAsync()
        {
            // Bỏ dòng if (!includeInactive)
            return await _context.DiUngs.Select(x => new AllergyCreateUpdateDto
            {
                AllergyId = x.MaDiUng,
                AllergyName = x.TenDiUng,
                IsActive = x.DangHoatDong ?? false
            }).ToListAsync();
        }

        public async Task<(bool IsSuccess, string Message)> CreateAllergyAsync(AllergyCreateUpdateDto dto)
        {
            var exists = await _context.DiUngs.AnyAsync(x => x.TenDiUng == dto.AllergyName);
            if (exists)
                return (false, "Allergy already exists");

            var diUng = new DiUng
            {
                TenDiUng = dto.AllergyName,
                DangHoatDong = dto.IsActive
            };

            _context.DiUngs.Add(diUng);
            await _context.SaveChangesAsync();
            return (true, "Add allergy success!");
        }

        public async Task<(bool IsSuccess, string Message)> UpdateAllergyAsync(AllergyCreateUpdateDto dto)
        {
            var diUng = await _context.DiUngs.FindAsync(dto.AllergyId);
            if (diUng == null) return (false, "Not found allergy");

            var exists = await _context.DiUngs.AnyAsync(x => x.MaDiUng != dto.AllergyId && x.TenDiUng == dto.AllergyName);
            if (exists)
                return (false, "Allergy already exists");

            diUng.TenDiUng = dto.AllergyName;
            diUng.DangHoatDong = dto.IsActive;

            await _context.SaveChangesAsync();
            return (true, "Update allergy success!");
        }

        public async Task<(bool IsSuccess, string Message)> ToggleAllergyStatusAsync(int id, bool isActive)
        {
            var diUng = await _context.DiUngs.FindAsync(id);
            if (diUng == null) return (false, "Not found allergy");

            diUng.DangHoatDong = isActive;

            await _context.SaveChangesAsync();
            return (true, isActive ? "Turn on allergy success!" : "Turn off allergy success!");
        }

        public async Task<IEnumerable<BackgroundDiseaseCreateUpdateDto>> GetAllBackgroundDiseasesAsync() 
        {
             return await _context.BenhNens.Select(x => new BackgroundDiseaseCreateUpdateDto
            {
                BackgroundDiseaseId = x.MaBenhNen,
                DiseaseName = x.TenBenhNen,
                IsActive = x.DangHoatDong ?? false
            }).ToListAsync();
        }

        public async Task<(bool IsSuccess, string Message)> CreateBackgroundDiseaseAsync(BackgroundDiseaseCreateUpdateDto dto)
        {
            var exists = await _context.BenhNens.AnyAsync(x => x.TenBenhNen == dto.DiseaseName);
            if (exists)
                return (false, "Background disease already exists");

            var benhNen = new BenhNen
            {
                TenBenhNen = dto.DiseaseName,
                DangHoatDong = dto.IsActive
            };

            _context.BenhNens.Add(benhNen);
            await _context.SaveChangesAsync();
            return (true, "Add background disease success!");
        }

        public async Task<(bool IsSuccess, string Message)> UpdateBackgroundDiseaseAsync(BackgroundDiseaseCreateUpdateDto dto)
        {
            var benhNen = await _context.BenhNens.FindAsync(dto.BackgroundDiseaseId);
            if (benhNen == null) return (false, "Not found background disease");

            var exists = await _context.BenhNens.AnyAsync(x => x.MaBenhNen != dto.BackgroundDiseaseId && x.TenBenhNen == dto.DiseaseName);
            if (exists)
                return (false, "Background disease already exists");

            benhNen.TenBenhNen = dto.DiseaseName;
            benhNen.DangHoatDong = dto.IsActive;

            await _context.SaveChangesAsync();
            return (true, "Update background disease success!");
        }

        public async Task<(bool IsSuccess, string Message)> ToggleBackgroundDiseaseStatusAsync(int id, bool isActive)
        {
            var benhNen = await _context.BenhNens.FindAsync(id);
            if (benhNen == null) return (false, "Not found background disease");

            benhNen.DangHoatDong = isActive;

            await _context.SaveChangesAsync();
            return (true, isActive ? "Turn on background disease success!" : "Turn off background disease success!");
        }

        public async Task<IEnumerable<DrugInteractionDetailDto>> GetAllDrugInteractionsAsync()
        {
            return await _context.TuongTacThuocs
                .Select(x => new DrugInteractionDetailDto
                {
                    Medicine1Id = x.MaThuoc1,
                    Medicine2Id = x.MaThuoc2,
                    Medicine1Name = x.MaThuoc1Navigation.TenThuoc,
                    Medicine2Name = x.MaThuoc2Navigation.TenThuoc,
                    SeverityLevel = x.MucDoNghiemTrong,
                    Description = x.MoTa ?? string.Empty
                }).ToListAsync();
        }

        public async Task<(bool IsSuccess, string Message)> CreateDrugInteractionAsync(DrugInteractionCreateDto dto)
        {
            var id1 = Math.Min(dto.Medicine1Id, dto.Medicine2Id);
            var id2 = Math.Max(dto.Medicine1Id, dto.Medicine2Id);

            if (id1 == id2)
                return (false, "Medicine cannot interact with itself");

            var medicinesExist = await _context.Thuocs.CountAsync(t => t.MaThuoc == id1 || t.MaThuoc == id2);
            if (medicinesExist != 2)
                return (false, "Not found medicine");

            var exists = await _context.TuongTacThuocs.AnyAsync(x => x.MaThuoc1 == id1 && x.MaThuoc2 == id2);
            if (exists)
                return (false, "Drug interaction already exists");

            var interaction = new TuongTacThuoc
            {
                MaThuoc1 = id1,
                MaThuoc2 = id2,
                MucDoNghiemTrong = dto.SeverityLevel,
                MoTa = dto.Description
            };

            _context.TuongTacThuocs.Add(interaction);
            await _context.SaveChangesAsync();
            return (true, "Add drug interaction success!");
        }

        public async Task<(bool IsSuccess, string Message)> DeleteDrugInteractionAsync(int medicine1Id, int medicine2Id)
        {
            var id1 = Math.Min(medicine1Id, medicine2Id);
            var id2 = Math.Max(medicine1Id, medicine2Id);

            var interaction = await _context.TuongTacThuocs.FindAsync(id1, id2);
            if (interaction == null) return (false, "Not found drug interaction");

            _context.TuongTacThuocs.Remove(interaction);
            await _context.SaveChangesAsync();
            return (true, "Delete drug interaction success!");
        }
    }
}
