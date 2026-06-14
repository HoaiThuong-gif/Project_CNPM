namespace Project_CNPM.Area.Admin.DTOs
{
    public class loginDto
    {
        
        public required string email { get; set; }
        public required string password { get; set; }
    }

    public class registerDto
    {
        public required string name { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        public required string confirmPassword { get; set; }
    }
}