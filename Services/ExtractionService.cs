using OfficeOpenXml;
using System.Text;
using UglyToad.PdfPig;

namespace Summary_generator_API.Services
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

        public string ExtractTextFromExcel(string filePath)
        {
            var summaryBuilder = new StringBuilder();
            ExcelPackage.License.SetNonCommercialPersonal("Nandan Wewhare");

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    summaryBuilder.AppendLine($"Sheet: {worksheet.Name}");
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        summaryBuilder.AppendLine($"Row {row}:");
                        for (int col = 1; col <= colCount; col++)
                        {
                            string header = worksheet.Cells[1, col].Text;
                            string value = worksheet.Cells[row, col].Text;
                            summaryBuilder.AppendLine($"{header}: {value}");
                        }
                        summaryBuilder.AppendLine();
                    }
                }
            }
            return summaryBuilder.ToString();

        }

        public async Task<string> MoveFile(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
