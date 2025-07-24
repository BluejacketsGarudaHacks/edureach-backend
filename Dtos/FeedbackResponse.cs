using System;

namespace Backend.Dtos
{
    public class FeedbackResponse
    {
        public Guid Id { get; set; }
        public Guid VolunteedId { get; set; }
        public string Message { get; set; }
    }
}