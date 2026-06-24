using Microsoft.EntityFrameworkCore;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Data;
using Project_CNPM.Models;
using System.Text;
using System.Text.Json;

namespace Project_CNPM.Area.Admin.Services
{
    public class MedicineAdminService : IMedicineAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public MedicineAdminService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5000"); 
        }

        public async Task<IEnumerable<MedicineDetailDto>> GetAllMedicinesAsync(bool includeInactive = true)
        {
            var query = _context.Thuocs
                .Include(t => t.MaThanhPhans)
                .Include(t => t.CanhBaoDiUngThuocs)
                    .ThenInclude(c => c.MaDiUngNavigation)
                .Include(t => t.CanhBaoBenhNenThuocs)
                    .ThenInclude(c => c.MaBenhNenNavigation)
                .AsQueryable();

            if (!includeInactive)
                query = query.Where(t => t.DangHoatDong == true);

            var medicines = await query.ToListAsync();
            return medicines.Select(t => new MedicineDetailDto
            {
                MedicineId = t.MaThuoc,
                MedicineName = t.TenThuoc,
                ActiveIngredient = t.HoatChat ?? string.Empty,
                MedicineGroup = t.NhomThuoc ?? string.Empty,
                DosageForm = t.DangBaoChe ?? string.Empty,
                Uses = t.CongDung ?? string.Empty,
                Dosage = t.LieuDung ?? string.Empty,
                HowToUse = t.CachDung ?? string.Empty,
                SideEffects = t.TacDungPhu ?? string.Empty,
                Notes = t.LuuY ?? string.Empty,
                Components = BuildMedicineComponents(t),
                Contraindications = BuildMedicineContraindications(t),
                RequiresPrescription = t.CanKeDon,
                IsActive = t.DangHoatDong ?? false,
                CreatedAt = t.NgayTao ?? DateTime.Now,
                UpdatedAt = t.NgayCapNhat
            }).ToList();
        }

        public async Task<MedicineDetailDto?> GetMedicineByIdAsync(int id)
        {
            var t = await _context.Thuocs
                .Include(x => x.MaThanhPhans)
                .Include(x => x.CanhBaoDiUngThuocs)
                    .ThenInclude(x => x.MaDiUngNavigation)
                .Include(x => x.CanhBaoBenhNenThuocs)
                    .ThenInclude(x => x.MaBenhNenNavigation)
                .FirstOrDefaultAsync(x => x.MaThuoc == id);
            if (t == null) return null;

            return new MedicineDetailDto
            {
                MedicineId = t.MaThuoc,
                MedicineName = t.TenThuoc,
                ActiveIngredient = t.HoatChat ?? string.Empty,
                MedicineGroup = t.NhomThuoc ?? string.Empty,
                DosageForm = t.DangBaoChe ?? string.Empty,
                Uses = t.CongDung ?? string.Empty,
                Dosage = t.LieuDung ?? string.Empty,
                HowToUse = t.CachDung ?? string.Empty,
                SideEffects = t.TacDungPhu ?? string.Empty,
                Notes = t.LuuY ?? string.Empty,
                Components = BuildMedicineComponents(t),
                Contraindications = BuildMedicineContraindications(t),
                RequiresPrescription = t.CanKeDon,
                IsActive = t.DangHoatDong ?? false,
                CreatedAt = t.NgayTao ?? DateTime.Now,
                UpdatedAt = t.NgayCapNhat
            };
        }

        public async Task<(bool IsSuccess, string Message)> CreateMedicineAsync(MedicineCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MedicineName))
                return (false, "Medicine name cannot be empty");

            var medicineName = dto.MedicineName.Trim();
            var exists = await _context.Thuocs.AnyAsync(t => t.TenThuoc.ToLower() == medicineName.ToLower());
            if (exists)
                return (false, "Medicine already exists");

            var thuoc = new Thuoc
            {
                TenThuoc = medicineName,
                HoatChat = dto.ActiveIngredient?.Trim(),
                NhomThuoc = dto.MedicineGroup?.Trim(),
                DangBaoChe = dto.DosageForm?.Trim(),
                CongDung = dto.Uses?.Trim(),
                LieuDung = dto.Dosage?.Trim(),
                CachDung = dto.HowToUse?.Trim(),
                TacDungPhu = dto.SideEffects?.Trim(),
                LuuY = dto.Notes?.Trim(),
                CanKeDon = dto.RequiresPrescription,
                DangHoatDong = true,
                NgayTao = DateTime.Now
            };

            _context.Thuocs.Add(thuoc);
            await _context.SaveChangesAsync(); 

            return (true, "Add medicine success!");
        }

        public async Task<(bool IsSuccess, string Message)> UpdateMedicineAsync(MedicineUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MedicineName))
                return (false, "Medicine name cannot be empty");

            var thuoc = await _context.Thuocs.FindAsync(dto.MedicineId);
            if (thuoc == null) return (false, "Not found medicine");

            var medicineName = dto.MedicineName.Trim();
            var exists = await _context.Thuocs.AnyAsync(t => t.MaThuoc != dto.MedicineId && t.TenThuoc.ToLower() == medicineName.ToLower());
            if (exists)
                return (false, "Medicine already exists");

            thuoc.TenThuoc = medicineName;
            thuoc.HoatChat = dto.ActiveIngredient?.Trim();
            thuoc.NhomThuoc = dto.MedicineGroup?.Trim();
            thuoc.DangBaoChe = dto.DosageForm?.Trim();
            thuoc.CongDung = dto.Uses?.Trim();
            thuoc.LieuDung = dto.Dosage?.Trim();
            thuoc.CachDung = dto.HowToUse?.Trim();
            thuoc.TacDungPhu = dto.SideEffects?.Trim();
            thuoc.LuuY = dto.Notes?.Trim();
            thuoc.CanKeDon = dto.RequiresPrescription;
            thuoc.DangHoatDong = dto.IsActive;
            thuoc.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();

            return (true, "Update medicine success!");
        }

        public async Task<(bool IsSuccess, string Message)> ToggleMedicineStatusAsync(int id, bool isActive)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return (false, "Not found medicine");

            thuoc.DangHoatDong = isActive;
            thuoc.NgayCapNhat = DateTime.Now;

            await _context.SaveChangesAsync();
            return (true, isActive ? "Turn on medicine success!" : "Turn off medicine success!");
        }

        private static string BuildMedicineComponents(Thuoc medicine)
        {
            return medicine.MaThanhPhans.Any()
                ? string.Join(", ", medicine.MaThanhPhans.Select(x => x.TenThanhPhan))
                : medicine.HoatChat ?? string.Empty;
        }

        private static string BuildMedicineContraindications(Thuoc medicine)
        {
            var warnings = medicine.CanhBaoDiUngThuocs
                .Select(x => $"Dị ứng {x.MaDiUngNavigation.TenDiUng}: {x.NoiDung}")
                .Concat(medicine.CanhBaoBenhNenThuocs.Select(x => $"Bệnh nền {x.MaBenhNenNavigation.TenBenhNen}: {x.NoiDung}"))
                .ToList();

            return warnings.Any() ? string.Join(" | ", warnings) : string.Empty;
        }

    }
}
