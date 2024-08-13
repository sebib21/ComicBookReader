using ComicBookReader.UseCases.Interfaces;
using ComicBookReader.UseCases.PluginInterfaces;

namespace ComicBookReader.UseCases
{
    public class ViewIssueUseCase : IViewIssueUseCase
    {
        private readonly IComicIssueRepository comicIssueRepository;

        public ViewIssueUseCase(IComicIssueRepository comicIssueRepository)
        {
            this.comicIssueRepository = comicIssueRepository;
        }

        public async Task<List<byte[]>> ExecuteAsync(string folderPath)
        {
            return await comicIssueRepository.GetIssueAsync(folderPath);
        }
    }
}
