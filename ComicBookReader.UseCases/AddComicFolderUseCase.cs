using ComicBookReader.CoreBusiness;
using ComicBookReader.UseCases.Interfaces;
using ComicBookReader.UseCases.PluginInterfaces;

namespace ComicBookReader.UseCases
{
    public class AddComicFolderUseCase : IAddComicFolderUseCase
    {
        private readonly IComicFolderRepository comicFolderRepository;

        public AddComicFolderUseCase(IComicFolderRepository comicFolderRepository)
        {
            this.comicFolderRepository = comicFolderRepository;
        }

        public async Task ExecuteAsync(ComicFolder comicFolder)
        {
            await comicFolderRepository.AddComicFolderAsync(comicFolder);
        }
    }
}
