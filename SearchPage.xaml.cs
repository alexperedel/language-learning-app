using Newtonsoft.Json;

namespace LearningApp;

public partial class SearchPage : ContentPage
{
    private readonly DictionaryService _dictionaryService = new DictionaryService();

    public SearchPage()
    {
        InitializeComponent();
    }

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var searchBar = (SearchBar)sender;
        string query = searchBar.Text;

        if (!string.IsNullOrEmpty(query))
        {
            var wordData = await _dictionaryService.SearchWordAsync(query);
            if (wordData != null)
            {
                DisplayWordDetails(wordData);
            }
            else
            {
                await DisplayAlert("Error", "Word not found or request failed.", "OK");
            }
        }

        searchBar.Text = string.Empty;
        searchBar.Unfocus();
    }

    private void DisplayWordDetails(Words wordData)
    {
        ContentArea.Children.Clear();

        // Display the Word
        ContentArea.Children.Add(new Label
        {
            Text = wordData.Word,
            FontAttributes = FontAttributes.Bold,
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center
        });

        // Display Phonetics
        if (wordData.Phonetics != null && wordData.Phonetics.Count > 0)
        {
            ContentArea.Children.Add(new Label
            {
                Text = "Phonetics:",
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                Margin = new Thickness(0, 10, 0, 5)
            });

            foreach (var phonetic in wordData.Phonetics)
            {
                ContentArea.Children.Add(new Label
                {
                    Text = phonetic.Text,
                    FontSize = 16
                });
            }
        }

        // Display Meanings and Definitions by Part of Speech
        if (wordData.Meanings != null && wordData.Meanings.Count > 0)
        {
            foreach (var meaning in wordData.Meanings)
            {
                ContentArea.Children.Add(new Label
                {
                    Text = char.ToUpper(meaning.PartOfSpeech[0]) + meaning.PartOfSpeech.Substring(1) + ":",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    Margin = new Thickness(0, 10, 0, 5)
                });

                // Display the first definition only
                if (meaning.Definitions != null && meaning.Definitions.Count > 0)
                {
                    ContentArea.Children.Add(new Label
                    {
                        Text = meaning.Definitions[0].DefinitionText,
                        FontSize = 16,
                        Margin = new Thickness(10, 0, 0, 5)
                    });
                }
            }
        }
    }
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string newText = e.NewTextValue;
    }
}