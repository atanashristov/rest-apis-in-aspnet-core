using Azure.Storage.Blobs;

namespace MusicApi.Helpers
{
  public class FormFileUploader : IFormFileUploader
  {
    static readonly Random random = new Random();
    private readonly IConfiguration _configuration;

    public FormFileUploader(IConfiguration configuration)
    {
      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<String> UploadFormFile(IFormFile formFile, string containerName = "songcovers")
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

  }
}