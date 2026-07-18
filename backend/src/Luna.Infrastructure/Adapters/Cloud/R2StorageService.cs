using Luna.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Luna.Infrastructure.Adapters.Cloud;

public class R2StorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly R2Options _options;
    private readonly ILogger<R2StorageService> _logger;

    public R2StorageService(IOptions<R2Options> options, ILogger<R2StorageService> logger)
    {
        _options = options.Value;
        _logger = logger;

        // Extract hostname 
        var endpoint = _options.Endpoint;
        if (Uri.TryCreate(endpoint, UriKind.Absolute, out var uri))
        {
            endpoint = uri.Host;
        }

        _minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(_options.AccessKeyId, _options.SecretAccessKey)
            .WithRegion("auto")
            .WithSSL()
            .Build();
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        var contentType = GetContentType(ext);

        // key: avatars/{guid}-avatar{ext}
        var key = $"avatars/{Guid.NewGuid():N}-avatar{ext}";

        var bucketExists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_options.BucketName), ct);

        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_options.BucketName), ct);
        }

        var putArgs = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(key)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putArgs, ct);

        // Build public url
        var publicUrl = $"{_options.PublicUrl.TrimEnd('/')}/{key}";
        return publicUrl;
    }

    private static string GetContentType(string extension)
    {
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream",
        };
    }
}

public class R2Options
{
    public const string SectionName = "R2";

    public string Endpoint { get; set; } = string.Empty;
    public string AccessKeyId { get; set; } = string.Empty;
    public string SecretAccessKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;
}
