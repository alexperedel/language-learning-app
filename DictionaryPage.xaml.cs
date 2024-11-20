using Microsoft.Maui.Controls;
using System.Linq;

namespace LearningApp;

public partial class DictionaryPage : ContentPage
{
    private DatabaseService _databaseService;
    private List<Word> _words;

    public DictionaryPage()
    {
        InitializeComponent();
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "words.db3");

        _databaseService = new DatabaseService(dbPath);

        // Initialize word list
        _words = new List<Word>();
        LoadWordsAsync();
    }

    private async Task LoadWordsAsync()
    {
        _words = await _databaseService.GetWordsAsync();
        PopulateWordList(_words);
    }

    private void PopulateWordList(IEnumerable<Word> words)
    {
        if (!words.Any())
        {
            ToggleNoResultsLabel(true);
        }
        else
        {
            ToggleNoResultsLabel(false);
            wordListView.ItemsSource = words.Select(word => word.Text).ToList();
        }
    }

    private void ToggleNoResultsLabel(bool isVisible)
    {
        wordListView.ItemsSource = new List<Word>();
        noResultsLabel.IsVisible = isVisible;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchQuery = e.NewTextValue?.ToLower();
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            PopulateWordList(_words);
        }
        else
        {
            var filteredWords = _words.Where(word => word.Text.ToLower().StartsWith(searchQuery)).ToList();
            PopulateWordList(filteredWords);
        }
    }

    private async void OnWordTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item == null) return;

        wordSearchBar.Unfocus();

        var tappedWord = e.Item.ToString();
        var wordPage = new WordDefinitionPage(tappedWord, OnWordRemoved);
        await Navigation.PushModalAsync(wordPage);
    }

    private void OnWordRemoved(string removedWord)
    {
        var wordToRemove = _words.FirstOrDefault(w => w.Text == removedWord);
        if (wordToRemove != null)
        {
            _words.Remove(wordToRemove);

            if(wordSearchBar.Text != null && wordSearchBar.Text.Length > 0)
            {
                var filteredWords = _words.Where(word => word.Text.ToLower().StartsWith(wordSearchBar.Text)).ToList();
                PopulateWordList(filteredWords);
            }
            else { PopulateWordList(_words); }
        }
    }

}