using System.Text.RegularExpressions;
using eVote360_Pro.Core.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Tesseract;

namespace eVote360_Pro.Infrastructure.Services
{
    public partial class OcrService(ILogger<OcrService> logger) : IOcrService
    {
        private readonly ILogger<OcrService> _logger = logger;
        private readonly string _tessDataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");

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

        /// <summary>
        /// Busca dentro del texto extraído el número de cédula, normalizando caracteres típicos
        /// y ofreciendo un fallback flexible si hay ruido en la imagen.
        /// </summary>
        private static string? ExtractDocumentNumber(string ocrText)
        {
            if (string.IsNullOrWhiteSpace(ocrText)) return null;

            // 1. Normalizar errores comunes de OCR (letras leídas por números)
            var normalizedText = ocrText
                .Replace('O', '0')
                .Replace('o', '0')
                .Replace('I', '1')
                .Replace('i', '1')
                .Replace('l', '1');

            // 2. Intentar formato exacto con guiones
            var matchWithDashes = RegexDashes().Match(normalizedText);
            if (matchWithDashes.Success)
                return matchWithDashes.Value.Replace("-", "");

            // 3. Intentar formato exacto de 11 dígitos
            var matchDigitsOnly = RegexNumber().Match(normalizedText);
            if (matchDigitsOnly.Success)
                return matchDigitsOnly.Value;

            // 4. Fallback extremo: Remover todo lo que no sea dígito y retornar todos los dígitos juntos.
            // Esto permite que el llamador verifique si la cédula ingresada está contenida como subcadena.
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
