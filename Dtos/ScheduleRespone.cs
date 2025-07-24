using System;

namespace Backend.Dtos
{
    public class ScheduleResponse
    {
        public Guid Id { get; set; }
        public Guid CommunityId { get; set; }
        public DateTime ScheduleTime { get; set; }
    }
}