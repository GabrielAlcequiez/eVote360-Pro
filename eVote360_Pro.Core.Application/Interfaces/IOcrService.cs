namespace eVote360_Pro.Core.Application.Interfaces
{
    public interface IOcrService
    {
        /// <summary>
        /// Procesa una imagen y devuelve el número de documento
        /// extraído, o null si no fue posible detectarlo.
        /// </summary>
        Task<string?> ExtractDocumentNumberAsync(Stream imageStream);
    }
}