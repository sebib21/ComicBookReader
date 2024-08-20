using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharpCompress.Archives;
using System.Diagnostics;

namespace ComicBookReader.Maui.ViewModels
{
    public partial class IssueViewModel : ObservableObject
    {
        private static readonly string[] PageImageExtensions = { ".jpg", ".jpeg", ".png" };

        private int archiveEntriesNumber;
        private CancellationTokenSource? cancellationTokenSource;
        private IArchive archive;

        public bool IsPreviousButtonVisible => CurrentIndex > 0;
        public bool IsNextButtonVisible => CurrentIndex < archiveEntriesNumber - 1;

        [ObservableProperty]
        private string? comicIssueName;

        [ObservableProperty]
        private string currentPageLabel = "0 / 0";

        [ObservableProperty]
        private byte[]? currentImage;

        private int currentIndex;
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                if (SetProperty(ref currentIndex, value))
                {
                    UpdateCurrentPageLabel();
                    LoadCurrentImageAsync().ConfigureAwait(false);
                    OnPropertyChanged(nameof(IsPreviousButtonVisible));
                    OnPropertyChanged(nameof(IsNextButtonVisible));
                }
            }
        }


        [RelayCommand]
        public void MoveToNextPage()
        {
            if (CurrentIndex < archiveEntriesNumber - 1)
            {
                CurrentIndex++;
            }
        }

        [RelayCommand]
        public void MoveToPreviousPage()
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
            }
        }

        public async Task GetIssueImagesFromArchiveAsync(string filePath)
        {
            SetComicIssueName(filePath);

            CurrentImage = null;
            CurrentIndex = 0;

            archive = ArchiveFactory.Open(filePath);
            archiveEntriesNumber = GetArchiveEntriesNumber();
            
            await LoadCurrentImageAsync();
            UpdateCurrentPageLabel();

            OnPropertyChanged(nameof(IsNextButtonVisible));
        }

        private async Task LoadCurrentImageAsync()
        {
            if (CurrentIndex >= archiveEntriesNumber || CurrentIndex < 0) return;

            try
            {
                CancellationToken cancellationToken = GetCancellationToken();
                IArchiveEntry? entry = GetArchiveEntry();

                if (entry == null)
                {
                    Debug.WriteLine("Entry not found for the current index.");
                    return;
                }

                CurrentImage = await IssueViewModel.ReadEntryToByteArrayAsync(entry, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine($"OperationCanceledException: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        private static async Task<byte[]> ReadEntryToByteArrayAsync(IArchiveEntry entry, CancellationToken cancellationToken)
        {
            await using var memoryStream = new MemoryStream();

            await using var entryStream = entry.OpenEntryStream();
            await entryStream.CopyToAsync(memoryStream, cancellationToken);

            return memoryStream.ToArray();
        }

        private void UpdateCurrentPageLabel()
        {
            CurrentPageLabel = $"{CurrentIndex + 1} / {archiveEntriesNumber}";
        }

        private void SetComicIssueName(string filePath)
        {
            ComicIssueName = Path.GetFileNameWithoutExtension(filePath);
        }

        private int GetArchiveEntriesNumber()
        {
            return archive.Entries
                .Count(e => !e.IsDirectory && e.Key != null &&
                            PageImageExtensions.Contains(Path.GetExtension(e.Key).ToLower()));
        }

        private IArchiveEntry? GetArchiveEntry()
        {
            return archive.Entries
                .Where(e => !e.IsDirectory && e.Key != null &&
                    PageImageExtensions.Contains(Path.GetExtension(e.Key).ToLower()))
                .Skip(CurrentIndex)
                .FirstOrDefault();
        }

        private CancellationToken GetCancellationToken()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new();

            return cancellationTokenSource.Token;
        }
    }
}
