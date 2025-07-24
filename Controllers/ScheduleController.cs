using Backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly ScheduleRepository _scheduleRepository;

    public ScheduleController(ScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    // GET: api/Schedule
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleResponse>>> GetSchedules()
    {
        var schedules = await _scheduleRepository.GetAllSchedulesAsync();
        return schedules.Select(s => new ScheduleResponse
        {
            Id = s.Id,
            CommunityId = s.CommunityId,
            ScheduleTime = s.ScheduleTime
        }).ToList();
    }

    // GET: api/Schedule/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleResponse>> GetSchedule(Guid id)
    {
        var schedule = await _scheduleRepository.GetScheduleByIdAsync(id);

        if (schedule == null)
        {
            return NotFound();
        }

        return Ok(schedule);
    }

    // POST: api/Schedule
    [HttpPost]
    public async Task<ActionResult<ScheduleResponse>> CreateSchedule([FromBody] CreateScheduleRequest request)
    {
        var schedule = new Schedule
        {
            CommunityId = Guid.Parse(request.CommunityId),
            ScheduleTime = request.ScheduleTime
        };

        await _scheduleRepository.AddScheduleAsync(schedule);

        return CreatedAtAction("GetSchedule", new { id = schedule.Id }, new ScheduleResponse
        {
            Id = schedule.Id,
            CommunityId = schedule.CommunityId,
            ScheduleTime = schedule.ScheduleTime
        });
    }

    // PUT: api/Schedule/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] CreateScheduleRequest request)
    {
        var schedule = new Schedule
        {
            CommunityId = Guid.Parse(request.CommunityId),
            ScheduleTime = request.ScheduleTime
        };


        var updatedSchedule = await _scheduleRepository.UpdateScheduleAsync(id, schedule);

        if (updatedSchedule == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Schedule/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        var result = await _scheduleRepository.DeleteScheduleAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}