using Backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackRepository _feedbackRepository;

        public FeedbackController(FeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        // GET: api/Feedback
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync();
            return feedbacks;
        }

        // GET: api/Feedback/volunteer
        [HttpGet("volunteer/{volunteerId}")]
        public async Task<ActionResult<ICollection<Feedback>>> GetVolunteerFeedbacks(Guid volunteerId)
        {
            var feedbacks = await _feedbackRepository.GetAllVolunteerFeedbackAsync(volunteerId);
            return feedbacks;
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(Guid id)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return new Feedback
            {
                Id = feedback.Id,
                VolunteerId = feedback.VolunteerId,
                Message = feedback.Message
            };
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<Feedback>> CreateFeedback([FromBody] CreateFeedbackRequest request)
        {
            var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
            var feedback = new Feedback
            {
                UserId = userId,
                VolunteerId = Guid.Parse(request.VolunteerId),
                Message = request.Message
            };

            await _feedbackRepository.AddFeedbackAsync(feedback);

            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, new Feedback
            {
                Id = feedback.Id,
                VolunteerId = feedback.VolunteerId,
                Message = feedback.Message
            });
        }

        // PUT: api/Feedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(Guid id, [FromBody] CreateFeedbackRequest request)
        {
            var feedback = new Feedback
            {
                VolunteerId = Guid.Parse(request.VolunteerId),
                Message = request.Message
            };

            var updatedFeedback = await _feedbackRepository.UpdateFeedbackAsync(id, feedback);

            if (updatedFeedback == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var result = await _feedbackRepository.DeleteFeedbackAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}