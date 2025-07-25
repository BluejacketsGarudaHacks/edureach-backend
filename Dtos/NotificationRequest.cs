namespace Backend.Dtos;

public class NotificationRequest
{
    public string Message { get; set; }
    public bool IsShown { get; set; }
    public bool IsChecked { get; set; }
}