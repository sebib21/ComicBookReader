namespace ComicBookReader.Maui.Helpers
{
    public class ApplicationAlertHelper
    {
        public static async Task ShowAlert(string title, string message, string cancel)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(title, message, cancel);
            }
            else
            {
                Console.WriteLine($"Failed to display alert: {title} - {message}");
            }
        }
    }
}
