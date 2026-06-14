using Microsoft.EntityFrameworkCore;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Data;
using Project_CNPM.Models;

namespace Project_CNPM.Area.Admin.Services
{
    public class MedicineAdminService : IMedicineAdminService
    {
        private readonly ApplicationDbContext _context;

        public MedicineAdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicineDetailDto>> GetAllMedicinesAsync(bool includeInactive = true)
        {
            var query = _context.Thuocs.AsQueryable();

            if (!includeInactive)
                query = query.Where(t => t.DangHoatDong == true);

            return await query.Select(t => new MedicineDetailDto
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
                RequiresPrescription = t.CanKeDon,
                IsActive = t.DangHoatDong ?? false,
                CreatedAt = t.NgayTao ?? DateTime.Now,
                UpdatedAt = t.NgayCapNhat
            }).ToListAsync();
        }

        public async Task<MedicineDetailDto?> GetMedicineByIdAsync(int id)
        {
            var t = await _context.Thuocs.FindAsync(id);
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
                RequiresPrescription = t.CanKeDon,
                IsActive = t.DangHoatDong ?? false,
                CreatedAt = t.NgayTao ?? DateTime.Now,
                UpdatedAt = t.NgayCapNhat
            };
        }

        public async Task<(bool IsSuccess, string Message)> CreateMedicineAsync(MedicineCreateDto dto)
        {
            var exists = await _context.Thuocs.AnyAsync(t => t.TenThuoc == dto.MedicineName);
            if (exists)
                return (false, "Medicine already exists");

            var thuoc = new Thuoc
            {
                TenThuoc = dto.MedicineName,
                HoatChat = dto.ActiveIngredient,
                NhomThuoc = dto.MedicineGroup,
                DangBaoChe = dto.DosageForm,
                CongDung = dto.Uses,
                LieuDung = dto.Dosage,
                CachDung = dto.HowToUse,
                TacDungPhu = dto.SideEffects,
                LuuY = dto.Notes,
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
            var thuoc = await _context.Thuocs.FindAsync(dto.MedicineId);
            if (thuoc == null) return (false, "Not found medicine");

            var exists = await _context.Thuocs.AnyAsync(t => t.MaThuoc != dto.MedicineId && t.TenThuoc == dto.MedicineName);
            if (exists)
                return (false, "Medicine already exists");

            thuoc.TenThuoc = dto.MedicineName;
            thuoc.HoatChat = dto.ActiveIngredient;
            thuoc.NhomThuoc = dto.MedicineGroup;
            thuoc.DangBaoChe = dto.DosageForm;
            thuoc.CongDung = dto.Uses;
            thuoc.LieuDung = dto.Dosage;
            thuoc.CachDung = dto.HowToUse;
            thuoc.TacDungPhu = dto.SideEffects;
            thuoc.LuuY = dto.Notes;
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
    }
}
