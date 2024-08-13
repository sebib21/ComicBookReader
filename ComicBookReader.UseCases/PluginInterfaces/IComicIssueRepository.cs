using ComicBookReader.CoreBusiness;

namespace ComicBookReader.UseCases.PluginInterfaces
{
    public interface IComicIssueRepository
    {
        Task<List<byte[]>> GetIssueAsync(string folderPath);
        Task<List<ComicIssue>> GetIssuesAsync(int locationId);
    }
}