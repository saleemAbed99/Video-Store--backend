using System;

namespace VideoStore_Backend.Dtos.User
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public DateTime DOB { get; set; } = DateTime.MinValue;
        public string Gender { get; set; }
        public string password { get; set; }
        public string Role { get; set; }
    
    }
}