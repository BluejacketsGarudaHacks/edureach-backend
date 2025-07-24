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
        public async Task<ActionResult<IEnumerable<FeedbackResponse>>> GetFeedbacks()
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync();
            return feedbacks.Select(f => new FeedbackResponse
            {
                Id = f.Id,
                VolunteedId = f.VolunteedId,
                Message = f.Message
            }).ToList();
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackResponse>> GetFeedback(Guid id)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return new FeedbackResponse
            {
                Id = feedback.Id,
                VolunteedId = feedback.VolunteedId,
                Message = feedback.Message
            };
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<ActionResult<FeedbackResponse>> CreateFeedback([FromBody] CreateFeedbackRequest request)
        {
            var feedback = new Feedback
            {
                VolunteerId = Guid.Parse(request.VolunteedId),
                Message = request.Message
            };

            await _feedbackRepository.AddFeedbackAsync(feedback);

            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, new FeedbackResponse
            {
                Id = feedback.Id,
                VolunteedId = feedback.VolunteedId,
                Message = feedback.Message
            });
        }

        // PUT: api/Feedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(Guid id, [FromBody] CreateFeedbackRequest request)
        {
            var feedback = new Feedback
            {
                VolunteedId = Guid.Parse(request.VolunteedId),
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