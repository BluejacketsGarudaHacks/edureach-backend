

namespace Backend.Dtos
{
    public record UpdateUserRequest(
        string FirstName,
        string LastName,
        string Email,
        DateOnly Dob, 
        string Password,
        string ConfirmPassword,
        bool IsVolunteer,
        IFormFile Image
    );
} 