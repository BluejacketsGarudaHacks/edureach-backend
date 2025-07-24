namespace Backend.Dtos;

public class NotificationRequest
{
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public bool IsShown { get; set; }
}