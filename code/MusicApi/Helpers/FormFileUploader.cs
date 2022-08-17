using Azure.Storage.Blobs;

namespace MusicApi.Helpers
{
  public class FormFileUploader : IFormFileUploader
  {
    private readonly IConfiguration _configuration;

    public FormFileUploader(IConfiguration configuration)
    {
      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<String> UploadFormFile(IFormFile formFile, string containerName = "songcovers")
    {
      var connectionString = _configuration.GetConnectionString("AzureStorageConnection");
      var containerClient = new BlobContainerClient(connectionString, containerName);
      BlobClient blobClient = containerClient.GetBlobClient(formFile.FileName);

      var memoryStream = new MemoryStream();
      await formFile.CopyToAsync(memoryStream);
      memoryStream.Position = 0;

      // TODO: Add error handling. If the file already exists, return a 409 Conflict.
      await blobClient.UploadAsync(memoryStream);
      return blobClient.Uri.AbsoluteUri;
    }

  }
}