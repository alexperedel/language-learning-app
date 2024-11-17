using Microsoft.Maui.Controls;

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

        LoadWordsAsync();
    }

    private async void LoadWordsAsync()
    {
        _words = await _databaseService.GetWordsAsync();
        List<String> wordsName = new List<String>();

        if (_words.Count == 0)
        {
            noResultsLabel.IsVisible = true;
        }
        else
        {
            noResultsLabel.IsVisible = false;
            foreach (Word word in _words)
            {
                wordsName.Add(word.Text);
            }
            wordListView.ItemsSource = wordsName;
        }
    }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchQuery = wordSearchBar.Text?.ToLower();
        if (string.IsNullOrEmpty(searchQuery))
        {
            LoadWordsAsync();
        }
        else
        {
            wordListView.ItemsSource = new List<Word>();
            var resultSearchQuery = _words.Where(word => word.Text.ToLower().StartsWith(searchQuery)).ToList();

            List<String> wordsName = new List<String>();

            if (resultSearchQuery.Count == 0)
            {
                noResultsLabel.IsVisible = true;
            }
            else
            {
                noResultsLabel.IsVisible = false;
                foreach (Word word in resultSearchQuery)
                {
                    wordsName.Add(word.Text);
                }
                wordListView.ItemsSource = wordsName;  
            }

        }

        /*
        else
        {
            _words.Where(word => word.Text.ToLower().Contains(searchQuery)).ToList();

            List<String> wordsName = new List<String>();
            foreach (Word word in _words)
            {
                wordsName.Add(word.Text);
            }
            wordListView.ItemsSource = new List<Word>();
            wordListView.ItemsSource = wordsName;
        }*/
    }

    private async void OnWordTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var tappedWord = e.Item.ToString();
            await Navigation.PushModalAsync(new WordDefinitionPage(tappedWord));
            LoadWordsAsync();
            //await DisplayAlert("Word", wordDefinition ?? "Definition not found", "OK");
        }
    }

}