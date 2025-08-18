using Microsoft.AspNetCore.Mvc;
using PPT_generator_API.Services;

namespace PPT_generator_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        private readonly IPresentationService _presentationService;
        private readonly IOpenAIService _openAIService;
        public GenerateController(IPresentationService presentationService, IOpenAIService openAIService)
        {
            _presentationService = presentationService;
            _openAIService = openAIService;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (extension != ".pdf" && extension != ".xls" && extension != ".xlsx")
                return BadRequest("Only PDF and Excel files are supported.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var extractedText = _presentationService.ExtractTextFromPdf(filePath);

            var chatResult = await _openAIService.GeneratePresentationContentAsync(extractedText);

            return Ok(chatResult);
        }
    }
}
