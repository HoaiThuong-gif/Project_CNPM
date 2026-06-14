namespace  Project_CNPM.Area.Admin.DTOs
{
    public class MedicineCreateDto
    {
        public string MedicineName { get; set; } = string.Empty;
        public string ActiveIngredient { get; set; } = string.Empty; //HoatChat
        public string MedicineGroup { get; set; } = string.Empty; //NhomThuoc
        public string DosageForm { get; set; } = string.Empty; // DangBaoChe
        public string Uses { get; set; } = string.Empty; // CongDung
        public string Dosage { get; set; } = string.Empty; // LieuDung
        public string HowToUse { get; set; } = string.Empty; // CachDung
        public string SideEffects { get; set; } = string.Empty; // TacDungPhu
        public string Notes { get; set; } = string.Empty; // LuuY
        public bool RequiresPrescription { get; set; } // CanKeDon
    }

    public class MedicineUpdateDto : MedicineCreateDto
    {
        public int MedicineId { get; set; }
        public bool IsActive { get; set; } // DangHoatDong
    }

    public class MedicineDetailDto : MedicineUpdateDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}