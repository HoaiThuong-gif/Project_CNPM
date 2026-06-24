namespace Project_CNPM.DTOs.Medicine
{
    public class UserAdminViewDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsLocked { get; set; } // BiKhoa
        public bool IsDeleted { get; set; } // Kiểm tra DeleteAt != null
        public DateTime CreatedAt { get; set; }
    }
}