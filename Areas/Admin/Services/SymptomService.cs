using Microsoft.EntityFrameworkCore;
using Project_CNPM.Data;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Models;
using System.Diagnostics;

namespace Project_CNPM.Area.Admin.Services
{
    public class SymptomAdminService : ISymptomAdminService
    {
        private readonly ApplicationDbContext _context;

        public SymptomAdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SymptomDetailDto>> GetAllSymptomsAsync()
        {
            var query = _context.TrieuChungs
                .Include(t => t.BenhTrieuChungs)
                    .ThenInclude(bt => bt.MaBenhNavigation)
                .AsQueryable();

            return await query.Select(t => new SymptomDetailDto
            {
                SymptomId = t.MaTrieuChung,
                SymptomName = t.TenTrieuChung,
                Description = t.MoTa ?? string.Empty,
                IsActive = t.DangHoatDong ?? false,
                CreatedAt = t.NgayTao ?? DateTime.Now,
                DiseaseIds = t.BenhTrieuChungs.Select(x => x.MaBenh).ToList(),
                Diseases = t.BenhTrieuChungs.Select(x => new SymptomDiseaseLinkDto
                {
                    DiseaseId = x.MaBenh,
                    DiseaseName = x.MaBenhNavigation.TenBenh
                }).ToList()
            }).ToListAsync();
        }

        public async Task<SymptomDetailDto?> GetSymptomByIdAsync(int id)
        {
            var t = await _context.TrieuChungs
                .Include(x => x.BenhTrieuChungs)
                    .ThenInclude(bt => bt.MaBenhNavigation)
                .FirstOrDefaultAsync(x => x.MaTrieuChung == id);
            if (t == null) return null;

            return new SymptomDetailDto
            {
                SymptomId = t.MaTrieuChung,
                SymptomName = t.TenTrieuChung,
                Description = t.MoTa ?? string.Empty,
                IsActive = t.DangHoatDong ?? false,
                CreatedAt = t.NgayTao ?? DateTime.Now,
                DiseaseIds = t.BenhTrieuChungs.Select(x => x.MaBenh).ToList(),
                Diseases = t.BenhTrieuChungs.Select(x => new SymptomDiseaseLinkDto
                {
                    DiseaseId = x.MaBenh,
                    DiseaseName = x.MaBenhNavigation.TenBenh
                }).ToList()
            };
        }

        public async Task<(bool IsSuccess, string Message)> CreateSymptomAsync(SymptomCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SymptomName))
                return (false, "Symptom name cannot be empty");

            var existSymptom = await _context.TrieuChungs.FirstOrDefaultAsync(s => s.TenTrieuChung.ToLower() == dto.SymptomName.Trim().ToLower());

            if (existSymptom != null)
                return (false, "Symptom already exists");

            var trieuChung = new TrieuChung
            {
                TenTrieuChung = dto.SymptomName.Trim(),
                MoTa = dto.Description?.Trim(),
                DangHoatDong = true,
                NgayTao = DateTime.Now
            };

            _context.TrieuChungs.Add(trieuChung);
            
            await _context.SaveChangesAsync();
            await SyncSymptomDiseasesAsync(trieuChung.MaTrieuChung, dto.DiseaseIds);
            return (true, "Add success");
        }

        public async Task<(bool IsSuccess, string Message)> UpdateSymptomAsync(SymptomCreateUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SymptomName))
                return (false, "Symptom name cannot be empty");

            var trieuChung = await _context.TrieuChungs.FindAsync(dto.SymptomId);
            if (trieuChung == null) return (false, "Not found Symptom");

            var exists = await _context.TrieuChungs.AnyAsync(s => s.MaTrieuChung != dto.SymptomId && s.TenTrieuChung.ToLower() == dto.SymptomName.Trim().ToLower());
            if (exists)
                return (false, "Symptom name already exists");

            trieuChung.TenTrieuChung = dto.SymptomName.Trim();
            trieuChung.MoTa = dto.Description?.Trim();
            await SyncSymptomDiseasesAsync(trieuChung.MaTrieuChung, dto.DiseaseIds);
            await _context.SaveChangesAsync();
            return (true, "Update success");
        }

        public async Task<(bool IsSuccess, string Message)> ToggleSymptomAsync(int id)
        {
            var symptom = await _context.TrieuChungs.FindAsync(id);
            if (symptom == null) return (false, "Not found symptom");
            symptom.DangHoatDong = !symptom.DangHoatDong;

            await _context.SaveChangesAsync();
            return (true, "Turn off success");
        }

        private async Task SyncSymptomDiseasesAsync(int symptomId, IEnumerable<int>? diseaseIds)
        {
            var newIds = (diseaseIds ?? Enumerable.Empty<int>())
                .Where(id => id > 0)
                .Distinct()
                .ToHashSet();

            var currentLinks = await _context.BenhTrieuChungs
                .Where(x => x.MaTrieuChung == symptomId)
                .ToListAsync();

            _context.BenhTrieuChungs.RemoveRange(currentLinks.Where(x => !newIds.Contains(x.MaBenh)));

            var currentIds = currentLinks.Select(x => x.MaBenh).ToHashSet();
            var validNewIds = await _context.Benhs
                .Where(x => newIds.Contains(x.MaBenh) && x.DeleteAt == null)
                .Select(x => x.MaBenh)
                .ToListAsync();

            foreach (var diseaseId in validNewIds.Where(id => !currentIds.Contains(id)))
            {
                _context.BenhTrieuChungs.Add(new BenhTrieuChung
                {
                    MaBenh = diseaseId,
                    MaTrieuChung = symptomId,
                    TrongSo = 3
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
