using ComicBookReader.CoreBusiness;
using ComicBookReader.UseCases.Interfaces;
using ComicBookReader.UseCases.PluginInterfaces;

namespace ComicBookReader.UseCases
{
    public class ViewIssuesUseCase : IViewIssuesUseCase
    {
        private readonly IComicIssueRepository comicIssueRepository;

        public ViewIssuesUseCase(IComicIssueRepository comicIssueRepository)
        {
            this.comicIssueRepository = comicIssueRepository;
        }

        public async Task<List<ComicIssue>> ExecuteAsync(int locationId)
        {
            return await comicIssueRepository.GetIssuesAsync(locationId);
        }
    }
}
