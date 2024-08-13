using ComicBookReader.CoreBusiness;
using CommunityToolkit.Mvvm.ComponentModel;
using SharpCompress.Archives;
using System.Collections.ObjectModel;

namespace ComicBookReader.Maui.ViewModels
{
    public partial class IssueViewModel : ObservableObject
    {
        public ObservableCollection<byte[]> ComicPages { get; set; } = new ObservableCollection<byte[]>();

        public async Task GetIssueImagesFromArchiveAsync(string filePath)
        {
            var pageImageExtensions = new[] { ".jpg", ".jpeg", ".png" };

            ComicPages.Clear();

            using var archive = ArchiveFactory.Open(filePath);
            var entries = archive.Entries
                .Where(e => !e.IsDirectory && pageImageExtensions.Contains(Path.GetExtension(e.Key).ToLower()))
                .ToList();

            await LoadImagesAsync(entries);
        }
        
        private async Task LoadImagesAsync(List<IArchiveEntry> entries)
        {
            foreach (var entry in entries)
            {
                using var stream = entry.OpenEntryStream();
                using var memoryStream = new MemoryStream();

                await stream.CopyToAsync(memoryStream);
                ComicPages.Add(memoryStream.ToArray());
            }
        }
    }
}
