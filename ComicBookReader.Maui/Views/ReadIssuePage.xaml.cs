using ComicBookReader.Maui.ViewModels;

namespace ComicBookReader.Maui.Views;

[QueryProperty(nameof(IssuePath), "IssuePath")]
public partial class ReadIssuePage : ContentPage
{
    private readonly IssueViewModel issueViewModel;

    public ReadIssuePage(IssueViewModel issueViewModel)
	{
		InitializeComponent();
        this.issueViewModel = issueViewModel;

        BindingContext = issueViewModel;
    }

    public string IssuePath
    {
        set
        {
            if(!string.IsNullOrEmpty(value) && File.Exists(value))
            {
                Task.Run(() => LoadComicIssue(value));
            }
        }
    }

    private async Task LoadComicIssue(string issuePath)
    {
        await issueViewModel.GetIssueImagesFromArchiveAsync(issuePath);
    }
}