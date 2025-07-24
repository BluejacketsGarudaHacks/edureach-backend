using System;

namespace Backend.Dtos
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsVolunteer { get; set; }
        public DateOnly Dob { get; set; }
        public string ImagePath { get; set; }
    }
}