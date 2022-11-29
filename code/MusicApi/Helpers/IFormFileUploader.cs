namespace MusicApi.Helpers
{
  public interface IFormFileUploader
  {
    Task<String?> UploadFormFile(IFormFile formFile, string containerName = "musiccover");
  }
}