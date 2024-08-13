using ComicBookReader.CoreBusiness;

namespace ComicBookReader.UseCases.Interfaces
{
    public interface IViewIssuesUseCase
    {
        Task<List<ComicIssue>> ExecuteAsync(int locationId);
    }
}