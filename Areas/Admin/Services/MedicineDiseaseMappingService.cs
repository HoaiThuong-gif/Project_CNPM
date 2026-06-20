using Microsoft.EntityFrameworkCore;
using Project_CNPM.Area.Admin.DTOs;
using Project_CNPM.Data;
using Project_CNPM.Models;
using System.Text;
using System.Text.Json;

namespace Project_CNPM.Area.Admin.Services
{
    public class MedicineDiseaseMappingService : IMedicineDiseaseMappingService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public MedicineDiseaseMappingService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5000");
        }

        public async Task<(bool IsSuccess, string Message)> LinkMedicineWithDiseaseAsync(int medicineId, int diseaseId, int priority, string treatmentType)
        {
            var thuoc = await _context.Thuocs.FindAsync(medicineId);
            if (thuoc == null) return (false, "Not found medicine");

            var benh = await _context.Benhs.FindAsync(diseaseId);
            if (benh == null) return (false, "Not found disease");

            var exists = await _context.BenhThuocs.AnyAsync(bt => bt.MaThuoc == medicineId && bt.MaBenh == diseaseId);
            if (exists) return (false, "Medicine is already linked with this disease");

            var benhThuoc = new BenhThuoc
            {
                MaThuoc = medicineId,
                MaBenh = diseaseId,
                DoUuTien = priority,
                LoaiDieuTri = treatmentType
            };

            _context.BenhThuocs.Add(benhThuoc);
            await _context.SaveChangesAsync();

            var linkedData = await _context.BenhThuocs
                .Include(bt => bt.MaBenhNavigation)
                .FirstOrDefaultAsync(bt => bt.MaThuoc == medicineId && bt.MaBenh == diseaseId);

            // GỌI API PYTHON ĐỂ EMBED
            await SyncWithAiBackendAsync(thuoc, linkedData);

            return (true, "Link medicine with disease success!");
        }

        public async Task<(bool IsSuccess, string Message)> UnlinkMedicineFromDiseaseAsync(int medicineId, int diseaseId)
        {
            var link = await _context.BenhThuocs.FindAsync(diseaseId, medicineId);
            if (link == null) return (false, "Mapping not found");

            _context.BenhThuocs.Remove(link);
            await _context.SaveChangesAsync();


            await RemoveFromAiBackendAsync(medicineId);

            var remainingLinks = await _context.BenhThuocs
                .Include(bt => bt.MaBenhNavigation)
                .Include(bt => bt.MaThuocNavigation)
                .Where(bt => bt.MaThuoc == medicineId)
                .ToListAsync();

            foreach (var remainLink in remainingLinks)
            {
                await SyncWithAiBackendAsync(remainLink.MaThuocNavigation, remainLink);
            }

            return (true, "Unlink success!");
        }

        public async Task<IEnumerable<DiseaseLinkedWithMedicineDto>> GetDiseasesOfMedicineAsync(int medicineId)
        {
            return await _context.BenhThuocs
                .Where(bt => bt.MaThuoc == medicineId)
                .Select(bt => new DiseaseLinkedWithMedicineDto
                {
                    DiseaseId = bt.MaBenh,
                    DiseaseName = bt.MaBenhNavigation.TenBenh,
                    Priority = bt.DoUuTien ?? 3,
                    TreatmentType = bt.LoaiDieuTri ?? "unknown"
                }).ToListAsync();
        }

        public async Task<IEnumerable<MedicineLinkedWithDiseaseDto>> GetMedicinesOfDiseaseAsync(int diseaseId)
        {
            return await _context.BenhThuocs
                .Where(bt => bt.MaBenh == diseaseId)
                .Select(bt => new MedicineLinkedWithDiseaseDto
                {
                    MedicineId = bt.MaThuoc,
                    MedicineName = bt.MaThuocNavigation.TenThuoc,
                    Priority = bt.DoUuTien ?? 3,
                    TreatmentType = bt.LoaiDieuTri ?? "unknown"
                }).ToListAsync();
        }

        private async Task SyncWithAiBackendAsync(Thuoc thuoc, BenhThuoc? benhThuoc)
        {
            try
            {
                if (benhThuoc == null) return;

                var payload = new
                {
                    ma_thuoc = thuoc.MaThuoc,
                    ten_thuoc = thuoc.TenThuoc,
                    hoat_chat = thuoc.HoatChat ?? "",
                    nhom_thuoc = thuoc.NhomThuoc ?? "",
                    cong_dung = thuoc.CongDung ?? "",
                    dang_bao_che = thuoc.DangBaoChe ?? "",
                    lieu_dung = thuoc.LieuDung ?? "",
                    can_ke_don = thuoc.CanKeDon ? 1 : 0,

                    ma_benh = benhThuoc.MaBenh,
                    ten_benh = benhThuoc.MaBenhNavigation != null ? benhThuoc.MaBenhNavigation.TenBenh : "",
                    do_uu_tien = benhThuoc.DoUuTien ?? 3,
                    loai_dieu_tri = benhThuoc.LoaiDieuTri ?? "unknown"
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("/embed-drug", jsonContent);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi đồng bộ map bệnh {benhThuoc?.MaBenh} của thuốc {thuoc.MaThuoc}: {ex.Message}");
            }
        }

        private async Task RemoveFromAiBackendAsync(int maThuoc)
        {
            try
            {
                var payload = new { ma_thuoc = maThuoc };
                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("/remove-drug", jsonContent);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xóa vector thuốc {maThuoc}: {ex.Message}");
            }
        }
    }
}