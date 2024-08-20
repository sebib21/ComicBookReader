using ComicBookReader.CoreBusiness;
using ComicBookReader.Maui.Helpers;
using ComicBookReader.Maui.Views;
using ComicBookReader.UseCases.Interfaces;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharpCompress.Archives;
using System.Collections.ObjectModel;

namespace ComicBookReader.Maui.ViewModels
{
    public partial class IssuesViewModel : ObservableObject
    {
        private readonly IAddComicFolderUseCase addComicFolderUseCase;
        public ObservableCollection<ComicIssue> Issues { get; set; }
        private ComicFolder comicFolder;

        public ComicFolder ComicFolder
        {
            get => comicFolder;
            set
            {
                SetProperty(ref comicFolder, value);
                LoadIssuesFromFolderFiles(comicFolder.Path);
            }
        }

        public IssuesViewModel(IAddComicFolderUseCase addComicFolderUseCase)
        {
            this.addComicFolderUseCase = addComicFolderUseCase;
            comicFolder = new();
            Issues = new ObservableCollection<ComicIssue>();
        }

        [RelayCommand]
        public async Task AddPath()
        {
            try
            {
                var result = await FolderPicker.Default.PickAsync(default);

                if (result.IsSuccessful && await AreAllFolderFilesExtensionsValid(result.Folder.Path))
                {
                    await addComicFolderUseCase.ExecuteAsync(comicFolder);

                    ComicFolder = new ComicFolder
                    {
                        Path = result.Folder.Path,
                        Name = result.Folder.Name
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        public async Task GoToIssue(string issuePath)
        {
            await Shell.Current.GoToAsync($"{nameof(ReadIssuePage)}?IssuePath={issuePath}");
        }

        private static async Task<bool> AreAllFolderFilesExtensionsValid(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    await ApplicationAlertHelper.ShowAlert("Error", "The folder doesn't exist!", "OK");
                    return false;
                }

                var files = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                                .Where(file => file.EndsWith(".cbr", StringComparison.OrdinalIgnoreCase) ||
                                               file.EndsWith(".cbz", StringComparison.OrdinalIgnoreCase))
                                .ToArray();

                if (files.Length == 0)
                {
                    await ApplicationAlertHelper.ShowAlert("Error", "The folder should contain files with the extensions .cbr or .cbz!", "OK");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await ApplicationAlertHelper.ShowAlert("Error", ex.Message, "OK");
                return false;
            }

            return true;
        }

        private void LoadIssuesFromFolderFiles(string folderPath)
        {
            Issues.Clear();

            var files = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                             .Where(file => file.EndsWith(".cbr", StringComparison.OrdinalIgnoreCase) ||
                                            file.EndsWith(".cbz", StringComparison.OrdinalIgnoreCase))
                             .OrderBy(file => Path.GetFileNameWithoutExtension(file))
                             .ToArray();

            foreach (var file in files)
            {
                Issues.Add(GetComicIssueInfoFromArchive(file));
            }
        }

        private static ComicIssue GetComicIssueInfoFromArchive(string filePath)
        {
            using var archive = ArchiveFactory.Open(filePath);
            var entry = archive.Entries.FirstOrDefault(e => !e.IsDirectory);

            if (entry != null)
            {
                using var stream = entry.OpenEntryStream();
                using var memoryStream = new MemoryStream();

                stream.CopyTo(memoryStream);
                return new ComicIssue
                {
                    Name = Path.GetFileNameWithoutExtension(filePath),
                    Path = filePath,
                    Cover = memoryStream.ToArray()
                };
            }

            return new ComicIssue();
        }
    }
}
