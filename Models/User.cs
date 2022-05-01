using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoStore_Backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; } = DateTime.MinValue;
        public string Gender { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [Required]
        public string Role { get; set; }
        public List<Gallery> Galleries { get; set; }
        public FavoriteList FavoriteList { get; set; }
    }
}