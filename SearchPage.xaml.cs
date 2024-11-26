using Newtonsoft.Json;

namespace LearningApp;

public partial class SearchPage : ContentPage
{
    private readonly DictionaryService _dictionaryService = new DictionaryService();
    private readonly DatabaseService _databaseService;
    private string? _currentWordText;

    public SearchPage()
    {
        InitializeComponent();
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "words.db3"); ;
        _databaseService = new DatabaseService(dbPath);
        DeviceDisplay.MainDisplayInfoChanged += OnOrientationChanged;
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
                _currentWordText = wordData.Word;
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

    private void OnOrientationChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        if (e.DisplayInfo.Orientation == DisplayOrientation.Portrait)
        {
            VisualStateManager.GoToState(MainLayout, "Portrait");
        }
        else if (e.DisplayInfo.Orientation == DisplayOrientation.Landscape)
        {
            VisualStateManager.GoToState(MainLayout, "Landscape");
        }
    }

    protected override void OnDisappearing()
    {
        DeviceDisplay.MainDisplayInfoChanged -= OnOrientationChanged;
        base.OnDisappearing();
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
            Text = "Add Word",
            Margin = new Thickness(0, 20, 0, 0),
            HorizontalOptions = LayoutOptions.Center
        };
        addWordButton.Clicked += OnAddWordButtonClicked;

        ContentArea.Children.Add(addWordButton);

    }

    private async void OnAddWordButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentWordText)) return;

        await _databaseService.AddWordAsync(_currentWordText);
        await DisplayAlert("Add Word", "The word has been added successfully!", "OK");
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string newText = e.NewTextValue;
    }
}