

namespace Backend.Dtos
{
    public record UpdateUserRequest(
        string FirstName,
        string LastName,
        string Email,
        DateOnly Dob, 
        bool IsVolunteer,
        IFormFile Image
    );
} 