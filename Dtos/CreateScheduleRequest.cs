namespace Backend.Dtos;

public class CreateScheduleRequest {
    public string CommunityId { get; set; }
    public DateTime ScheduleTime { get; set; }
}
