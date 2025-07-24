using System;

namespace Backend.Dtos
{
    public class FeedbackResponse
    {
        public Guid Id { get; set; }
        public Guid VolunteerId { get; set; }
        public string Message { get; set; }
    }
}