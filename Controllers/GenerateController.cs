using Microsoft.AspNetCore.Mvc;
using Summary_generator_API.Services;

namespace Summary_generator_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        private readonly IExtractionService _extractionService;
        private readonly IOpenAIService _openAIService;
        public GenerateController(IExtractionService extractionService, IOpenAIService openAIService)
        {
            _extractionService = extractionService;
            _openAIService = openAIService;
        }

        [HttpGet]
        public ActionResult HealthCheck()
        {
            return Ok("Service is up 🚀");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string jobDescription)
        {
            string extractedText = "";
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            if (jobDescription.Length == 0)
                return BadRequest("Job description is required.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            string filePath = "";
            switch (extension)
            {
                case ".pdf":
                    filePath = await _extractionService.MoveFile(file);
                    extractedText = _extractionService.ExtractTextFromPdf(filePath);
                    break;
                default:
                    return BadRequest("File type not supported.");
            }
            var chatResult = await _openAIService.GenerateContentAsync(extractedText, jobDescription);

            return Ok(chatResult);
        }
    }
}
