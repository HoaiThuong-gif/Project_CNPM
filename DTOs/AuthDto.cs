namespace Projcet_CNPM.DTOs.Auth
{
    public class loginDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class registerDto
    {
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
        public string confirmPassword { get; set; } = null!;
    }
}