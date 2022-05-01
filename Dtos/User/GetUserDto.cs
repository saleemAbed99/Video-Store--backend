using System;

namespace VideoStore_Backend.Dtos.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; } = DateTime.MinValue;
        public string Gender { get; set; }
        public string Role { get; set; }

    }
}