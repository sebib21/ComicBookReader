using ComicBookReader.Maui.Views;

namespace ComicBookReader.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(IssuesPage), typeof(IssuesPage));
            Routing.RegisterRoute(nameof(ReadIssuePage), typeof(ReadIssuePage));
        }
    }
}
