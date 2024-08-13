using ComicBookReader.CoreBusiness;

namespace ComicBookReader.UseCases.PluginInterfaces
{
    public interface IComicFolderRepository
    {
        Task AddComicFolderAsync(ComicFolder comicFolder);
    }
}
