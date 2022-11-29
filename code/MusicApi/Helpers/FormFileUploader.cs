using Azure;
using Azure.Storage.Blobs;

namespace MusicApi.Helpers
{
  public class FormFileUploader : IFormFileUploader
  {
    static readonly Random random = new Random();
    private readonly IConfiguration _configuration;
    private readonly ILogger<FormFileUploader> _logger;

    public FormFileUploader(IConfiguration configuration, ILogger<FormFileUploader> logger)
    {
      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<String?> UploadFormFile(IFormFile formFile, string containerName = "musiccover")
    {
      try
      {
        var connectionString = _configuration.GetConnectionString("AzureStorageConnection");
        var containerClient = new BlobContainerClient(connectionString, containerName);
        var fileName = random.Next(1000).ToString() + "-" + formFile.FileName;
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        // TODO: Add error handling. If the file already exists, return a 409 Conflict.
        await blobClient.UploadAsync(memoryStream);
        return blobClient.Uri.AbsoluteUri;
      }
      catch (RequestFailedException ex)
      {
        _logger.LogError($"Azure exception {ex}");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Exception {ex}");
      }

      return null;
    }

  }
}