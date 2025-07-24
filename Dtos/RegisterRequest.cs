namespace Backend.Dtos
{
    public record RegisterRequest(
        string FirstName,
        string LastName,
        string Email,
        DateOnly Dob, 
        string Password,
        string ConfirmPassword,
        bool IsVolunteer
    );
} 