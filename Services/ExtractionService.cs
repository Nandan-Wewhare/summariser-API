using System.Text;
using UglyToad.PdfPig;

namespace PPT_generator_API.Services
{
    public class ExtractionService : IExtractionService
    {
        public string ExtractTextFromPdf(string filePath)
        {
            var text = new StringBuilder();
            using (var document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }
            return text.ToString();
        }
    }
}
