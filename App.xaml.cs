namespace LearningApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            var savedTheme = Preferences.Get("AppTheme", "Light");
            ApplyTheme(savedTheme);
        }

        public static void ApplyTheme(string theme)
        {
            Current.Resources.MergedDictionaries.Clear();

            if (theme == "Dark")
            {
                Current.Resources.MergedDictionaries.Add(new DarkTheme());
            }
            else
            {
                Current.Resources.MergedDictionaries.Add(new LightTheme());
            }
        }
    }
}
