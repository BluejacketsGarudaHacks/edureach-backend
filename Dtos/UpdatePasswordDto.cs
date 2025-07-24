namespace Backend.Dtos;

public class UpdatePasswordDto
{
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}