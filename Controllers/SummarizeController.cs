using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummarizeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SummarizeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
            return Ok(result);
        }
    }
}