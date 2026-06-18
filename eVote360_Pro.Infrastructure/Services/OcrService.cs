using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using eVote360_Pro.Core.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Tesseract;

namespace eVote360_Pro.Infrastructure.Services
{
    public partial class OcrService(ILogger<OcrService> logger) : IOcrService
    {
        private readonly ILogger<OcrService> _logger = logger;

        // asignación dinamica por que estoy usando arch linux
        private readonly string _tessDataPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? Path.Combine(AppContext.BaseDirectory, "tessdata")
            : "/usr/share/tessdata/"; // ruta estandar de arch

        public Task<string?> ExtractDocumentNumberAsync(Stream imageStream)
        {
            try
            {
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    imageStream.CopyTo(ms);
                    imageBytes = ms.ToArray();
                }
                
                _logger.LogInformation("Iniciando Tesseract OCR en ruta: {Path}", _tessDataPath);
                using var engine = new TesseractEngine(_tessDataPath, "spa", EngineMode.Default);
                using var pix = Pix.LoadFromMemory(imageBytes);
                using var page = engine.Process(pix);

                var fullText = page.GetText();
                _logger.LogInformation("Texto crudo extraído por Tesseract OCR:\n{Text}", fullText);

                var documentNumber = ExtractDocumentNumber(fullText);
                _logger.LogInformation("Resultado de extracción OCR: '{Result}'", documentNumber);

                return Task.FromResult(documentNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al procesar la imagen con Tesseract OCR.");
                return Task.FromResult<string?>(null);
            }
        }

        private static string? ExtractDocumentNumber(string ocrText)
        {
            if (string.IsNullOrWhiteSpace(ocrText)) return null;

            var normalizedText = ocrText
                .Replace('O', '0')
                .Replace('o', '0')
                .Replace('I', '1')
                .Replace('i', '1')
                .Replace('l', '1');

            var matchWithDashes = RegexDashes().Match(normalizedText);
            if (matchWithDashes.Success)
                return matchWithDashes.Value.Replace("-", "");

            var matchDigitsOnly = RegexNumber().Match(normalizedText);
            if (matchDigitsOnly.Success)
                return matchDigitsOnly.Value;

            var cleanDigits = new string(normalizedText.Where(char.IsDigit).ToArray());
            if (cleanDigits.Length >= 11)
            {
                return cleanDigits;
            }

            return null;
        }

        [GeneratedRegex(@"\b\d{3}-\d{7}-\d{1}\b")]
        private static partial Regex RegexDashes();

        [GeneratedRegex(@"\b\d{11}\b")]
        private static partial Regex RegexNumber();
    }
}
