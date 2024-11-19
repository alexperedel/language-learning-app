using Microsoft.Maui.Controls;

namespace LearningApp;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

   
        var savedTheme = Preferences.Get("AppTheme", "Light");
        ApplyTheme(savedTheme);
        themePicker.SelectedItem = savedTheme;
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        var selectedTheme = themePicker.SelectedItem?.ToString();
        if (selectedTheme != null)
        {
            Preferences.Set("AppTheme", selectedTheme);
            ApplyTheme(selectedTheme);
        }
    }
    private void ApplyTheme(string theme)
    {
        if (theme == "Dark")
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
        }
        else
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
        }
    }
}
