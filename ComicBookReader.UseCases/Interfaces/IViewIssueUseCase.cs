namespace ComicBookReader.UseCases.Interfaces
{
    public interface IViewIssueUseCase
    {
        Task<List<byte[]>> ExecuteAsync(string folderPath);
    }
}