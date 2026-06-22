using Microsoft.EntityFrameworkCore;
using Project_CNPM.Data;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Models;

namespace Project_CNPM.Area.Admin.Services
{
    public class DiseaseAdminService : IDiseaseAdminService
    {
        private readonly ApplicationDbContext _context;

        public DiseaseAdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiseaseDetailDto>> GetAllDiseasesAsync(bool includeDeleted = false)
        {
            var query = _context.Benhs.AsQueryable();

            if (!includeDeleted)
            {
                query = query.Where(b => b.DeleteAt == null); 
            }

            return await query.Select(b => new DiseaseDetailDto
            {
                DiseaseId = b.MaBenh,
                DiseaseName = b.TenBenh,
                Description = b.MoTa,
                DiseaseGroup = b.NhomBenh,
                SeverityLevel = b.MucDoNghiemTrong ?? 1,
                IsActive = b.DangHoatDong ?? false,
                CreatedAt = b.NgayTao ?? DateTime.Now,
                UpdatedAt = b.NgayCapNhat
            }).ToListAsync();
        }

        public async Task<DiseaseDetailDto?> GetDiseaseByIdAsync(int id)
        {
            var querry = await _context.Benhs.FindAsync(id);
            if (querry == null) return null;

            return new DiseaseDetailDto
            {
                DiseaseId = querry.MaBenh,
                DiseaseName = querry.TenBenh,
                Description = querry.MoTa,
                DiseaseGroup = querry.NhomBenh,
                SeverityLevel = querry.MucDoNghiemTrong ?? 1,
                IsActive = querry.DangHoatDong ?? false,
                CreatedAt = querry.NgayTao ?? DateTime.Now,
                UpdatedAt = querry.NgayCapNhat
            };
        }

        public async Task<(bool IsSuccess, string Message)> CreateDiseaseAsync(DiseaseCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DiseaseName))
                return (false, "Disease name cannot be empty");

            var existDisease = await _context.Benhs.FirstOrDefaultAsync(d => d.TenBenh.ToLower() == dto.DiseaseName.Trim().ToLower());

            if (existDisease != null)
                return (false, "Disease already exists");

            var benh = new Benh
            {
                TenBenh = dto.DiseaseName.Trim(),
                MoTa = dto.Description?.Trim(),
                NhomBenh = dto.DiseaseGroup,
                MucDoNghiemTrong = dto.SeverityLevel,
                DangHoatDong = true,
                NgayTao = DateTime.Now
            };

            _context.Benhs.Add(benh);
            await _context.SaveChangesAsync();
            return (true, "Add disease success!");
        }

        public async Task<(bool IsSuccess, string Message)> UpdateDiseaseAsync(DiseaseUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DiseaseName))
                return (false, "Disease name cannot be empty");

            var benh = await _context.Benhs.FindAsync(dto.DiseaseId);
            if (benh == null) return (false, "Not found disease");

            var exists = await _context.Benhs.AnyAsync(b => b.MaBenh != dto.DiseaseId && b.TenBenh.ToLower() == dto.DiseaseName.Trim().ToLower());
            if (exists)
                return (false, "Disease name already exists");

            benh.TenBenh = dto.DiseaseName.Trim();
            benh.MoTa = dto.Description?.Trim();
            benh.NhomBenh = dto.DiseaseGroup;
            benh.MucDoNghiemTrong = dto.SeverityLevel;
            benh.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            return (true, "Update success!");
        }

        public async Task<(bool IsSuccess, string Message)> ToggleDiseaseAsync(int id)
        {
            var benh = await _context.Benhs.FindAsync(id);

            benh.DeleteAt = DateTime.Now;
            benh.DangHoatDong = false;

            await _context.SaveChangesAsync();
            return (true, "turn off success");
        }
    }
}