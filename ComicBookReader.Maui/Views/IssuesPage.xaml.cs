using ComicBookReader.Maui.ViewModels;

namespace ComicBookReader.Maui.Views;

public partial class IssuesPage : ContentPage
{
    private readonly IssuesViewModel comicIssuesViewModel;

    public IssuesPage(IssuesViewModel comicIssuesViewModel)
	{
		InitializeComponent();
        this.comicIssuesViewModel = comicIssuesViewModel;
        
        BindingContext = comicIssuesViewModel;
    }
}