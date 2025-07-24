using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using Backend.Dtos;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummarizeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserRepository _userRepository;

        public SummarizeController(IHttpClientFactory httpClientFactory,
            UserRepository userRepository)
        {
            _httpClientFactory = httpClientFactory;
            _userRepository = userRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdfToPython(
            IFormFile file,
            [FromForm] string sourceLang,
            [FromForm] string targetLang)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var client = _httpClientFactory.CreateClient();
            var userId = Guid.Parse(HttpContext.Items["UserId"]!.ToString()!);
            
            var requestContent = new MultipartFormDataContent();

            // Add file
            using var streamContent = new StreamContent(file.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            requestContent.Add(streamContent, "file", file.FileName);

            // Add form fields
            requestContent.Add(new StringContent(sourceLang), "source_lang");
            requestContent.Add(new StringContent(targetLang), "target_lang");

            // Call your Python FastAPI endpoint
            var response = await client.PostAsync("http://localhost:8000/upload-pdf", requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, error);
            }

            var result = await response.Content.ReadAsStringAsync();

            if (result == null)
                throw new Exception("Error meringkas data!");

            var jsonResponse = JsonSerializer.Deserialize<SummarizeResponse>(result);
            var saveResult = await this.SaveUserSummary(userId, file, jsonResponse.Result);
            
            return Ok(result);
        }

        private async Task<bool> SaveUserSummary(Guid userId, IFormFile file, string summaryResult)
        {
            var fileName = file.FileName;

            var userSummary = new UserSummary()
            {
                SummaryResult = summaryResult,
                SummaryTitle = fileName,
                UserId = userId,
            };

            await _userRepository.CreateUserSummary(userSummary);
            return true;
        }
    }
}