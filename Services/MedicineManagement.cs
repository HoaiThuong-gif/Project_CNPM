using Project_CNPM.Data;
using Projcet_CNPM.DTOs.Medicin;
using Microsoft.EntityFrameworkCore;
namespace Project_CNPM.Services
{
    public class MedicineManagement : IMedicineManagement
    {
        private readonly ApplicationDbContext _context;

        public MedicineManagement(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool IsSuccess, string Message)> AddMedicineAsync(MedicinDTO medicineDto)
        {
            if (_context.Thuocs.Any(m => m.TenThuoc == medicineDto.TenThuoc))
                return (false, "Medicine already exists");
            
            var medicine = new Models.Thuoc
            {
                TenThuoc = medicineDto.TenThuoc,
                HoatChat = medicineDto.HoatChat,
                CongDung = medicineDto.CongDung,
                LieuDung = medicineDto.LieuDung,
                CachDung = medicineDto.CachDung,
                TacDungPhu = medicineDto.TacDungPhu,
                LuuY = medicineDto.LuuY
            };

            _context.Thuocs.Add(medicine);
            await _context.SaveChangesAsync(); 
            return (true, "Medicine added successfully");
        }

        public async Task<(bool IsSuccess, string Message)> UpdateMedicineAsync(int id, MedicinDTO medicineDto)
        {
            var medicine = await _context.Thuocs.FindAsync(id);
            if (medicine == null) return (false, "Medicine not found");

            medicine.TenThuoc = medicineDto.TenThuoc;
            medicine.HoatChat = medicineDto.HoatChat;
            medicine.CongDung = medicineDto.CongDung;
            medicine.LieuDung = medicineDto.LieuDung;
            medicine.CachDung = medicineDto.CachDung;
            medicine.TacDungPhu = medicineDto.TacDungPhu;
            medicine.LuuY = medicineDto.LuuY;

            await _context.SaveChangesAsync();
            return (true, "Medicine updated successfully");
        }

        public async Task<(bool IsSuccess, string Message)> DeleteMedicineAsync(int id)
        {
            var medicine = _context.Thuocs.Find(id);
            if (medicine == null) return (false, "Medicine not found");

            _context.Thuocs.Remove(medicine);
            await _context.SaveChangesAsync();
            return (true, "Medicine deleted successfully");
        }

        public async Task<List<MedicinDTO>> GetAllMedicinesAsync()
        {
            var medicines = await _context.Thuocs.ToListAsync();
            return medicines.Select(m => new MedicinDTO
            {
                TenThuoc = m.TenThuoc,
                HoatChat = m.HoatChat,
                CongDung = m.CongDung,
                LieuDung = m.LieuDung,
                CachDung = m.CachDung,
                TacDungPhu = m.TacDungPhu,
                LuuY = m.LuuY
            }).ToList();
        }

        public async Task<MedicinDTO?> GetMedicineByIdAsync(int id)
        {
            var medicine = await _context.Thuocs.FindAsync(id);
            if (medicine == null) return null;

            return new MedicinDTO
            {
                TenThuoc = medicine.TenThuoc,
                HoatChat = medicine.HoatChat,
                CongDung = medicine.CongDung,
                LieuDung = medicine.LieuDung,
                CachDung = medicine.CachDung,
                TacDungPhu = medicine.TacDungPhu,
                LuuY = medicine.LuuY
            };
        }
    }
}