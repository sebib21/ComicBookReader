using ComicBookReader.CoreBusiness;
using ComicBookReader.UseCases.PluginInterfaces;

namespace ComicBookReader.Plugins.DataStore.InMemory
{
    public class ComicFolderInMemoryRepository : IComicFolderRepository
    {
        public static ComicFolder _comicFolder;

        public ComicFolderInMemoryRepository()
        {
            _comicFolder = new ComicFolder();
        }

        public Task AddComicFolderAsync(ComicFolder comicFolder)
        {
            _comicFolder = comicFolder;

            return Task.CompletedTask;
        }
    }
}
