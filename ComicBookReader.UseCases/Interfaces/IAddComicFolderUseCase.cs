using ComicBookReader.CoreBusiness;

namespace ComicBookReader.UseCases.Interfaces
{
    public interface IAddComicFolderUseCase
    {
        Task ExecuteAsync(ComicFolder comicFolder);
    }
}