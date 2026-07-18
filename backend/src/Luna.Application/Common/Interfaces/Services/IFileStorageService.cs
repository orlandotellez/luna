namespace Luna.Application.Common.Interfaces.Services;

public interface IFileStorageService
{
    /// <summary>
    /// upload file image to cloudflare r2
    /// Formats: jpg, jpeg, png, gif, webp.
    /// </summary>
    Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken ct = default);
}
