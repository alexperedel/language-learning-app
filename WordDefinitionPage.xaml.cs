using System.Collections;
using System.ComponentModel.Design;

namespace LearningApp;

public partial class WordDefinitionPage : ContentPage
{
    private string? _currentWordText;
    private readonly DictionaryService _dictionaryService = new DictionaryService();
    private readonly Action<string>? _onWordRemoved;
    private readonly DatabaseService _databaseService;

    public WordDefinitionPage(string definition, Action<string> onWordRemoved = null)
    {
        InitializeComponent();
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "words.db3"); ;
        _databaseService = new DatabaseService(dbPath);
        _onWordRemoved = onWordRemoved;
        SearchDefinition(definition);
    }

    private async void SearchDefinition(string word)
    {
        string query = word;

        if (!string.IsNullOrEmpty(query))
        {
            var wordData = await _dictionaryService.SearchWordAsync(query);
            if (wordData != null)
            {
                _currentWordText = wordData.Word;
                DisplayWordDetails(wordData);
            }
            else
            {
                await DisplayAlert("Error", "Word not found or request failed.", "OK");
            }
        }
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }


    private void DisplayWordDetails(Words wordData)
    {
        ContentArea.Children.Clear();

        ContentArea.Children.Add(new Label
        {
            Text = wordData.Word,
            FontAttributes = FontAttributes.Bold,
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center
        });

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

        var addWordButton = new Button
        {
            Text = "Remove Word",
            Margin = new Thickness(0, 20, 0, 0),
            HorizontalOptions = LayoutOptions.Center
        };
        addWordButton.Clicked += OnRemovedWordButtonClicked;

        ContentArea.Children.Add(addWordButton);

    }

    private async void OnRemovedWordButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentWordText)) return;
        await _databaseService.RemoveWordAsync(_currentWordText);

        _onWordRemoved?.Invoke(_currentWordText);

        await DisplayAlert("Remove Word", "The word has been removed successfully!", "OK");
    }
}