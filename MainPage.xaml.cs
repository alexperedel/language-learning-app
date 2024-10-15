namespace LearningApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnImageButtonClicked(object sender, EventArgs e)
        {

        }

        private async void SwitchToSearchPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }
    }

}
