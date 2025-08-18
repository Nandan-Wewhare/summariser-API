namespace Summary_generator_API.Services
{
    public interface IExtractionService
    {
        public string ExtractTextFromPdf(string filePath);
        public string ExtractTextFromExcel(string filePath);
        public Task<string> MoveFile(IFormFile file);
    }
}
