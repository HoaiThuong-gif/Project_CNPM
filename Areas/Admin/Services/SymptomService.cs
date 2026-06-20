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
            var query = _context.TrieuChungs.AsQueryable();

            return await query.Select(t => new SymptomDetailDto
            {
                SymptomId = t.MaTrieuChung,
                SymptomName = t.TenTrieuChung,
                Description = t.MoTa,
                IsActive = t.DangHoatDong ?? false,
                CreatedAt = t.NgayTao ?? DateTime.Now
            }).ToListAsync();
        }

        public async Task<SymptomDetailDto?> GetSymptomByIdAsync(int id)
        {
            var t = await _context.TrieuChungs.FindAsync(id);
            if (t == null) return null;

            return new SymptomDetailDto
            {
                SymptomId = t.MaTrieuChung,
                SymptomName = t.TenTrieuChung,
                Description = t.MoTa,
                IsActive = t.DangHoatDong ?? false,
                CreatedAt = t.NgayTao ?? DateTime.Now
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
            await _context.SaveChangesAsync();
            return (true, "Update success");
        }

        public async Task<(bool IsSuccess, string Message)> SolfDeleteAsync(int id)
        {
            var symptom = await _context.TrieuChungs.FindAsync(id);
            if (symptom == null) return (false, "Not found symptom");
            symptom.DangHoatDong = false;

            await _context.SaveChangesAsync();
            return (true, "Turn off success");
        }
    }
}